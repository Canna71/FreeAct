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

    let notifySubscribers () =
        if not batchProcessing then
            for subscriber in subscribers do
                subscriber state

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
                state <- reducer state
                notifySubscribers ()
            | _ -> console.error ("Invalid action type dispatched to AppDb")

        member _.ForceRefresh() =
            for subscriber in subscribers do
                subscriber state

        member _.BatchProcess(action) =
            // Set batch processing flag to temporarily disable notifications
            batchProcessing <- true

            try
                // Execute the batch action
                action ()
            finally
                // Restore normal behavior and notify subscribers once
                batchProcessing <- false

                for subscriber in subscribers do
                    subscriber state

// Improved batch dispatch function
let batchDispatch<'State> (appDb: IAppDb<'State>) (dispatches: (unit -> unit) list) =
    appDb.BatchProcess(fun () ->
        // Execute all dispatches without triggering notifications
        for dispatchFn in dispatches do
            dispatchFn ()
    )
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

    eventHandlers.[EventId.key eventId] <- wrappedHandler

let inline registerTypedEventHandler<'EventType, 'State>
    (handler: EventHandler<'EventType, 'State>)
    =
    let eventId = EventId.ofType<'EventType> ()
    registerNamedEventHandler eventId handler

let internal dispatchInternal<'Payload, 'State>
    (appDb: IAppDb<'State>)
    (eventId: string)
    (payload: 'Payload)
    =
    match eventHandlers.TryGetValue(eventId) with
    | true, handler ->
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
                    match subscriptionRef.current with
                    | Some dispose -> dispose.Dispose()
                    | None -> ()
            }
        ),
        [| subscription :> obj |]
    )

    // Return the current value from state
    state.current

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

// Execute an effect - returns Result<'Success, exn> to be more F# idiomatic
let runEffect<'Payload, 'Result>
    (effectId: EffectId<'Payload, 'Result>)
    (payload: 'Payload)
    (callback: Result<'Result, exn> -> unit)
    =
    async {
        match effectHandlers.TryGetValue(EffectId.key effectId) with
        | true, handler ->
            try
                let! result = handler (payload :> obj)
                let typedResult = result :?> 'Result
                callback (Ok typedResult)
            with ex ->
                callback (Error ex)
        | false, _ ->
            let error = Exception($"No handler registered for effect {EffectId.key effectId}")
            console.error (error.Message)
            callback (Error error)
    }
    |> Async.StartImmediate

// Version that returns a promise with Result
let runEffectAsPromise<'Payload, 'Result>
    (effectId: EffectId<'Payload, 'Result>)
    (payload: 'Payload)
    : JS.Promise<Result<'Result, exn>>
    =
    Async.StartAsPromise(
        async {
            try
                match effectHandlers.TryGetValue(EffectId.key effectId) with
                | true, handler ->
                    let! result = handler (payload :> obj)
                    return Ok(result :?> 'Result)
                | false, _ ->
                    let error =
                        Exception($"No handler registered for effect {EffectId.key effectId}")

                    console.error (error.Message)
                    return Error error
            with ex ->
                return Error ex
        }
    )

// Async version - more F# idiomatic
let runEffectAsync<'Payload, 'Result>
    (effectId: EffectId<'Payload, 'Result>)
    (payload: 'Payload)
    : Async<Result<'Result, exn>>
    =
    async {
        try
            match effectHandlers.TryGetValue(EffectId.key effectId) with
            | true, handler ->
                let! result = handler (payload :> obj)
                return Ok(result :?> 'Result)
            | false, _ ->
                let error = Exception($"No handler registered for effect {EffectId.key effectId}")
                console.error (error.Message)
                return Error error
        with ex ->
            return Error ex
    }

// Chain multiple effects together - simplified without unnecessary appDb parameter
let chainEffects (effects: (unit -> unit) list) =
    for effect in effects do
        effect ()

// Helper to dispatch an event after an effect completes - using Result
let dispatchAfterEffect<'Payload, 'Result, 'EventPayload, 'State>
    (appDb: IAppDb<'State>)
    (effectId: EffectId<'Payload, 'Result>)
    (payload: 'Payload)
    (onResult: Result<'Result, exn> -> EventId<'EventPayload> option * 'EventPayload option)
    =
    runEffect
        effectId
        payload
        (fun result ->
            let (eventIdOpt, eventPayloadOpt) = onResult result

            match eventIdOpt, eventPayloadOpt with
            | Some eventId, Some eventPayload -> dispatch appDb eventId eventPayload
            | _ -> () // No event to dispatch
        )

// Hook for handling effects in React components - using Result
let useEffect<'Payload, 'Result> (effectId: EffectId<'Payload, 'Result>) (payload: 'Payload) =
    let loadingState = Hooks.useState true
    let resultState = Hooks.useState<Result<'Result, exn> option> None

    Hooks.useEffect (
        (fun () ->
            runEffect
                effectId
                payload
                (fun result ->
                    resultState.update (Some result)
                    loadingState.update false
                )
        ),
        [| box payload |]
    )

    loadingState.current, resultState.current

// Hook for handling effects that automatically retrigger on dependencies - using Result
let useEffectWithDeps<'Payload, 'Result>
    (effectId: EffectId<'Payload, 'Result>)
    (payloadFn: unit -> 'Payload)
    (dependencies: obj array)
    =
    let loadingState = Hooks.useState true
    let resultState = Hooks.useState<Result<'Result, exn> option> None

    Hooks.useEffect (
        (fun () ->
            loadingState.update true

            runEffect
                effectId
                (payloadFn ())
                (fun result ->
                    resultState.update (Some result)
                    loadingState.update false
                )
        ),
        dependencies
    )

    loadingState.current, resultState.current
