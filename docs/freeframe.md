# FreeFrame

FreeFrame is a Re-Frame inspired state management system for F# React applications.

## Core Concepts

- Events: Messages that trigger state changes
- Reducers: Pure functions that update state
- Subscriptions: Reactive views of state
- Effects: Side effects management

## Basic Usage

```fsharp
// Define your state
type AppState = {
    count: int
    loading: bool
}

// Define events
type CounterEvent =
    | Increment
    | Decrement
    | Reset

// Register event handler
registerEventHandler CounterEvent (fun event state ->
    match event with
    | Increment -> { state with count = state.count + 1 }
    | Decrement -> { state with count = state.count - 1 }
    | Reset -> { state with count = 0 }
)

// Create subscriptions
let countView = createView appDb (fun state -> state.count)

// Use in components
let Counter() =
    let count = useView countView
    div {
        button {
            onClick (fun _ -> dispatch Increment)
            text "+"
        }
        span { text (string count) }
    }
```

## Effects

```fsharp
// Define effects
let loadData = effect {
    let! data = Api.fetchData()
    dispatch (DataLoaded data)
}

// Use effects
button {
    onClick (fun _ -> Effects.runEffect loadData)
    text "Load Data"
}
```
