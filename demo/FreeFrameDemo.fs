module Demo.FreeFrameDemo

open Fable.React
open Browser
open Fable.Core.JS
open FreeAct.FreeFrame
open Fable.React.ReactBindings
open FreeAct

// Define application state type
type TodoItem = {
    id: int
    text: string
    completed: bool
}

type AppState = {
    todos: TodoItem list
    filter: string  // "all" | "active" | "completed"
    nextId: int
    isLoading: bool // Track loading state in the app state
}

// Add a union type for loading state
type LoadingState<'T> =
    | NotStarted
    | Loading
    | Loaded of 'T
    | Failed of string

// Create initial state
let initialState = {
    todos = []
    filter = "all"
    nextId = 1
    isLoading = false
}

// Create app-db instance with proper generic type parameters
let appDb : IAppDb<AppState> = AppDb<AppState>(initialState) :> IAppDb<AppState>

// Define a discriminated union for all Todo events
type TodoEvent = 
    | AddTodo of string
    | ToggleTodo of int
    | DeleteTodo of int
    | SetFilter of string
    | SetLoading of bool
    | SetTodos of TodoItem list  // New event to set todos from API

// Define another union type to demonstrate that there's no conflict
type AdminEvent = 
    | AddUser of string    // Note that this has same shape as AddTodo
    | DeleteUser of int    // Note that this has same shape as DeleteTodo
    | TogglePermission of int  // Note that this has same shape as ToggleTodo

// === Method 1: Using traditional event identifiers (auto-generated) ===
let addTodoEvent = EventId.auto<string>()
let toggleTodoEvent = EventId.auto<int>()
let deleteTodoEvent = EventId.auto<int>()
let setFilterEvent = EventId.auto<string>()
let setLoadingEvent = EventId.auto<bool>()
let setTodosEvent = EventId.auto<TodoItem list>()

// === Method 1b: Using string-based event identifiers ===
let addTodoNamedEvent = EventId.named<string>("add-todo")
let toggleTodoNamedEvent = EventId.named<int>("toggle-todo")
let deleteTodoNamedEvent = EventId.named<int>("delete-todo")
let setFilterNamedEvent = EventId.named<string>("set-filter")
let setLoadingNamedEvent = EventId.named<bool>("set-loading")
let setTodosNamedEvent = EventId.named<TodoItem list>("set-todos")

// === Method 2: Using type-based events ===
let todoEventId = EventId.ofType<TodoEvent>()

// Register handlers for the auto-generated events
registerNamedEventHandler addTodoEvent (fun text state ->
    let newTodo = { id = state.nextId; text = text; completed = false }
    { state with 
        todos = state.todos @ [newTodo]
        nextId = state.nextId + 1 }
)

registerNamedEventHandler toggleTodoEvent (fun id state ->
    { state with
        todos = state.todos |> List.map (fun todo ->
            if todo.id = id then { todo with completed = not todo.completed } else todo
        )
    }
)

registerNamedEventHandler deleteTodoEvent (fun id state ->
    { state with
        todos = state.todos |> List.filter (fun todo -> todo.id <> id)
    }
)

registerNamedEventHandler setFilterEvent (fun filter state ->
    { state with filter = filter }
)

registerNamedEventHandler setLoadingEvent (fun isLoading state ->
    { state with isLoading = isLoading }
)

registerNamedEventHandler setTodosEvent (fun todos state ->
    { state with 
        todos = todos
        nextId = 
            match todos with
            | [] -> 1
            | _ -> (todos |> List.map (fun t -> t.id) |> List.max) + 1
        isLoading = false
    }
)

// Register handlers for the string-based events
registerNamedEventHandler addTodoNamedEvent (fun text state ->
    let newTodo = { id = state.nextId; text = text; completed = false }
    { state with 
        todos = state.todos @ [newTodo]
        nextId = state.nextId + 1 }
)

registerNamedEventHandler toggleTodoNamedEvent (fun id state ->
    { state with
        todos = state.todos |> List.map (fun todo ->
            if todo.id = id then { todo with completed = not todo.completed } else todo
        )
    }
)

registerNamedEventHandler deleteTodoNamedEvent (fun id state ->
    { state with
        todos = state.todos |> List.filter (fun todo -> todo.id <> id)
    }
)

registerNamedEventHandler setFilterNamedEvent (fun filter state ->
    { state with filter = filter }
)

registerNamedEventHandler setLoadingNamedEvent (fun isLoading state ->
    { state with isLoading = isLoading }
)

registerNamedEventHandler setTodosNamedEvent (fun todos state ->
    { state with 
        todos = todos
        nextId = 
            match todos with
            | [] -> 1
            | _ -> (todos |> List.map (fun t -> t.id) |> List.max) + 1
        isLoading = false
    }
)

// === Method 2: Using union-based event handling (safer approach) with manual case names ===

