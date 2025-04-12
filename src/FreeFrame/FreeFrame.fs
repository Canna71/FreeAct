module FreeFrame

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
let registerEventHandler<'Payload, 'State>
    (eventId: EventId<'Payload>)
    (handler: EventHandler<'Payload, 'State>)
    =
    // Wrap the handler to match the expected signature
    // This allows us to use the same handler for different payload types
    // and to avoid boxing issues
    let wrappedHandler (payload: obj) : obj =
        let typedPayload = payload :?> 'Payload
        let reducer = (fun (state: 'State) -> handler typedPayload state) :> obj
        reducer

    eventHandlers.[EventId.key eventId] <- wrappedHandler

let inline registerTypedEventHandler<'EventType, 'State>
    (handler: EventHandler<'EventType, 'State>)
    =
    let eventId = EventId.ofType<'EventType> ()
    registerEventHandler eventId handler

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

let registerFocusedEventHandler<'Payload, 'State, 'SubState>
    (lens: 'State -> 'SubState)
    (setLens: 'SubState -> 'State -> 'State)
    (eventId: EventId<'Payload>)
    (handler: EventHandler<'Payload, 'SubState>)
    =
    let focusedHandler = focusHandler lens setLens handler

    registerEventHandler eventId focusedHandler

let inline registerFocusedTypedEventHandler<'EventType, 'State, 'SubState>
    (lens: 'State -> 'SubState)
    (setLens: 'SubState -> 'State -> 'State)
    (handler: EventHandler<'EventType, 'SubState>)
    =
    let eventId = EventId.ofType<'EventType> ()
    registerFocusedEventHandler lens setLens eventId handler

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
//             Views on app-db
// =======================================================

type IView<'V> =
    abstract member Value: 'V
    abstract member Subscribe: ('V -> unit) -> IDisposable

// Create a derived view of app-db
let createView<'T, 'V> (appDb: IAppDb<'T>) (selector: 'T -> 'V) =
    let mutable currentValue = selector (appDb.GetState())
    let subscribers = ResizeArray<'V -> unit>()

    let subscription =
        { new IView<'V> with
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
//             Views Composition
// =====================================================

// Combine two subscriptions into a new one
let combineViews<'A, 'B, 'C>
    (subA: IView<'A>)
    (subB: IView<'B>)
    (combiner: 'A -> 'B -> 'C)
    : IView<'C>
    =

    let mutable currentValueA = subA.Value
    let mutable currentValueB = subB.Value
    let mutable currentValueC = combiner currentValueA currentValueB
    let subscribers = ResizeArray<'C -> unit>()

    let subscription =
        { new IView<'C> with
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
    { new IView<'C> with
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
let combine3Views<'A, 'B, 'C, 'D>
    (subA: IView<'A>)
    (subB: IView<'B>)
    (subC: IView<'C>)
    (combiner: 'A -> 'B -> 'C -> 'D)
    : IView<'D>
    =

    // First combine A and B
    let subAB = combineViews subA subB (fun a b -> (a, b))

    // Then combine the result with C
    combineViews subAB subC (fun (a, b) c -> combiner a b c)

// Map a subscription to a new type
let mapView<'A, 'B> (subscription: IView<'A>) (mapper: 'A -> 'B) : IView<'B> =
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

    { new IView<'B> with
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
let filterView<'A> (subscription: IView<'A>) (predicate: 'A -> bool) : IView<'A option> =

    mapView
        subscription
        (fun value ->
            if predicate value then
                Some value
            else
                None
        )

// ===== React Integration =====

// React hook to use a subscription in a React component with immediate subscription
let useView<'V> (view: IView<'V>) =
    let initialValue = view.Value

    // Create the state hook with the initial value
    let state = Hooks.useState (initialValue)

    // Subscribe immediately using useRef to manage the subscription reference
    let subscriptionRef = Hooks.useRef (None)

    // If we don't have a subscription yet, create one immediately
    if subscriptionRef.current.IsNone then
        // Function to update the React state when subscription value changes
        let setState = fun (newValue: 'V) -> state.update (newValue)

        // Create the subscription right away
        let dispose = view.Subscribe(setState)

        // Store the dispose function in the ref
        subscriptionRef.current <- Some dispose

    // Use Effect for cleanup only
    Hooks.useEffectDisposable (
        (fun () ->
            // Return the dispose function for cleanup
            { new IDisposable with
                member _.Dispose() =

                    match subscriptionRef.current with
                    | Some dispose ->
                        dispose.Dispose()
                        subscriptionRef.current <- None
                    | None -> ()
            }
        ),
        [| view :> obj |]
    )

    // Return the current value from state
    state.current

// creates a new subscription based on the selector
let useNewView<'T, 'V> (appDb: IAppDb<'T>) (selector: 'T -> 'V) : 'V =
    // Create a subscription using the selector, storing it in a ref and disposing it
    // when the component unmounts
    let subscriptionRef = Hooks.useRef (None)

    if subscriptionRef.current.IsNone then
        // Function to update the React state when subscription value changes
        // Create the subscription right away
        let subscription = createView appDb selector
        // Store the subscription in the ref
        subscriptionRef.current <- Some subscription

    useView subscriptionRef.current.Value

/// Hook that combines two view subscriptions with a combiner function
let useCombinedViews<'A, 'B, 'C> (subA: IView<'A>) (subB: IView<'B>) (combiner: 'A -> 'B -> 'C) =

    let combinedSub = combineViews subA subB combiner
    useView combinedSub

/// Hook that combines view subscription with rendering
let useViewRender<'V> (view: IView<'V>) (render: 'V -> ReactElement) =
    let value = useView view
    render value

/// Hook that combines two view subscriptions with rendering
let useViewRender2<'A, 'B>
    (viewA: IView<'A>)
    (viewB: IView<'B>)
    (render: 'A -> 'B -> ReactElement)
    =
    let valueA = useView viewA
    let valueB = useView viewB
    render valueA valueB

/// Hook that combines three view subscriptions with rendering
let useViewRender3<'A, 'B, 'C>
    (viewA: IView<'A>)
    (viewB: IView<'B>)
    (viewC: IView<'C>)
    (render: 'A -> 'B -> 'C -> ReactElement)
    =
    let valueA = useView viewA
    let valueB = useView viewB
    let valueC = useView viewC
    render valueA valueB valueC

/// create a React component that uses a view subscription
let createViewComponent<'V> (view: IView<'V>) (render: 'V -> ReactElement) =
    // Create a unique name for each component instance
    let f =
        fun () ->
            let value = useView view
            render value

    let elemType = ReactElementType.ofFunction f
    fun props -> ReactElementType.create elemType props []

/// create a React component that uses two view subscriptions
let createViewComponent2<'A, 'B>
    (viewA: IView<'A>)
    (viewB: IView<'B>)
    (render: 'A -> 'B -> ReactElement)
    =
    // Create a unique name for each component instance
    let f =
        fun () ->
            let valueA = useView viewA
            let valueB = useView viewB
            render valueA valueB

    let elemType = ReactElementType.ofFunction f
    fun props -> ReactElementType.create elemType props []

/// create a React component that uses three view subscriptions
let createViewComponent3<'A, 'B, 'C>
    (viewA: IView<'A>)
    (viewB: IView<'B>)
    (viewC: IView<'C>)
    (render: 'A -> 'B -> 'C -> ReactElement)
    =
    // Create a unique name for each component instance
    let f =
        fun () ->
            let valueA = useView viewA
            let valueB = useView viewB
            let valueC = useView viewC
            render valueA valueB valueC

    let elemType = ReactElementType.ofFunction f
    fun props -> ReactElementType.create elemType props []

// =======================================================
