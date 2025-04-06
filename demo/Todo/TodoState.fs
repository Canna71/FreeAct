module Demo.Todo.State

open FreeFrame
open Fable.Core.JS
open Demo.Todo.Types
open Demo.AppState
open FreeFrame


// === Method 1b: Using string-based event identifiers ===
let addTodoEvent = EventId.named<string> ("add-todo")
let toggleTodoEvent = EventId.named<int> ("toggle-todo")
let deleteTodoEvent = EventId.named<int> ("delete-todo")
let setFilterEvent = EventId.named<string> ("set-filter")
let setLoadingEvent = EventId.named<bool> ("set-loading")
let setTodosEvent = EventId.named<TodoItem list> ("set-todos")

// Define the lens functions once
let todoStateLens (state: AppState) = state.TodoState
let setTodoState todoState state = { state with TodoState = todoState }

// Register the event handlers for the Todo events, using the lens functions
let registerTodosEventHandler eventId handler =
    registerFocusedEventHandler
        todoStateLens
        setTodoState
        eventId
        (fun arg todoState -> handler arg todoState)

// Register handlers for the string-based events
registerTodosEventHandler
    addTodoEvent
    (fun text todoState ->
        let newTodo =
            {
                id = todoState.nextId
                text = text
                completed = false
            }

        { todoState with
            todos =
                todoState.todos
                @ [
                    newTodo
                ]
            nextId = todoState.nextId + 1
        }
    )

registerTodosEventHandler
    toggleTodoEvent
    (fun id todoState ->
        { todoState with
            todos =
                todoState.todos
                |> List.map (fun todo ->
                    if todo.id = id then
                        { todo with completed = not todo.completed }
                    else
                        todo
                )
        }
    )

registerTodosEventHandler
    deleteTodoEvent

    (fun id todoState ->
        { todoState with todos = todoState.todos |> List.filter (fun todo -> todo.id <> id) }
    )

registerTodosEventHandler
    setFilterEvent
    (fun filter todoState -> { todoState with filter = filter })

registerTodosEventHandler
    setLoadingEvent
    (fun isLoading todoState -> { todoState with isLoading = isLoading })

registerTodosEventHandler
    setTodosEvent
    (fun todos todoState ->
        { todoState with

            todos = todos
            nextId =
                match todos with
                | [] -> 1
                | _ -> (todos |> List.map (fun t -> t.id) |> List.max) + 1
            isLoading = false

        }
    )

let todoStateSubscription = createView appDb todoStateLens

// Create subscriptions
let todosSubscription =
    mapView todoStateSubscription (fun state -> state.todos)

let filteredTodosSubscription =
    mapView
        todoStateSubscription
        (fun state ->
            match state.filter with
            | "active" -> state.todos |> List.filter (fun t -> not t.completed)
            | "completed" -> state.todos |> List.filter (fun t -> t.completed)
            | _ -> state.todos
        )

let filterSubscription =
    mapView todoStateSubscription (fun state -> state.filter)

let isLoadingSubscription =
    mapView todoStateSubscription (fun state -> state.isLoading)

// === Define an effect for loading todos from a simulated API ===
// Define a specialized result type for the fetchTodos effect
type FetchTodosResult =
    | TodosLoaded of TodoItem list
    | FetchFailed of string

let fetchTodosEffect = EffectId.named<unit, TodoItem list> ("fetch-todos")
// Add this with your other event declarations
let fetchTodosResultEvent = EventId.auto<FetchTodosResult> ()

// Register a handler for this dedicated result event
registerEventHandler
    fetchTodosResultEvent
    (focusHandler
        todoStateLens
        setTodoState
        (fun result state ->
            match result with
            | TodosLoaded todos ->
                { state with
                    todos = todos
                    nextId =
                        match todos with
                        | [] -> 1
                        | _ -> (todos |> List.map (fun t -> t.id) |> List.max) + 1
                    isLoading = false
                }
            | FetchFailed err ->
                // Just turn off loading, keep existing todos
                console.error ("Error loading todos:", err)
                { state with isLoading = false }
        ))

// Register the effect handler - simulate an API call with a delay
Effects.registerHandler
    fetchTodosEffect
    (fun _ ->
        async {
            // Simulate network latency
            do! Async.Sleep 2000

            // Return mock data as if it came from an API
            return
                [
                    {
                        id = 101
                        text = "Learn F# and FreeFrame"
                        completed = false
                    }
                    {
                        id = 102
                        text = "Build an awesome application"
                        completed = false
                    }
                    {
                        id = 103
                        text = "Deploy to production"
                        completed = false
                    }
                    {
                        id = 104
                        text = "Share with the community"
                        completed = true
                    }
                ]
        }
    )

// Initialize the application with some test data
let initializeApp () =
    console.log ("Initializing app with test data...")

    batchDispatch appDb [
        box addTodoEvent, "Learn F#"
        box addTodoEvent, "Build a FreeFrame app"
        box addTodoEvent, "Share with the community"
    ]
