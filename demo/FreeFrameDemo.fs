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
}

// Create initial state
let initialState = {
    todos = []
    filter = "all"
    nextId = 1
}

// Create app-db instance with proper generic type parameters
let appDb : IAppDb<AppState> = AppDb<AppState>(initialState) :> IAppDb<AppState>

// Define a discriminated union for all Todo events
type TodoEvent = 
    | AddTodo of string
    | ToggleTodo of int
    | DeleteTodo of int
    | SetFilter of string

// Define another union type to demonstrate that there's no conflict
type AdminEvent = 
    | AddUser of string    // Note that this has same shape as AddTodo
    | DeleteUser of int    // Note that this has same shape as DeleteTodo
    | TogglePermission of int  // Note that this has same shape as ToggleTodo

// === Method 1: Using traditional event identifiers (auto-generated) ===
let addTodoEvent = defineEvent<string>() 
let toggleTodoEvent = defineEvent<int>()
let deleteTodoEvent = defineEvent<int>()
let setFilterEvent = defineEvent<string>()

// === Method 1b: Using string-based event identifiers ===
let addTodoNamedEvent = defineNamedEvent<string>("add-todo")
let toggleTodoNamedEvent = defineNamedEvent<int>("toggle-todo")
let deleteTodoNamedEvent = defineNamedEvent<int>("delete-todo")
let setFilterNamedEvent = defineNamedEvent<string>("set-filter")


// Register handlers for the auto-generated events
registerEventHandler addTodoEvent (fun text state ->
    let newTodo = { id = state.nextId; text = text; completed = false }
    { state with 
        todos = state.todos @ [newTodo]
        nextId = state.nextId + 1 }
)

registerEventHandler toggleTodoEvent (fun id state ->
    { state with
        todos = state.todos |> List.map (fun todo ->
            if todo.id = id then { todo with completed = not todo.completed } else todo
        )
    }
)

registerEventHandler deleteTodoEvent (fun id state ->
    { state with
        todos = state.todos |> List.filter (fun todo -> todo.id <> id)
    }
)

registerEventHandler setFilterEvent (fun filter state ->
    { state with filter = filter }
)

// Register handlers for the string-based events
registerEventHandler addTodoNamedEvent (fun text state ->
    let newTodo = { id = state.nextId; text = text; completed = false }
    { state with 
        todos = state.todos @ [newTodo]
        nextId = state.nextId + 1 }
)

registerEventHandler toggleTodoNamedEvent (fun id state ->
    { state with
        todos = state.todos |> List.map (fun todo ->
            if todo.id = id then { todo with completed = not todo.completed } else todo
        )
    }
)

registerEventHandler deleteTodoNamedEvent (fun id state ->
    { state with
        todos = state.todos |> List.filter (fun todo -> todo.id <> id)
    }
)

registerEventHandler setFilterNamedEvent (fun filter state ->
    { state with filter = filter }
)

// === Method 2: Using union-based event handling (safer approach) with manual case names ===

registerUnionEventHandler  (fun event state ->
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
)

registerEventHandler (defineUnionEvent<AdminEvent>()) (fun event state ->
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

let TodoList () =
    let todos = useSubscription filteredTodosSubscription
    
    div {
        h2 { "Todo List" }
        TodoForm()
        TodoFilters()
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
                onClick (fun _ -> dispatchUnion appDb (AddTodo "Task via union dispatch") )
                str "Add via Union"
            }
            
            // Show that there's no conflict with AdminEvent
            button {
                className "admin-event"
                onClick (fun _ -> dispatchUnion appDb (AddUser "Admin action") )
                str "Add User (Admin)"
            }
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
