module Demo.AppState

open FreeAct.FreeFrame
open FreeAct.FreeFrameRouter
open Demo.Todo.Types
open Fable.Core.JS

// Define the application state that includes router state
type AppState = {
    Router: RouterState
    // Add other application state here as needed
    Count: int
    TodoState: TodoAppState
}

// Initialize the app database with initial state
let appDb = AppDb<AppState>({
    Router = createDefaultRouterState()
    Count = 0
    TodoState = initialTodoState
})
console.log("AppDb initialized:", appDb)

// Helper functions to get and set router state
let getRouterState (state: AppState) = state.Router
let setRouterState (routerState: RouterState) (state: AppState) =
    // Only update if something actually changed
    if routerState <> state.Router then
        { state with Router = routerState }
    else
        state
