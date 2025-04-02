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

// Add a union type for loading state
type LoadingState<'T> =
    | NotStarted
    | Loading
    | Loaded of 'T
    | Failed of string

// Example of more complex combined effects
// First, define a new effect for "analyzing" todos
type TodoAnalysis = {
    totalCount: int
    completedCount: int
    activeCount: int
    averageTextLength: float
}

type AppState = {
    todos: TodoItem list
    filter: string  // "all" | "active" | "completed"
    nextId: int
    isLoading: bool // Track loading state in the app state
    todoAnalysis: LoadingState<TodoAnalysis> // Track analysis state
}



// Create initial state
let initialState = {
    todos = []
    filter = "all"
    nextId = 1
    isLoading = false
    todoAnalysis = NotStarted // Initialize analysis state
}

// Create app-db instance with proper generic type parameters
let appDb : IAppDb<AppState> = AppDb<AppState> initialState :> IAppDb<AppState>

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

let setTodoAnalysisEvent = EventId.named<LoadingState<TodoAnalysis>>("set-todo-analysis")

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

registerNamedEventHandler setTodoAnalysisEvent (fun (analysis) state ->
    console.log("Setting todo analysis:", analysis)
    { state with todoAnalysis = analysis }
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



let analyzeTodosEffect = EffectId.named<TodoItem list, TodoAnalysis>("analyze-todos")

// Register the effect handler
registerEffectHandler analyzeTodosEffect (fun todos -> async {
    // Simulate processing time
    do! Async.Sleep 1500
    
    let completed = todos |> List.filter (fun t -> t.completed)
    let active = todos |> List.filter (fun t -> not t.completed)
    let totalTextLength = todos |> List.sumBy (fun t -> t.text.Length)
    let avgLength = 
        if todos.Length > 0 then 
            float totalTextLength / float todos.Length 
        else 
            0.0
            
    return {
        totalCount = todos.Length
        completedCount = completed.Length
        activeCount = active.Length
        averageTextLength = avgLength
    }
})

// create a view for the analysis result
let todosAnalysisSubscription = createSubscription appDb (fun state ->
   state.todoAnalysis 
)

// Add a chained effects example
// Define a second effect that depends on the result of the first
let prioritizeTodosEffect = EffectId.named<TodoItem list, TodoItem list>("prioritize-todos")

// Register the effect handler
registerEffectHandler prioritizeTodosEffect (fun todos -> async {
    // Simulate processing time
    do! Async.Sleep 1000
    
    // Sort todos by completion status (active first) and then alphabetically
    return todos 
        |> List.sortBy (fun t -> (t.completed, t.text.ToLower()))
})

// Initialize the application with some test data
let initializeApp() =
    batchDispatch appDb [
        fun () -> dispatch appDb addTodoEvent "Learn F#"
        fun () -> dispatch appDb addTodoEvent "Build a FreeFrame app"
        fun () -> dispatch appDb addTodoEvent "Share with the community"
    ]
