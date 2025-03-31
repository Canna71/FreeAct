module FreeAct.FreeFrameSubscription

open System
open Fable.Core
open Fable.React
open FreeAct.FreeFrame

/// Properly managed subscription hook that handles disposal and updates
let useSubscription<'State, 'Value> (appDb: IAppDb<'State>) (selector: 'State -> 'Value) =
    // Get the initial value for state
    let initialValue = selector (appDb.GetState())
    let state = Hooks.useState initialValue

    // Create a ref to hold the subscription (for proper cleanup)
    let subscriptionRef = Hooks.useRef None

    // Set up subscription with proper cleanup
    Hooks.useEffectDisposable (
        (fun () ->
            // Create the subscription callback
            let onStateChanged (newValue: 'Value) = state.update newValue

            // Create the actual subscription and store in ref
            let subscription =
                appDb.Subscribe(fun newState ->
                    let newValue = selector newState
                    onStateChanged newValue
                )

            // Store in ref for later cleanup
            subscriptionRef.current <- Some subscription

            // Create disposable for cleanup
            { new IDisposable with
                member _.Dispose() =
                    // Cleanup the subscription when component unmounts
                    match subscriptionRef.current with
                    | Some subscription -> subscription.Dispose()
                    | None -> ()

                    // Clear the ref
                    subscriptionRef.current <- None
            }
        ),
        [| box appDb; box selector |] // Re-subscribe if appDb or selector changes
    )

    // If the subscription value and the state get out of sync, update it
    Hooks.useEffect (
        (fun () ->
            let currentValue = selector (appDb.GetState())

            if not (Object.Equals(currentValue, state.current)) then
                state.update currentValue
        ),
        [||] // Run this check on every render
    )

    state.current

/// Use a subscription based on a subscription object that is already created
let useExistingSubscription<'Value> (subscription: ISubscription<'Value>) =
    // Get the initial value for state
    let initialValue = subscription.Value
    let state = Hooks.useState initialValue

    // Create a ref to hold the subscription (for proper cleanup)
    let subscriptionRef = Hooks.useRef None

    // Set up subscription with proper cleanup
    Hooks.useEffectDisposable (
        (fun () ->
            // Create the subscription callback
            let onValueChanged (newValue: 'Value) = state.update newValue

            // Create the actual subscription and store in ref
            let disposable = subscription.Subscribe(onValueChanged)

            // Store in ref for later cleanup
            subscriptionRef.current <- Some disposable

            // Create disposable for cleanup
            { new IDisposable with
                member _.Dispose() =
                    // Cleanup the subscription when component unmounts
                    match subscriptionRef.current with
                    | Some disposable -> disposable.Dispose()
                    | None -> ()

                    // Clear the ref
                    subscriptionRef.current <- None
            }
        ),
        [| box subscription |] // Re-subscribe if subscription object changes
    )

    // If the subscription value and the state get out of sync, update it
    Hooks.useEffect (
        (fun () ->
            let currentValue = subscription.Value

            if not (Object.Equals(currentValue, state.current)) then
                state.update currentValue
        ),
        [||] // Run this check on every render
    )

    state.current

/// Combine two subscriptions into a single value
let useCombinedSubscription<'State, 'A, 'B, 'Result>
    (appDb: IAppDb<'State>)
    (selectorA: 'State -> 'A)
    (selectorB: 'State -> 'B)
    (combiner: 'A -> 'B -> 'Result)
    =

    let valueA = useSubscription appDb selectorA
    let valueB = useSubscription appDb selectorB

    // Combine the values whenever either changes
    let combinedValue = combiner valueA valueB
    combinedValue
