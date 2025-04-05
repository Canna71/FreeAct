module Demo.TodoDemo

open Fable.React
open Browser
open Fable.Core.JS
open FreeAct.FreeFrame
open Fable.React.ReactBindings
open FreeAct
open Demo.FreeFrameDemo

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
                console.log("Loading todos...")
                // Run the effect and handle the result with the specialized result type
                dispatchAfterEffect appDb fetchTodosEffect () (fun result ->
                    // Transform the raw effect result into our domain-specific result type
                    console.log("Effect result: ", result)
                    let resultValue = 
                        match result with
                        | Ok todos -> TodosLoaded todos
                        | Error err -> FetchFailed err.Message
                    
                    // Return the single event type with the domain result
                    Some fetchTodosResultEvent, Some resultValue
                ) |> ignore // Ignore the result of dispatchAfterEffect to make onClick happy
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

// Main Todo demo component
let _TodoDemo () =
    // Initialize data when component mounts
    Hooks.useEffect((fun () ->
        initializeApp()
    ), [| |])
    
    div {
        className "todo-demo"
        h1 { "Todo App Demo" }
        p { "This demo shows the basic usage of FreeFrame with a Todo application." }
        TodoList()
        ExampleComponent()
    }
let TodoDemo = FunctionComponent.Of(_TodoDemo)