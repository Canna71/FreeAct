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

// Define events (just identifiers)
let addTodoEvent = defineEvent<string>("addTodo")
let toggleTodoEvent = defineEvent<int>("toggleTodo")
let deleteTodoEvent = defineEvent<int>("deleteTodo")
let setFilterEvent = defineEvent<string>("setFilter")

// Define and register event handlers separately
registerEventHandler(registerHandler addTodoEvent (fun text state ->
    let newTodo = { id = state.nextId; text = text; completed = false }
    { state with 
        todos = state.todos @ [newTodo]
        nextId = state.nextId + 1 }
))

registerEventHandler(registerHandler toggleTodoEvent (fun id state ->
    { state with
        todos = state.todos |> List.map (fun todo ->
            if todo.id = id then { todo with completed = not todo.completed } else todo
        )
    }
))

registerEventHandler(registerHandler deleteTodoEvent (fun id state ->
    { state with
        todos = state.todos |> List.filter (fun todo -> todo.id <> id)
    }
))

registerEventHandler(registerHandler setFilterEvent (fun filter state ->
    { state with filter = filter }
))

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
                onClick (fun _ -> dispatch appDb toggleTodoEvent todo.id)  // Now just needs the event ID and payload
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
            dispatch appDb addTodoEvent text'  // Cleaner dispatch with just event ID and payload
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
            onClick (fun _ -> dispatch appDb setFilterEvent "all")  // Cleaner dispatch
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
                // TODO: how to pass key to child component?
                TodoItemComponent {| todo = todo |} (*[key (string todo.id)]*)
            )
        }
        p {
            str (sprintf "Total: %d items" todos.Length)
        }
    }

// Main component
let FreeFrameApp () =
    // Hook to render the todos when the component mounts
    Hooks.useEffect((fun () ->
        // Add some initial todos for demonstration
        dispatch appDb addTodoEvent "Learn F#"
        dispatch appDb addTodoEvent "Build a FreeFrame app"
        dispatch appDb addTodoEvent "Share with the community"
        
    ), [| |])
    
    div {
        className "freeframe-app"
        h1 { "FreeFrame Demo - Todo App" }
        TodoList()
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
