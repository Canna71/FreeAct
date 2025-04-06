module FreeAct.FreeFrame

open System
open Fable.Core
open Browser
open Fable.React

// =================================================
//            Application State (app-db)
// =================================================

// Generic state container with type safety
type IAppDb<'T> =
    abstract member GetState: unit -> 'T
    abstract member Subscribe: ('T -> unit) -> IDisposable
    abstract member Dispatch: obj -> unit
    abstract member ForceRefresh: unit -> unit
    abstract member BatchProcess: (unit -> unit) -> unit // New method for batch processing

type AppDb<'T>(initialState: 'T) =
    let mutable state = initialState
    let subscribers = ResizeArray<'T -> unit>()
    let mutable batchProcessing = false
    let pendingReducers = ResizeArray<'T -> 'T>()
    let mutable processingScheduled = false

    let processReducers () =
        if pendingReducers.Count > 0 then
            // Apply all pending reducers
            let finalState = pendingReducers |> Seq.fold (fun s reducer -> reducer s) state

            // Clear the queue
            pendingReducers.Clear()
            processingScheduled <- false

            // Update state and notify if changed
            if not (Object.Equals(state, finalState)) then
                state <- finalState
                // Notify subscribers only if not in batch mode
                if not batchProcessing then
                    for subscriber in subscribers do
                        subscriber state

    let scheduleProcessing () =
        if not processingScheduled then
            processingScheduled <- true
            Browser.Dom.window.requestAnimationFrame (fun _ -> processReducers ()) |> ignore

    interface IAppDb<'T> with
        member _.GetState() = state

        member _.Subscribe(callback) =
            subscribers.Add(callback)

            { new IDisposable with
                member _.Dispose() = subscribers.Remove(callback) |> ignore
            }

        member _.Dispatch(action) =
            match action with
            | :? ('T -> 'T) as reducer ->
                pendingReducers.Add reducer
                scheduleProcessing ()
            | _ -> console.error "Invalid action type dispatched to AppDb"

        member _.ForceRefresh() =
            for subscriber in subscribers do
                subscriber state

        member _.BatchProcess(action) =
            batchProcessing <- true

            try
                action ()
                // Schedule the processing of all accumulated reducers
                scheduleProcessing ()
            finally
                batchProcessing <- false

// A single notification will be sent after all dispatches

// =================================================
//             Events (event system)
// =================================================

//  EventId implementation
type EventId<'Payload> = private EventId of string