registerTypedEventHandler  (fun event state ->
    match event with
    | AddTodo text ->
        let newTodo = { id = state.nextId; text = text; completed = false }
        { state with 
            todos = state.todos @ [newTodo]
            nextId = state.nextId + 1 }
    | ToggleTodo id ->
        { state with
            todos = state.todos |> List.map (fun todo ->
                if todo.id = id then { todo with completed = not todo.completed } else todo
            )
        }
    | DeleteTodo id ->
        { state with
            todos = state.todos |> List.filter (fun todo -> todo.id <> id)
        }
    | SetFilter filter ->
        { state with filter = filter }
    | SetLoading isLoading ->
        { state with isLoading = isLoading }
    | SetTodos todos ->
        { state with 
            todos = todos
            nextId = 
                match todos with
                | [] -> 1
                | _ -> (todos |> List.map (fun t -> t.id) |> List.max) + 1
            isLoading = false
        }
) 

registerTypedEventHandler (fun event state ->
    match event with
    | AddUser name ->
        printfn "Adding user: %s" name
        // Handle adding a user
        state
    | DeleteUser id ->
        printfn "Deleting user with ID: %d" id
        // Handle deleting a user
        state
    | TogglePermission id ->
        printfn "Toggling permission for user with ID: %d" id
        // Handle toggling permission for a user
        state
)


// testUnion<TodoEvent> ()



// Create subscriptions
let todosSubscription = createSubscription appDb (fun state -> state.todos)
let filteredTodosSubscription = createSubscription appDb (fun state ->
    match state.filter with
    | "active" -> state.todos |> List.filter (fun t -> not t.completed)
    | "completed" -> state.todos |> List.filter (fun t -> t.completed)
    | _ -> state.todos
)
let filterSubscription = createSubscription appDb (fun state -> state.filter)
let isLoadingSubscription = createSubscription appDb (fun state -> state.isLoading)

// === Define an effect for loading todos from a simulated API ===
// Define a specialized result type for the fetchTodos effect
type FetchTodosResult =
    | TodosLoaded of TodoItem list
    | FetchFailed of string
let fetchTodosEffect = EffectId.named<unit, TodoItem list>("fetch-todos")
// Add this with your other event declarations
let fetchTodosResultEvent = EventId.auto<FetchTodosResult>()

// Register a handler for this dedicated result event
registerNamedEventHandler fetchTodosResultEvent (fun result state ->
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
        console.error("Error loading todos:", err)
        { state with isLoading = false }
)

// Register the effect handler - simulate an API call with a delay
registerEffectHandler fetchTodosEffect (fun _ -> async {
    // Simulate network latency
    do! Async.Sleep 2000
    
    // Return mock data as if it came from an API
    return [
        { id = 101; text = "Learn F# and FreeFrame"; completed = false }
        { id = 102; text = "Build an awesome application"; completed = false }
        { id = 103; text = "Deploy to production"; completed = false }
        { id = 104; text = "Share with the community"; completed = true }
    ]
})

// React components
let TodoItemComponent (props: {| todo: TodoItem |}) =
    let todo = props.todo
    li {
        key (string todo.id)
        className (if todo.completed then "completed" else "")
        div {
            span { todo.text }
            button {
                onClick (fun _ -> dispatch appDb toggleTodoEvent todo.id)
                str (if todo.completed then "✓" else "☐")
            }
            button {
                onClick (fun _ -> dispatch appDb deleteTodoEvent todo.id)
                str "🗑"
            }
        }
    }

let TodoForm () =
    let textState = Hooks.useState("")
    let text' = textState.current
    let setText = fun (newText: string) -> textState.update newText

    let handleSubmit (e: Browser.Types.Event) =
        e.preventDefault()
        if text'.Trim() <> "" then
            dispatch appDb addTodoEvent text'
            // dispatchUnion appDb (AddTodo text') "AddTodo"
            setText("")
    
    form {
        onSubmit handleSubmit
        input {
            value text'
            onChange (fun e -> setText(e.Value))
            placeholder "What needs to be done?"
        }
        button {
            type' InputType.Submit
            str "Add"
        }
    }

let TodoFilters () =
    let currentFilter = useSubscription filterSubscription
    
    div {
        className "filters"
        button {
            className (if currentFilter = "all" then "active" else "")
            onClick (fun _ -> dispatch appDb setFilterEvent "all")
            str "All"
        }
        button {
            className (if currentFilter = "active" then "active" else "")
            onClick (fun _ -> dispatch appDb setFilterEvent "active")
            str "Active"
        }
        button {
            className (if currentFilter = "completed" then "active" else "")
            onClick (fun _ -> dispatch appDb setFilterEvent "completed")
            str "Completed"
        }
    }

let LoadTodosButton () =
    let isLoading = useSubscription isLoadingSubscription
    
    div {
        className "load-todos-container"
        button {
            disabled isLoading
            className "load-button"
            onClick (fun _ -> 
                // Set loading state
                dispatch appDb setLoadingEvent true
                
                // Run the effect and handle the result with the specialized result type
                dispatchAfterEffect appDb fetchTodosEffect () (fun result ->
                    // Transform the raw effect result into our domain-specific result type
                    let resultValue = 
                        match result with
                        | Ok todos -> TodosLoaded todos
                        | Error err -> FetchFailed err.Message
                    
                    // Return the single event type with the domain result
                    Some fetchTodosResultEvent, Some resultValue
                )
            )
            
            if isLoading then
                str "Loading..."
            else
                str "Load Todos from API"
        }
        
        if isLoading then
            div {
                className "loading-indicator"
                str "Please wait, loading data..."
            }
    }

