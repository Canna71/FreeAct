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
    abstract member ForceRefresh: unit -> unit // New method to force refresh

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

        member _.ForceRefresh() = notifySubscribers ()

// Batch dispatch to execute multiple dispatches and force a refresh at the end
let batchDispatch<'State> (appDb: IAppDb<'State>) (dispatches: (unit -> unit) list) =
    // Execute all dispatches
    for dispatchFn in dispatches do
        dispatchFn ()

    // Force a refresh after all dispatches
    appDb.ForceRefresh()

// ===== Improved Event Handling =====

// Internal event identifier (not exposed to users)
type private EventKey =
    | StringKey of string
    | TypeKey of Type * string // Type + a string identifier for the union case
    | AutoKey of Guid // Auto-generated unique ID

// Public event identifier - generic on the payload type
type EventId<'Payload> = private { key: EventKey }

// Create a string-based event
let defineEvent<'Payload> (id: string) : EventId<'Payload> = { key = StringKey id }

// Create an auto-generated event ID
let defineAutoEvent<'Payload> () : EventId<'Payload> = { key = AutoKey(Guid.NewGuid()) }

// Create an event ID for a specific union case - with a manually provided name
let defineUnionCaseEvent<'Union> (name: string) : EventId<'Union> =
    { key = TypeKey(typeof<'Union>, name) }

// Private storage for handlers
let private eventHandlers = Collections.Generic.Dictionary<EventKey, obj -> obj>()

// Event handler definition
type EventHandler<'Payload, 'State> =
    { eventId: EventId<'Payload>; handler: 'Payload -> 'State -> 'State }

// Register a handler for an event
let registerHandler<'Payload, 'State>
    (eventId: EventId<'Payload>)
    (handler: 'Payload -> 'State -> 'State)
    : EventHandler<'Payload, 'State>
    =
    { eventId = eventId; handler = handler }

// Register an event handler
let registerEventHandler<'Payload, 'State> (handler: EventHandler<'Payload, 'State>) =
    let wrappedHandler (payload: obj) : obj =
        let typedPayload = payload :?> 'Payload
        let reducer = (fun (state: 'State) -> handler.handler typedPayload state) :> obj
        reducer

    eventHandlers.[handler.eventId.key] <- wrappedHandler

// Dispatch an event with payload
let dispatch<'Payload, 'State>
    (appDb: IAppDb<'State>)
    (eventId: EventId<'Payload>)
    (payload: 'Payload)
    =
    match eventHandlers.TryGetValue(eventId.key) with
    | true, handler ->
        let action = handler (payload :> obj)
        appDb.Dispatch(action)
    | false, _ -> console.error ($"No handler registered for event")

// Direct union dispatch with manual case identifier
let dispatchUnion<'Union, 'State> (appDb: IAppDb<'State>) (unionCase: 'Union) (caseName: string) =
    let unionType = unionCase.GetType()
    let eventKey = TypeKey(unionType, caseName)

    match eventHandlers.TryGetValue(eventKey) with
    | true, handler ->
        let action = handler (unionCase :> obj)
        appDb.Dispatch(action)
    | false, _ -> console.error ($"No handler registered for union case: {caseName}")

// Register a handler for a union case
let registerUnionHandler<'Union, 'State> (unionCaseHandler: 'Union -> 'State -> 'State) =
    let unionType = typeof<'Union>
    let eventId = { key = TypeKey(unionType, "") }

    let wrappedHandler (payload: obj) : obj =
        let typedPayload = payload :?> 'Union
        let reducer = (fun (state: 'State) -> unionCaseHandler typedPayload state) :> obj
        reducer

    eventHandlers.[eventId.key] <- wrappedHandler

// Register a handler for a specific union case
let registerUnionCaseHandler<'Union, 'State>
    (caseName: string)
    (handler: 'Union -> 'State -> 'State)
    =
    let unionType = typeof<'Union>
    let eventId = { key = TypeKey(unionType, caseName) }

    let wrappedHandler (payload: obj) : obj =
        let typedPayload = payload :?> 'Union
        let reducer = (fun (state: 'State) -> handler typedPayload state) :> obj
        reducer

    eventHandlers.[eventId.key] <- wrappedHandler

// ===== Backward Compatibility =====

// For compatibility with previous code
type EventDef<'Payload, 'State> = EventHandler<'Payload, 'State>

// Create event with handler in one step (for backward compatibility)
let createEvent<'Payload, 'State>
    (id: string)
    (handler: 'Payload -> 'State -> 'State)
    : EventHandler<'Payload, 'State>
    =
    let eventId = defineEvent<'Payload> id
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
