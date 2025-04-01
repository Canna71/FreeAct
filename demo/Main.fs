module Demo.Main

printfn "Hello, from the demo!"

// render a simple HTML page using the Fable.React library
open Fable.React
open FreeAct
open Browser
open Fable.React.ReactBindings
open Fable.Core.JS

open FreeAct.Router
open FreeAct.ReactRouter
open FreeAct.FreeFrame
open FreeAct.FreeFrameRouter
open Demo.FreeFrameDemo
open Demo

// Define the application state that includes router state
type AppState = {
    Router: RouterState
    // Add other application state here as needed
    Count: int
}

// Initialize the app database with initial state
let appDb = AppDb<AppState>({
    Router = createDefaultRouterState()
    Count = 0
})

// Helper functions to get and set router state
let getRouterState (state: AppState) = state.Router
let setRouterState (routerState: RouterState) (state: AppState) =
    // Only update if something actually changed
    if routerState <> state.Router then
        { state with Router = routerState }
    else
        state

// Example of an additional event for counter
type CounterEvent = 
    | Increment
    | Decrement

// Register counter event handlers
registerTypedEventHandler<CounterEvent, AppState>(fun ev state ->
    match ev with
    | Increment -> 
        { state with Count = state.Count + 1 }
    | Decrement -> 
        { state with Count = state.Count - 1 }
)

// Create subscriptions for parts of the state
let useCount () =
    useView appDb (fun state -> state.Count)

// Route handlers
let home route =
    let count = useCount()
    
    div {
        h1 { "Home" }
        p { "Welcome to the home page!" }
        p { sprintf "Current count: %d" count }
        
        button {
            onClick (fun _ -> dispatchTyped appDb Increment)
            "Increment"
        }
        
        button {
            onClick (fun _ -> dispatchTyped appDb Decrement)
            "Decrement"
        }
        
        // Example of using FreeFrameLink component
        div {
            FreeFrameLink {| 
                appDb = appDb
                destination = "/about"
                className = Some "nav-link"
                children = [str "Go to About"] 
            |}
        }
    }

let about route =
    div {
        h1 { "About" }
        p { "This is the about page." }
        
        // Example of using Link component
        div {
            FreeFrameLink {| 
                appDb = appDb
                destination = "/users"
                className = Some "nav-link"
                children = [str "View Users"] 
            |}
        }
    }

let users route =
    div {
        h1 { "Users" }
        p { "List of users." }
        
        // Example of using Link component
        div {
            FreeFrameLink {| 
                appDb = appDb
                destination = "/users/42"
                className = Some "nav-link"
                children = [str "View User 42"] 
            |}
        }
    }

let notFound  =
    div {
        h1 { "404 Not Found" }
        p { "The page you are looking for does not exist." }
        div {
            "test"
            // Link {| destination = "/"; className = Some "nav-link"; children = [str "Go Home"] |}
        }
    }

// Create a custom navigation component using FreeFrameLink
let Navigation () =
    div {
        className "navigation"
        nav {
            ul {
                li { 
                    FreeFrameLink {| 
                        appDb = appDb
                        destination = "/"
                        className = None
                        children = [str "Home"] 
                    |} 
                }
                li { 
                    FreeFrameLink {| 
                        appDb = appDb
                        destination = "/about"
                        className = None
                        children = [str "About"] 
                    |} 
                }
                li { 
                    FreeFrameLink {| 
                        appDb = appDb
                        destination = "/users"
                        className = None
                        children = [str "Users"] 
                    |} 
                }
                li { 
                    FreeFrameLink {| 
                        appDb = appDb
                        destination = "/search?q=example"
                        className = None
                        children = [str "Search"] 
                    |} 
                }
                li { 
                    FreeFrameLink {| 
                        appDb = appDb
                        destination = "/freeframeeffects"
                        className = None
                        children = [str "FreeFrame Demo"] 
                    |} 
                }
                li { 
                    FreeFrameLink {| 
                        appDb = appDb
                        destination = "/freeframecomposition"
                        className = None
                        children = [str "FreeFrame Composition"] 
                    |} 
                }
                li { 
                    FreeFrameLink {| 
                        appDb = appDb
                        destination = "/tododemo"
                        className = None
                        children = [str "FreeFrame Todo"] 
                    |} 
                }
            }
        }
    }

// Main app component using FreeFrameRouter
let App () =
    console.log("App component rendering...")
    
    // Create the router
    let router = Router<ReactElement>()
    router.Route("/", home)
          .Route("/about", about)
          .Route("/users", users)
          .Route(
            "/users/:userId",
            fun result ->
                let userId = result.PathParams.["userId"]
                div {
                    h1 { sprintf "User: %s" userId }
                    p { "User details go here." }
                    div {
                        Link {| 
                            destination = sprintf "/users/%s/posts/123" userId
                            className = Some "nav-link"
                            children = [str "View User's Post"] 
                        |}
                    }
                }
          )
          .Route(
            "/users/:userId/posts/:postId",
            fun result ->
                let userId = result.PathParams.["userId"]
                let postId = result.PathParams.["postId"]
                div {
                    h1 { sprintf "Post %s by User %s" postId userId }
                    p { "Post details go here." }
                    div {
                        Link {| destination = "/users"; className = Some "nav-link"; children = [str "Back to Users"] |}
                    }
                }
          )
          .Route("/users/admin", fun _ -> div { h1 { "Admin Panel" } })
          .Route(
            "/search",
            fun result ->
                let query =
                    match result.QueryParams.TryFind "q" with
                    | Some [ q ] -> q
                    | _ -> ""
                div {
                    h1 { sprintf "Search results for: %s" query }
                    p { "Search results go here." }
                }
          )
          .Route(
            "/freeframeeffects", 
            fun _ -> 
                div {
                    h1 { "FreeFrame Effects Demo" }
                    // Render the component directly
                    // FreeFrameDemo()
                    EffectsDemo.EffectsDemo()
                }
          )
          .Route(
            "/freeframecomposition", 
            fun _ -> 
                div {
                    h1 { "FreeFrame Composition Demo" }
                    // Render the component directly
                    // FreeFrameDemo()
                    CompositionDemo.CompositionDemo()
                }
          )
          .Route(
            "/tododemo", 
            fun _ -> 
                div {
                    h1 { "FreeFrame ToDo Demo" }
                    // Render the component directly
                    // FreeFrameDemo
                    TodoDemo.TodoDemo()
                }
            )
          |> ignore
    
    FreeFrameRouterProvider {|
        AppDb = appDb
        Router = router
        GetRouterState = getRouterState
        SetRouterState = setRouterState
        Mode = RouterMode.HistoryAPI
        Children = [
            div {
                className "app-container"
                Navigation()
                main {
                    className "content"
                    FreeFrameRoutes {| 
                        Router = router
                        GetRouterState = getRouterState
                        AppDb = appDb
                        DefaultContent = notFound
                    |}
                }
            }
        ]
    |}

// Initialize the application
let container = Browser.Dom.document.getElementById("root")
if isNull container then
    Browser.Dom.console.error("No element with id 'root' found.")
else
    let root = ReactDomClient.createRoot(container)
    console.log("Rendering App...")
    root.render(App())