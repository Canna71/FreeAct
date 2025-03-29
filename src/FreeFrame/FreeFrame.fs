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

type AppDb<'T>(initialState: 'T) =
    let mutable state = initialState
    let subscribers = ResizeArray<'T -> unit>()

    let notifySubscribers () =
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

// ===== Event Handling with improved separation of concerns =====

// Define the event type - just an identifier, no handlers
type EventId<'E> = { id: string }

// Define the event handler type - connects an event to its handling logic
type EventHandler<'E, 'State> = { eventId: EventId<'E>; handler: 'E -> 'State -> 'State }

// Create an event identifier (no handler attached)
let defineEvent<'E> (id: string) : EventId<'E> = { id = id }

// Define a handler for an event
let registerHandler<'E, 'State>
    (eventId: EventId<'E>)
    (handler: 'E -> 'State -> 'State)
    : EventHandler<'E, 'State>
    =
    { eventId = eventId; handler = handler }

// Private storage for handlers
let private eventHandlers = Collections.Generic.Dictionary<string, obj -> obj>()

// Register an event handler in the system
let registerEventHandler<'E, 'T> (handler: EventHandler<'E, 'T>) =
    let wrappedHandler (payload: obj) : obj =
        let typedPayload = payload :?> 'E
        let reducer = (fun (state: 'T) -> handler.handler typedPayload state) :> obj
        reducer

    eventHandlers.[handler.eventId.id] <- wrappedHandler

// Dispatch an event with payload - now only needs the event ID and payload
let dispatch<'E, 'T> (appDb: IAppDb<'T>) (eventId: EventId<'E>) (payload: 'E) =
    match eventHandlers.TryGetValue(eventId.id) with
    | true, handler ->
        let action = handler (payload :> obj)
        appDb.Dispatch(action)
    | false, _ -> console.error ($"No handler registered for event: {eventId.id}")

// ===== Backward Compatibility =====

// For compatibility with previous code
type EventDef<'E, 'State> = EventHandler<'E, 'State>

// Create event with handler in one step (for backward compatibility)
let createEvent<'E, 'State>
    (id: string)
    (handler: 'E -> 'State -> 'State)
    : EventHandler<'E, 'State>
    =
    let eventId = defineEvent<'E> id
    registerHandler eventId handler

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

// React hook to use a subscription in a React component
let useSubscription<'V> (subscription: ISubscription<'V>) =
    let initialValue = subscription.Value
    let state = Hooks.useState (initialValue)
    let value = state.current

    // Use the direct value update signature, not the function signature
    let setState = fun (newValue: 'V) -> state.update newValue

    Hooks.useEffectDisposable (
        (fun () ->
            let dispose = subscription.Subscribe(setState)

            dispose
        ),
        [| subscription :> obj |]
    )

    value