// Module to encapsulate EventId operations
module EventId =
    // Internal helper to create an EventId
    let internal create<'Payload> (id: string) : EventId<'Payload> = EventId id

    // Internal helper to extract the key from an EventId
    let internal key (EventId id) = id

    let inline internal typeKey<'EventType> () = typeof<'EventType>.ToString()
    // Public API for creating events
    let inline named<'Payload> (id: string) : EventId<'Payload> = EventId id
    let inline auto<'Payload> () : EventId<'Payload> = EventId(Guid.NewGuid().ToString())
    let inline ofType<'EventType> () : EventId<'EventType> = EventId(typeKey<'EventType> ())

// Private storage for handlers - use string as the key
let private eventHandlers = Collections.Generic.Dictionary<string, obj -> obj>()

type EventHandler<'Payload, 'State> = 'Payload -> 'State -> 'State

// Event handler definition
type EventHandlerRegistration<'Payload, 'State> =
    { eventId: EventId<'Payload>; handler: EventHandler<'Payload, 'State> }

// Register an event handler
let registerNamedEventHandler<'Payload, 'State>
    (eventId: EventId<'Payload>)
    (handler: EventHandler<'Payload, 'State>)
    =
    let wrappedHandler (payload: obj) : obj =
        let typedPayload = payload :?> 'Payload
        let reducer = (fun (state: 'State) -> handler typedPayload state) :> obj
        reducer

    console.log ($"Registering event handler for {EventId.key eventId}")
    eventHandlers.[EventId.key eventId] <- wrappedHandler

let inline registerTypedEventHandler<'EventType, 'State>
    (handler: EventHandler<'EventType, 'State>)
    =
    let eventId = EventId.ofType<'EventType> ()
    registerNamedEventHandler eventId handler

/// Helper to create an event handler that operates on a subset of the state
let focusHandler<'Payload, 'State, 'SubState>
    (lens: 'State -> 'SubState)
    (setLens: 'SubState -> 'State -> 'State)
    (handler: 'Payload -> 'SubState -> 'SubState)
    : EventHandler<'Payload, 'State>
    =
    fun payload state ->
        let subState = lens state
        let newSubState = handler payload subState
        setLens newSubState state

let internal dispatchInternal<'Payload, 'State>
    (appDb: IAppDb<'State>)
    (eventId: string)
    (payload: 'Payload)
    =
    match eventHandlers.TryGetValue(eventId) with
    | true, handler ->
        console.log ($"Dispatching event {eventId} with payload {payload} to handler {handler}")
        let action = handler (payload :> obj)
        appDb.Dispatch(action)
    | false, _ -> console.error ($"No handler registered for event")

// Dispatch an event with payload
let dispatch<'Payload, 'State>
    (appDb: IAppDb<'State>)
    (eventId: EventId<'Payload>)
    (payload: 'Payload)
    =
    dispatchInternal appDb (EventId.key eventId) payload

// Improved batch dispatch function
let batchDispatch<'State> (appDb: IAppDb<'State>) (dispatches) =
    appDb.BatchProcess(fun () ->
        // Execute all dispatches without triggering notifications
        for (eventId, payload) in dispatches do
            dispatch appDb (unbox eventId) payload
    )

let inline dispatchTyped<'EventType, 'State> (appDb: IAppDb<'State>) (payload: 'EventType) =
    dispatchInternal appDb (EventId.typeKey<'EventType> ()) payload

// =======================================================
//             Subscriptions (views on app-db)
// =======================================================

type ISubscription<'V> =
    abstract member Value: 'V
    abstract member Subscribe: ('V -> unit) -> IDisposable

// Create a derived view of app-db
let createSubscription<'T, 'V> (appDb: IAppDb<'T>) (selector: 'T -> 'V) =
    let mutable currentValue = selector (appDb.GetState())
    let subscribers = ResizeArray<'V -> unit>()

    let subscription =
        { new ISubscription<'V> with
            member _.Value = currentValue

            member _.Subscribe(callback) =
                subscribers.Add(callback)

                { new IDisposable with
                    member _.Dispose() = subscribers.Remove(callback) |> ignore
                }
        }

    // Subscribe to app-db changes
    appDb.Subscribe(fun newState ->
        let newValue = selector newState
        // Only notify if the value has changed (simple equality check)
        if not (Object.Equals(currentValue, newValue)) then
            currentValue <- newValue
            // Notify all subscribers
            for subscriber in subscribers do
                subscriber newValue
    )
    |> ignore

    subscription

// =====================================================
//             Subscription Composition
// =====================================================

// Combine two subscriptions into a new one
let combineSubscriptions<'A, 'B, 'C>
    (subA: ISubscription<'A>)
    (subB: ISubscription<'B>)
    (combiner: 'A -> 'B -> 'C)
    : ISubscription<'C>
    =

    let mutable currentValueA = subA.Value
    let mutable currentValueB = subB.Value
    let mutable currentValueC = combiner currentValueA currentValueB
    let subscribers = ResizeArray<'C -> unit>()

    let subscription =
        { new ISubscription<'C> with
            member _.Value = currentValueC

            member _.Subscribe(callback) =
                subscribers.Add(callback)

                { new IDisposable with
                    member _.Dispose() = subscribers.Remove(callback) |> ignore
                }
        }

    // Subscribe to first subscription
    let disposeA =
        subA.Subscribe(fun newValueA ->
            currentValueA <- newValueA
            let newValueC = combiner currentValueA currentValueB

            if not (Object.Equals(currentValueC, newValueC)) then
                currentValueC <- newValueC

                for subscriber in subscribers do
                    subscriber newValueC
        )

    // Subscribe to second subscription
    let disposeB =
        subB.Subscribe(fun newValueB ->
            currentValueB <- newValueB
            let newValueC = combiner currentValueA currentValueB

            if not (Object.Equals(currentValueC, newValueC)) then
                currentValueC <- newValueC

                for subscriber in subscribers do
                    subscriber newValueC
        )

    // Add a special handler to dispose both subscriptions
    { new ISubscription<'C> with
        member _.Value = currentValueC

        member _.Subscribe(callback) =
            let innerSub = subscription.Subscribe(callback)

            { new IDisposable with
                member _.Dispose() =
                    innerSub.Dispose()

                    // If we're the last subscriber, clean up the source subscriptions
                    if subscribers.Count = 0 then
                        disposeA.Dispose()
                        disposeB.Dispose()
            }
    }

// Combine three subscriptions
let combine3Subscriptions<'A, 'B, 'C, 'D>
    (subA: ISubscription<'A>)
    (subB: ISubscription<'B>)
    (subC: ISubscription<'C>)
    (combiner: 'A -> 'B -> 'C -> 'D)
    : ISubscription<'D>
    =

    // First combine A and B
    let subAB = combineSubscriptions subA subB (fun a b -> (a, b))

    // Then combine the result with C
    combineSubscriptions subAB subC (fun (a, b) c -> combiner a b c)

// Map a subscription to a new type
let mapSubscription<'A, 'B>
    (subscription: ISubscription<'A>)
    (mapper: 'A -> 'B)
    : ISubscription<'B>
    =
    let mutable currentValueA = subscription.Value
    let mutable currentValueB = mapper currentValueA
    let subscribers = ResizeArray<'B -> unit>()
    let mutable sourceSubscription: IDisposable option = None

    let ensureSourceSubscription () =
        if sourceSubscription.IsNone then
            sourceSubscription <-
                Some(
                    subscription.Subscribe(fun newValueA ->
                        currentValueA <- newValueA
                        let newValueB = mapper newValueA

                        if not (Object.Equals(currentValueB, newValueB)) then
                            currentValueB <- newValueB

                            for subscriber in subscribers do
                                subscriber newValueB
                    )
                )

    { new ISubscription<'B> with
        member _.Value = currentValueB

        member _.Subscribe(callback) =
            ensureSourceSubscription ()
            subscribers.Add(callback)

            { new IDisposable with
                member _.Dispose() =
                    subscribers.Remove(callback) |> ignore
                    // Only dispose source subscription if no subscribers left
                    if subscribers.Count = 0 then
                        sourceSubscription
                        |> Option.iter (fun sub ->
                            sub.Dispose()
                            sourceSubscription <- None
                        )
            }
    }

// Filter a subscription
let filterSubscription<'A>
    (subscription: ISubscription<'A>)
    (predicate: 'A -> bool)
    : ISubscription<'A option>
    =

    mapSubscription
        subscription
        (fun value ->
            if predicate value then
                Some value
            else
                None
        )

// =====================================================
//             Effects (side effects)
// =====================================================

// A type to represent an effect
type Effect = Effect of id: string * payload: obj

// A type to represent the result of an effect
type EffectResult =
    | Success of obj
    | Failure of exn

//  EffectId implementation
type EffectId<'Payload, 'Result> = private EffectId of string

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

// Effect handler type
type EffectHandler<'Payload, 'Result> = 'Payload -> Async<'Result>

// Private storage for effect handlers
let private effectHandlers =
    Collections.Generic.Dictionary<string, obj -> Async<obj>>()

// Register an effect handler
let registerEffectHandler<'Payload, 'Result>
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

let registerNamedEffectHandler<'Payload, 'Result>
    (effectName: string)
    (handler: EffectHandler<'Payload, 'Result>)
    =
    let effectId = EffectId.named<'Payload, 'Result> effectName
    registerEffectHandler effectId handler
    effectId

// Execute an effect - returns Result<'Success, exn> to be more F# idiomatic
let runEffect<'Payload, 'Result> (effectId: EffectId<'Payload, 'Result>) (payload: 'Payload) =
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

// Async version - ignores the result
let runEffectAsync<'Payload> (effectId: EffectId<'Payload, unit>) (payload: 'Payload) =
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
        let! result = runEffect effectId payload

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

// =====================================================
//             Effect Composition
// =====================================================

// Chain two effects where the second effect depends on the result of the first
let chainEffects<'PayloadA, 'ResultA, 'PayloadB, 'ResultB>
    (effect1: EffectId<'PayloadA, 'ResultA>)
    (payload1: 'PayloadA)
    (mapResult: 'ResultA -> 'PayloadB)
    (effect2: EffectId<'PayloadB, 'ResultB>)
    : Async<Result<'ResultB, exn>>
    =

    async {
        // Run the first effect
        let! result1 = runEffect effect1 payload1

        // If the first effect succeeds, run the second
        match result1 with
        | Ok resultA ->
            let payload2 = mapResult resultA
            return! runEffect effect2 payload2
        | Error e -> return Error e
    }

// Run two effects in parallel and combine their results
let combineEffects<'PayloadA, 'ResultA, 'PayloadB, 'ResultB, 'Combined>
    (effect1: EffectId<'PayloadA, 'ResultA>)
    (payload1: 'PayloadA)
    (effect2: EffectId<'PayloadB, 'ResultB>)
    (payload2: 'PayloadB)
    (combiner: 'ResultA -> 'ResultB -> 'Combined)
    : Async<Result<'Combined, exn>>
    =

    async {
        // Create the async tasks but don't await them yet
        let task1 = runEffect effect1 payload1
        let task2 = runEffect effect2 payload2

        let! childA = Async.StartChild task1
        let! childB = Async.StartChild task2

        let! resultA = childA
        let! resultB = childB

        match resultA, resultB with
        | Ok r1, Ok r2 -> return Ok(combiner r1 r2)
        | Error e, _ -> return Error e
        | _, Error e -> return Error e

    }

// ===== React Integration =====

// React hook to use a subscription in a React component with immediate subscription
let useSubscription<'V> (subscription: ISubscription<'V>) =
    let initialValue = subscription.Value

    // Create the state hook with the initial value
    let state = Hooks.useState (initialValue)

    // Subscribe immediately using useRef to manage the subscription reference
    let subscriptionRef = Hooks.useRef (None)

    // If we don't have a subscription yet, create one immediately
    if subscriptionRef.current.IsNone then
        // Function to update the React state when subscription value changes
        let setState = fun (newValue: 'V) -> state.update (newValue)

        // Create the subscription right away
        let dispose = subscription.Subscribe(setState)

        // Store the dispose function in the ref
        subscriptionRef.current <- Some dispose

    // Use Effect for cleanup only
    Hooks.useEffectDisposable (
        (fun () ->
            // Return the dispose function for cleanup
            { new IDisposable with
                member _.Dispose() =
                    console.log ("Disposing of subscriptionRef for ", subscription)

                    match subscriptionRef.current with
                    | Some dispose ->
                        dispose.Dispose()
                        subscriptionRef.current <- None
                    | None -> ()
            }
        ),
        [| subscription :> obj |]
    )

    // Return the current value from state
    state.current

// creates a new subscription based on the selector
let useView<'T, 'V> (appDb: IAppDb<'T>) (selector: 'T -> 'V) : 'V =
    // Create a subscription using the selector, storing it in a ref and disposing it
    // when the component unmounts
    let subscriptionRef = Hooks.useRef (None)

    if subscriptionRef.current.IsNone then
        // Function to update the React state when subscription value changes
        // Create the subscription right away
        let subscription = createSubscription appDb selector
        // Store the subscription in the ref
        subscriptionRef.current <- Some subscription

    useSubscription subscriptionRef.current.Value

// Return the subscription object

// React hook for combined subscription
let useCombinedSubscription<'A, 'B, 'C>
    (subA: ISubscription<'A>)
    (subB: ISubscription<'B>)
    (combiner: 'A -> 'B -> 'C)
    =

    let combinedSub = combineSubscriptions subA subB combiner
    useSubscription combinedSub

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
