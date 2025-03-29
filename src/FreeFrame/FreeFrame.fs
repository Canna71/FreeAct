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

// ===== Event Handling =====

// Enhanced event definition with type safety
type EventDef<'E, 'State> = { id: string; handler: 'E -> 'State -> 'State }

// Create a typed event definition with a handler function
let createEvent<'E, 'State> (id: string) (handler: 'E -> 'State -> 'State) : EventDef<'E, 'State> =
    { id = id; handler = handler }

// For compatibility with existing code - creates an event def without a handler
// let createEventDef<'E> (id: string) : EventDef<'E, obj> =
//     {
//         id = id
//         handler = fun _ s -> s  // Default no-op handler
//     }

// Private storage for handlers
let private eventHandlers = Collections.Generic.Dictionary<string, obj -> obj>()

// Register an event handler - now takes the event definition that already has the handler
let registerEventHandler<'E, 'T> (eventDef: EventDef<'E, 'T>) =
    let wrappedHandler (payload: obj) : obj =
        let typedPayload = payload :?> 'E
        let reducer = (fun (state: 'T) -> eventDef.handler typedPayload state) :> obj
        reducer

    eventHandlers.[eventDef.id] <- wrappedHandler

// For backward compatibility - register handler separately
let registerEventHandlerLegacy<'E, 'T> (eventDef: EventDef<'E, obj>) (handler: 'E -> 'T -> 'T) =
    let wrappedHandler (payload: obj) : obj =
        let typedPayload = payload :?> 'E
        let reducer = (fun (state: 'T) -> handler typedPayload state) :> obj
        reducer

    eventHandlers.[eventDef.id] <- wrappedHandler

// Dispatch an event with payload - works with both event styles
let dispatch<'E, 'T> (appDb: IAppDb<'T>) (eventDef: EventDef<'E, 'T>) (payload: 'E) =
    match eventHandlers.TryGetValue(eventDef.id) with
    | true, handler ->
        let action = handler (payload :> obj)
        appDb.Dispatch(action)
    | false, _ ->
        // For strongly typed events that haven't been explicitly registered,
        // we can register them on-the-fly if they have a handler

        let wrappedHandler (p: obj) : obj =
            let typedPayload = p :?> 'E
            let reducer = (fun (state: 'T) -> eventDef.handler typedPayload state) :> obj
            reducer

        eventHandlers.[eventDef.id] <- wrappedHandler
        let action = wrappedHandler (payload :> obj)
        appDb.Dispatch(action)
// | _ ->
//     console.error($"No handler registered for event: {eventDef.id}")

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
