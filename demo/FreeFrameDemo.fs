module Demo.FreeFrameDemo

open Fable.React
open Browser
open Fable.Core.JS
open FreeFrame
open Fable.React.ReactBindings
open FreeAct





// Example of more complex combined effects
// First, define a new effect for "analyzing" todos




// Create initial state


// Create app-db instance with proper generic type parameters
// let appDb : IAppDb<TodoAppState> = AppDb<TodoAppState> initialState :> IAppDb<TodoAppState>

// Define a discriminated union for all Todo events
// type TodoEvent = 
//     | AddTodo of string
//     | ToggleTodo of int
//     | DeleteTodo of int
//     | SetFilter of string
//     | SetLoading of bool
//     | SetTodos of TodoItem list  // New event to set todos from API

// Define another union type to demonstrate that there's no conflict
// type AdminEvent = 
//     | AddUser of string    // Note that this has same shape as AddTodo
//     | DeleteUser of int    // Note that this has same shape as DeleteTodo
//     | TogglePermission of int  // Note that this has same shape as ToggleTodo

// === Method 1: Using traditional event identifiers (auto-generated) ===
// let addTodoEvent = EventId.auto<string>()
// let toggleTodoEvent = EventId.auto<int>()
// let deleteTodoEvent = EventId.auto<int>()
// let setFilterEvent = EventId.auto<string>()
// let setLoadingEvent = EventId.auto<bool>()
// let setTodosEvent = EventId.auto<TodoItem list>()


// === Method 2: Using type-based events ===
// let todoEventId = EventId.ofType<TodoEvent>()



// === Method 2: Using union-based event handling (safer approach) with manual case names ===
// registerTypedEventHandler  (fun event state ->
//     match event with
//     | AddTodo text ->
//         let newTodo = { id = state.nextId; text = text; completed = false }
//         { state with 
//             todos = state.todos @ [newTodo]
//             nextId = state.nextId + 1 }
//     | ToggleTodo id ->
//         { state with
//             todos = state.todos |> List.map (fun todo ->
//                 if todo.id = id then { todo with completed = not todo.completed } else todo
//             )
//         }
//     | DeleteTodo id ->
//         { state with
//             todos = state.todos |> List.filter (fun todo -> todo.id <> id)
//         }
//     | SetFilter filter ->
//         { state with filter = filter }
//     | SetLoading isLoading ->
//         { state with isLoading = isLoading }
//     | SetTodos todos ->
//         { state with 
//             todos = todos
//             nextId = 
//                 match todos with
//                 | [] -> 1
//                 | _ -> (todos |> List.map (fun t -> t.id) |> List.max) + 1
//             isLoading = false
//         }
// ) 

// registerTypedEventHandler (fun event state ->
//     match event with
//     | AddUser name ->
//         printfn "Adding user: %s" name
//         // Handle adding a user
//         state
//     | DeleteUser id ->
//         printfn "Deleting user with ID: %d" id
//         // Handle deleting a user
//         state
//     | TogglePermission id ->
//         printfn "Toggling permission for user with ID: %d" id
//         // Handle toggling permission for a user
//         state
// )
