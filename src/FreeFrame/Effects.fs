// =====================================================
//             Effects (side effects)
// =====================================================
namespace FreeFrame

open System
open Fable.Core.JS

// A type to represent an effect
type Effect = Effect of id: string * payload: obj

// A type to represent the result of an effect
type EffectResult =
    | Success of obj
    | Failure of exn

//  EffectId implementation
type EffectId<'Payload, 'Result> = private EffectId of string

// Effect handler type
type EffectHandler<'Payload, 'Result> = 'Payload -> Async<'Result>

[<AutoOpen>]
// Module to encapsulate EffectId operations
module EffectId =
    // Internal helper to create an EffectId
    let internal create<'Payload, 'Result> (id: string) : EffectId<'Payload, 'Result> = EffectId id

    // Internal helper to extract the key from an EffectId
    let internal key (EffectId id) = id

    // Public API for creating effects
    let inline named<'Payload, 'Result> (id: string) : EffectId<'Payload, 'Result> = EffectId id

    let inline auto<'Payload, 'Result> () : EffectId<'Payload, 'Result> =
        EffectId(Guid.NewGuid().ToString())

[<RequireQualifiedAccess>]
module Effects =
    open FreeFrame

    // Private storage for effect handlers
    let internal effectHandlers =
        Collections.Generic.Dictionary<string, obj -> Async<obj>>()

    // Register an effect handler
    let registerHandler<'Payload, 'Result>
        (effectId: EffectId<'Payload, 'Result>)
        (handler: EffectHandler<'Payload, 'Result>)
        =
        let wrappedHandler (payload: obj) : Async<obj> =
            async {
                let typedPayload = payload :?> 'Payload
                let! result = handler typedPayload
                return result :> obj
            }

        effectHandlers.[EffectId.key effectId] <- wrappedHandler

    let registerNamed<'Payload, 'Result>
        (effectName: string)
        (handler: EffectHandler<'Payload, 'Result>)
        =
        let effectId = EffectId.named<'Payload, 'Result> effectName
        registerHandler effectId handler
        effectId

    /// Creates an async computation that will execute the registered effect handler.
    /// Returns a Result containing either the handler's result or an error if:
    /// - No handler is registered for the given effectId
    /// - The handler throws an exception during execution
    /// - Type conversion of the payload or result fails
    let createEffectTask<'Payload, 'Result>
        (effectId: EffectId<'Payload, 'Result>)
        (payload: 'Payload)
        =
        async {
            match effectHandlers.TryGetValue(EffectId.key effectId) with
            | true, handler ->
                try
                    let! result = handler (payload :> obj)
                    let typedResult = result :?> 'Result
                    return Ok typedResult
                with ex ->
                    return Error ex
            | false, _ ->
                let error = Exception($"No handler registered for effect {EffectId.key effectId}")
                console.error (error.Message)
                return Error error
        }

    // =====================================================
    //             Effect Composition
    // =====================================================

    // Chain two effects where the second effect depends on the result of the first
    let chain<'PayloadA, 'ResultA, 'PayloadB, 'ResultB>
        (effect1: EffectId<'PayloadA, 'ResultA>)
        (payload1: 'PayloadA)
        (mapResult: 'ResultA -> 'PayloadB)
        (effect2: EffectId<'PayloadB, 'ResultB>)
        : Async<Result<'ResultB, exn>>
        =

        async {
            // Run the first effect
            let! result1 = createEffectTask effect1 payload1

            // If the first effect succeeds, run the second
            match result1 with
            | Ok resultA ->
                let payload2 = mapResult resultA
                return! createEffectTask effect2 payload2
            | Error e -> return Error e
        }

    // Run two effects in parallel and combine their results
    let combine<'PayloadA, 'ResultA, 'PayloadB, 'ResultB, 'Combined>
        (effect1: EffectId<'PayloadA, 'ResultA>)
        (payload1: 'PayloadA)
        (effect2: EffectId<'PayloadB, 'ResultB>)
        (payload2: 'PayloadB)
        (combiner: 'ResultA -> 'ResultB -> 'Combined)
        : Async<Result<'Combined, exn>>
        =

        async {
            // Create the async tasks but don't await them yet
            let task1 = createEffectTask effect1 payload1
            let task2 = createEffectTask effect2 payload2

            let! childA = Async.StartChild task1
            let! childB = Async.StartChild task2

            let! resultA = childA
            let! resultB = childB

            match resultA, resultB with
            | Ok r1, Ok r2 -> return Ok(combiner r1 r2)
            | Error e, _ -> return Error e
            | _, Error e -> return Error e

        }

    let runEffectWithCallback<'Payload, 'Result>
        (effectId: EffectId<'Payload, 'Result>)
        (payload: 'Payload)
        (callback: Result<'Result, exn> -> unit)
        =
        try
            match effectHandlers.TryGetValue(EffectId.key effectId) with
            | true, handler ->
                printfn "Running effect %s with payload %A" (EffectId.key effectId) payload

                async {
                    let! result = handler (payload :> obj)
                    callback (result :?> 'Result |> Ok)
                }
                |> Async.StartImmediate

                Ok(())
            | false, _ ->
                printfn "No handler registered for effect %s" (EffectId.key effectId)
                let error = Exception($"No handler registered for effect {EffectId.key effectId}")
                console.error (error.Message)
                Error error
        with ex ->
            Error ex

    // Async version - ignores the result
    let runEffect<'Payload> (effectId: EffectId<'Payload, unit>) (payload: 'Payload) =
        try
            match effectHandlers.TryGetValue(EffectId.key effectId) with
            | true, handler ->
                printfn "Running effect %s with payload %A" (EffectId.key effectId) payload

                async {
                    let! _ = handler (payload :> obj)
                    return ()
                }
                |> Async.StartImmediate

                Ok(())
            | false, _ ->
                printfn "No handler registered for effect %s" (EffectId.key effectId)
                let error = Exception($"No handler registered for effect {EffectId.key effectId}")
                console.error (error.Message)
                Error error
        with ex ->
            Error ex

    // Run multiple async effects in parallel
    let runParallelAsyncEffects (effects: (unit -> Async<'T>) list) : Async<'T[]> =
        effects |> List.map (fun effect -> effect ()) |> Async.Parallel

    // Helper to dispatch an event after an effect completes - using Result
    let dispatchAfterEffect<'Payload, 'Result, 'EventPayload, 'State>
        (appDb: IAppDb<'State>)
        (effectId: EffectId<'Payload, 'Result>)
        (payload: 'Payload)
        (mapToEvent: Result<'Result, exn> -> EventId<'EventPayload> option * 'EventPayload option)
        =
        async {
            let! result = createEffectTask effectId payload

            match result with
            | Ok _ ->
                // Dispatch the event if the effect was successful
                let eventIdOpt, eventPayloadOpt = mapToEvent result

                match eventIdOpt, eventPayloadOpt with
                | Some eventId, Some eventPayload -> dispatch appDb eventId eventPayload
                | _ -> () // No event to dispatch
            | Error ex ->
                // Handle the error case if needed
                console.error (sprintf "Effect failed: %s" ex.Message)
        // Optionally dispatch an error event or handle it in some way

        }
        |> Async.StartImmediate

// Simplified version that just passes through the result without managing loading state
// let useEffectSimple<'Payload, 'Result> (effectId: EffectId<'Payload, 'Result>) (payload: 'Payload) =
//     let resultState = Hooks.useState<Result<'Result, exn> option> None

//     Hooks.useEffect (
//         (fun () ->
//           runEffect effectId payload (fun result -> resultState.update (Some result))
//         ),
//         [| box payload |]
//     )

//     resultState.current

// Original hook for handling effects in React components (maintained for backward compatibility)
// let useFreeFrameEffect<'Payload, 'Result>
//     (effectId: EffectId<'Payload, 'Result>)
//     (payload: 'Payload)
//     =
//     let loadingState = Hooks.useState true
//     let resultState = Hooks.useState<Result<'Result, exn> option> None

//     Hooks.useEffect (
//         (fun () ->
//             runEffect
//                 effectId
//                 payload
//                 (fun result ->
//                     resultState.update (Some result)
//                     loadingState.update false
//                 )
//         ),
//         [| box payload |]
//     )

//     loadingState.current, resultState.current

// Custom version for discriminated union loading state
// let useEffectWithUnionState<'Payload, 'Result, 'LoadingState>
//     (effectId: EffectId<'Payload, 'Result>)
//     (payload: 'Payload)
//     (initialState: 'LoadingState)
//     (onResult: Result<'Result, exn> -> 'LoadingState)
//     =
//     let stateHook = Hooks.useState initialState
//     let resultState = useEffectSimple effectId payload

//     Hooks.useEffect (
//         (fun () ->
//             match resultState with
//             | Some result -> stateHook.update (onResult result)
//             | None -> stateHook.update initialState
//         ),
//         [| box resultState |]
//     )

//     stateHook.current, resultState

// React hook for chained effects
// let useChainedEffect<'PayloadA, 'ResultA, 'PayloadB, 'ResultB>
//     (effect1: EffectId<'PayloadA, 'ResultA>)
//     (payload1: 'PayloadA)
//     (createPayload2: 'ResultA -> 'PayloadB)
//     (effect2: EffectId<'PayloadB, 'ResultB>)
//     =

//     let loadingState = Hooks.useState true
//     let resultState = Hooks.useState<Result<'ResultB, exn> option> None

//     Hooks.useEffect (
//         (fun () ->
//             loadingState.update true

//             async {
//                 let! result = chainEffect effect1 payload1 createPayload2 effect2

//                 // Update state with the result
//                 Browser.Dom.window.setTimeout (
//                     (fun () ->
//                         resultState.update (Some result)
//                         loadingState.update false
//                     ),
//                     0
//                 )
//                 |> ignore
//             }
//             |> Async.StartImmediate
//         ),
//         [| box payload1 |]
//     )

//     loadingState.current, resultState.current

// React hook for combined effects
// let useCombinedEffects<'PayloadA, 'ResultA, 'PayloadB, 'ResultB, 'Combined>
//     (effect1: EffectId<'PayloadA, 'ResultA>)
//     (payload1: 'PayloadA)
//     (effect2: EffectId<'PayloadB, 'ResultB>)
//     (payload2: 'PayloadB)
//     (combiner: 'ResultA -> 'ResultB -> 'Combined)
//     =

//     let loadingState = Hooks.useState true
//     let resultState = Hooks.useState<Result<'Combined, exn> option> None

//     Hooks.useEffect (
//         (fun () ->
//             loadingState.update true

//             async {
//                 let! result = combineEffects effect1 payload1 effect2 payload2 combiner

//                 // Update state with the result
//                 Browser.Dom.window.setTimeout (
//                     (fun () ->
//                         resultState.update (Some result)
//                         loadingState.update false
//                     ),
//                     0
//                 )
//                 |> ignore
//             }
//             |> Async.StartImmediate
//         ),
//         [| box payload1; box payload2 |]
//     )

//     loadingState.current, resultState.current
