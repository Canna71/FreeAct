module Demo.Todo.State

open FreeAct.FreeFrame
open Fable.Core.JS
open Demo.Todo.Types
open Demo.AppState


// === Method 1b: Using string-based event identifiers ===
let addTodoEvent = EventId.named<string>("add-todo")
let toggleTodoEvent = EventId.named<int>("toggle-todo")
let deleteTodoEvent = EventId.named<int>("delete-todo")
let setFilterEvent = EventId.named<string>("set-filter")
let setLoadingEvent = EventId.named<bool>("set-loading")
let setTodosEvent = EventId.named<TodoItem list>("set-todos")
let setTodoAnalysisEvent = EventId.named<LoadingState<TodoAnalysis>>("set-todo-analysis")



// Register handlers for the string-based events
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

registerNamedEventHandler setTodoAnalysisEvent (fun (analysis) state ->
    console.log("Setting todo analysis:", analysis)
    { state with todoAnalysis = analysis }
)

let todoStateSubscription = createSubscription appDb (fun state -> state.TodoState)

// Create subscriptions
let todosSubscription = mapSubscription todoStateSubscription (fun state -> state.todos)
let filteredTodosSubscription = mapSubscription todoStateSubscription (fun state ->
    match state.filter with
    | "active" -> state.todos |> List.filter (fun t -> not t.completed)
    | "completed" -> state.todos |> List.filter (fun t -> t.completed)
    | _ -> state.todos
)
let filterSubscription = mapSubscription todoStateSubscription (fun state -> state.filter)
let isLoadingSubscription = mapSubscription todoStateSubscription (fun state -> state.isLoading)

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
let todosAnalysisSubscription = mapSubscription todoStateSubscription (fun state ->
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
    // batchDispatch appDb [
    //     fun () -> dispatch appDb addTodoEvent "Learn F#"
    //     fun () -> dispatch appDb addTodoEvent "Build a FreeFrame app"
    //     fun () -> dispatch appDb addTodoEvent "Share with the community"
    // ]
    batchDispatch appDb [
        box addTodoEvent, "Learn F#"
        box addTodoEvent, "Build a FreeFrame app"
        box addTodoEvent, "Share with the community"
    ]