let TodoList () =
    let todos = useSubscription filteredTodosSubscription
    let isLoading = useSubscription isLoadingSubscription
    
    div {
        h2 { "Todo List" }
        TodoForm()
        TodoFilters()
        LoadTodosButton() // Add the load button
        
        if isLoading then
            div {
                className "loading-overlay"
                str "Loading..."
            }
        
        ul {
            className "todo-list"
            todos |> List.map (fun todo ->
                TodoItemComponent {| todo = todo |}
            )
        }
        p {
            str (sprintf "Total: %d items" todos.Length)
        }
    }

// Example component showing both dispatch methods with improved union handling
let ExampleComponent () =
    div {
        className "dispatch-examples"
        h3 { "Two Ways to Dispatch Events" }
        div {
            // Method 1: Traditional event dispatch
            button {
                className "traditional"
                onClick (fun _ -> dispatch appDb addTodoEvent "Task via traditional dispatch")
                str "Add via Traditional Event"
            }
            
            // Method 2: Direct union case dispatch (now with explicit case name)
            button {
                className "direct-union"
                onClick (fun _ -> dispatchTyped appDb (AddTodo "Task via union dispatch") )
                str "Add via Union"
            }
            
            // Show that there's no conflict with AdminEvent
            button {
                className "admin-event"
                onClick (fun _ -> dispatchTyped appDb (AddUser "Admin action") )
                str "Add User (Admin)"
            }
        }
    }

// Example component with Effects
let EffectExampleComponent () =
    // Use the F# idiomatic hook for effects
    let isLoading, result = useEffect fetchTodosEffect ()
    
    div {
        className "effect-example"
        h3 { "Effect Example (Auto-Loading)" }
        
        match isLoading, result with
        | true, _ -> 
            div { 
                className "loading"
                str "Loading data..." 
            }
        | false, Some (Ok todos) ->
            div {
                className "success"
                str (sprintf "Successfully loaded %d items" todos.Length)
                ul {
                    todos |> List.map (fun todo ->
                        li {
                            key (string todo.id)
                            str todo.text
                        }
                    )
                }
            }
        | false, Some (Error err) ->
            div {
                className "error"
                str (sprintf "Error: %s" err.Message)
            }
        | false, None ->
            div {
                className "none"
                str "No data loaded yet."
            }
    }

// Add an example using the new union-based loading state
let EffectWithUnionStateExample () =
    // Define how to map the result to our union state
    let mapResultToState (result: Result<TodoItem list, exn>) =
        match result with
        | Ok todos -> Loaded todos
        | Error err -> Failed err.Message
    
    // Use the new hook with our union state
    let loadingState, resultOpt = 
        useEffectWithUnionState 
            fetchTodosEffect
            ()
            NotStarted  // Initial state
            mapResultToState  // Result mapping function
    
    div {
        className "effect-example-union"
        h3 { "Effect Example (Union Loading State)" }
        
        match loadingState with
        | NotStarted -> 
            div { 
                className "not-started"
                str "Effect not started yet" 
            }
        | Loading -> 
            div { 
                className "loading"
                str "Loading data..." 
            }
        | Loaded todos ->
            div {
                className "success"
                str (sprintf "Successfully loaded %d items" todos.Length)
                ul {
                    todos |> List.map (fun todo ->
                        li {
                            key (string todo.id)
                            str todo.text
                        }
                    )
                }
            }
        | Failed errorMsg ->
            div {
                className "error"
                str (sprintf "Error: %s" errorMsg)
            }
    }

// Main component
let FreeFrameApp () =
    // Use the useEffect hook to initialize data after component mount,
    // but use batchDispatch to ensure state updates are properly processed
    Hooks.useEffect((fun () ->
        printfn "FreeFrameApp mounted"
        
        // Use batch dispatch to ensure all events are processed and subscribers are notified
        batchDispatch appDb [
            fun () -> dispatch appDb addTodoEvent "Learn F#"
            fun () -> dispatch appDb addTodoEvent "Build a FreeFrame app"
            fun () -> dispatch appDb addTodoEvent "Share with the community"
        ]
    ), [| |])
    
    div {
        className "freeframe-app"
        h1 { "FreeFrame Demo - Todo App" }
        TodoList()
        ExampleComponent()
        EffectExampleComponent() 
        EffectWithUnionStateExample() // Add the new example with union state
    }

// Initialize the application, can be called directly from the router
let renderFreeFrameDemo() =
    let container = Browser.Dom.document.getElementById("freeframe-root")
    if not (isNull container) then
        let root = ReactDomClient.createRoot(container)
        root.render(FreeFrameApp())
    else
        console.error("No element with id 'freeframe-root' found.")

// Export the FreeFrameApp component for direct use in the router
let FreeFrameDemo = FreeFrameApp
