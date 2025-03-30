module FreeAct.FreeFrame

open System
open Fable.Core
open Browser
open Fable.React

// ===== Application State (app-db) =====

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

// ===== Improved Event Handling =====

// Simplified EventId implementation
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
    // let eventId = EventId.ofType<'EventType> ()
    dispatchInternal appDb (EventId.typeKey<'EventType> ()) payload

// ===== Backward Compatibility =====

// For compatibility with previous code
// type EventDef<'Payload, 'State> = EventHandler<'Payload, 'State>

// Create event with handler in one step (for backward compatibility)
// let createEvent<'Payload, 'State>
//     (id: string)
//     (handler: 'Payload -> 'State -> 'State)
//     : EventHandler<'Payload, 'State>
//     =
//     let eventId = defineEvent<'Payload> id
//     registerHandler eventId handler

// ===== Subscriptions (Views on app-db) =====

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
