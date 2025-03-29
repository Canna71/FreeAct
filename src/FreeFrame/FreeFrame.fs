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

// Event registration and dispatch
type EventHandler<'T> = 'T -> obj
type EventDef<'E> = { id: string }

let createEventDef<'E> (id: string) : EventDef<'E> = { id = id }

// Global registry of event handlers
let private eventHandlers = Collections.Generic.Dictionary<string, obj -> obj>()

// Register an event handler
let registerEventHandler<'E, 'T> (eventDef: EventDef<'E>) (handler: 'E -> 'T -> 'T) =
    let wrappedHandler (payload: obj) : obj =
        let typedPayload = payload :?> 'E
        let reducer = (fun (state: 'T) -> handler typedPayload state) :> obj
        reducer

    eventHandlers.[eventDef.id] <- wrappedHandler

// Dispatch an event with payload (fix with correct type parameters)
let dispatch<'E, 'T> (appDb: IAppDb<'T>) (eventDef: EventDef<'E>) (payload: 'E) =
    match eventHandlers.TryGetValue(eventDef.id) with
    | true, handler ->
        let action = handler (payload :> obj)
        appDb.Dispatch(action)
    | false, _ -> console.error ($"No handler registered for event: {eventDef.id}")

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
