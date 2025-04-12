module Demo.Todo.UI

open Fable.React
open Browser
open Fable.Core.JS
open FreeFrame
open FreeAct // needed for React DSL
open Demo.Todo.Types
open Demo.Todo.State
open Demo.AppState

// open Demo.FreeFrameDemo

// React components
let TodoItemComponent (props: {| todo: TodoItem |}) =
    let todo = props.todo

    li {
        key (string todo.id)

        className (
            if todo.completed then
                "completed"
            else
                ""
        )

        div {
            span { todo.text }

            button {
                onClick (fun _ -> dispatch appDb toggleTodoEvent todo.id)

                str (
                    if todo.completed then
                        "✓"
                    else
                        "☐"
                )
            }

            button {
                onClick (fun _ -> dispatch appDb deleteTodoEvent todo.id)
                str "🗑"
            }
        }
    }

let TodoForm () =
    let textState = Hooks.useState ("")
    let text' = textState.current
    let setText = fun (newText: string) -> textState.update newText

    let handleSubmit (e: Browser.Types.Event) =
        e.preventDefault ()

        if text'.Trim() <> "" then
            dispatch appDb addTodoEvent text'
            // dispatchUnion appDb (AddTodo text') "AddTodo"
            setText ("")

    form {
        onSubmit handleSubmit

        input {
            value text'
            onChange (fun e -> setText (e.Value))
            placeholder "What needs to be done?"
        }

        button {
            type' InputType.Submit
            str "Add"
        }
    }

let TodoFilters = 
  createViewComponent filterSubscription 
    (fun (currentFilter: string) ->
      // let currentFilter = useView filterSubscription
      
      let onFilter (filter: string) =
          fun _ ->
            dispatch appDb setFilterEvent filter
      
      div {
          className "filters"

          button {
              className (if currentFilter = "all" then Demo.Style.classes.active.AsClassName  else "")
              onClick (onFilter "all")
              str "All"
          }

          button {
              className (if currentFilter = "active" then Demo.Style.classes.active.AsClassName else "")
              onClick (onFilter "active")
              str "Active"
          }

          button {
              className (if currentFilter = "completed" then Demo.Style.classes.active.AsClassName else "")
              onClick (onFilter "completed")
              str "Completed"
          }
      }
    )

let LoadTodosButton ()  =
    let isLoading  = useView isLoadingSubscription

    div {
        className "load-todos-container"

        button {
            disabled isLoading
            className "load-button"
            onClick (fun _ -> Effects.runEffect loadTodosEffect () |> ignore)

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

let TodoList (todos: TodoItem list, isLoading) =
    console.log ("Rendering TodoList with todos: ", todos)
    
    fragment {
        if isLoading then
            div {
                className "loading-overlay"
                str "Loading..."
            }

        ul {
            className "todo-list"
            todos |> List.map (fun todo -> TodoItemComponent {| todo = todo |})
        }

        p { str (sprintf "Total: %d items" todos.Length) }
    }

let TodoListForm () =
    let todos = useView filteredTodosSubscription
    let isLoading = useView isLoadingSubscription

    div {
        h2 { "Todo List" }
        TodoForm()
        TodoFilters()
        LoadTodosButton() // Add the load button

        TodoList(todos, isLoading)
    }

// Main Todo demo component
let _TodoDemo () =
    // Initialize data when component mounts
    Hooks.useEffect ((fun () -> initializeApp ()), [||])

    div {
        className "todo-demo"
        h1 { "Todo App Demo" }
        p { "This demo shows the basic usage of FreeFrame with a Todo application." }
        TodoListForm()
        // ExampleComponent()
    }

let TodoDemo = FunctionComponent.Of(_TodoDemo)
