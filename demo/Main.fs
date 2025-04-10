module Demo.Main


// render a simple HTML page using the Fable.React library
open Fable.React
open FreeAct
open Browser
open Fable.React.ReactBindings
open Fable.Core.JS

open FreeAct.Router
open FreeFrame
open FreeAct.FreeFrameRouter
open Demo.FreeFrameDemo
open Demo
open Demo.Todo.State
open Demo.Todo.UI
open AppState
open FreeSS




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
    useNewView appDb (fun state -> state.Count)

// Route handlers - they now clearly take RouteMatchResult
let _home (context: RouteContext<ReactElement>) : ReactElement =
    let count = useCount()
    div {
        className "my"
        h1 { "Home" }
        p { "Welcome to the home page!" }
        p { sprintf "Current count: %d" count }
        
        button {
            style {
                backgroundColor "#4CAF50"

            }
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

let home = FunctionComponent.Of(_home, "Home")

let about (context: RouteContext<ReactElement>) =
    let ret = div {
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
            p { "one" }
            p { "two" }
        }
        
        context.ChildContent 

    }
    console.log("About page rendered with result: ", ret)
    ret

// Route handlers now receive RouteContext with child content
let users (context: RouteContext<ReactElement>) : ReactElement =
    console.log("Users route context: ", context)
    div {
        h1 { "Users" }
        p { "List of users." }
        
        // Links for navigation
        div {
            FreeFrameLink {| 
                appDb = appDb
                destination = "/users/id/42"
                className = Some "nav-link"
                children = [str "View User 42"] 
            |}
        }
        
        // Render child content where we want it to appear
        match context.ChildContent with
        | Some childContent -> 
            div {
                h2 { "Child Content" }
                className "child-content"
                childContent
            }
        | None -> None
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
                li { 
                    FreeFrameLink {| 
                        appDb = appDb
                        destination = "/users/profile"
                        className = None
                        children = [str "User Profile"] 
                    |} 
                }
                li { 
                    FreeFrameLink {| 
                        appDb = appDb
                        destination = "/users/settings"
                        className = None
                        children = [str "User Settings"] 
                    |} 
                }
            }
        }
    }

// Fix userRouter route handlers to use RouteContext
let userRouter = Router<ReactElement>()
userRouter.Route(
    "/profile",
    fun (context: RouteContext<ReactElement>) ->
        div {
            h2 { "User Profile" }
            p { "User profile details" }
            context.ChildContent 
        }
    )
    .Route(
        "/settings",
        fun (context: RouteContext<ReactElement>) ->
            div {
                h2 { "User Settings" }
                p { "User settings panel" }
                div {
                    FreeFrameLink {| 
                        appDb = appDb
                        destination = "/users/settings/privacy"
                        className = Some "nav-link"
                        children = [str "Privacy Settings"] 
                    |}
                }
                // Render nested content if present
                context.ChildContent 
            }
    )
    .Route(
        "/settings/privacy",
        fun (result) ->
            div {
                h2 { "Privacy Settings" }
                p { "User privacy settings" }
            }
    ) |> ignore

// Main app component using FreeFrameRouter
let App () =
    console.log("App component rendering...")
    
    // Create the router
    let router = Router<ReactElement>()
    router.Route("/", home)
          .Route("/about", about)
          .RouteWithChildren("/users", users, userRouter)
          .Route(
            "/users/id/:userId",
            fun (result) ->
                let userId = result.Result.PathParams.["userId"]
                div {
                    h1 { sprintf "User: %s" userId }
                    p { "User details go here." }
                    div {
                        FreeFrameLink {| 
                            appDb = appDb
                            destination = sprintf "/users/%s/posts/123" userId
                            className = Some "nav-link"
                            children = [str "View User's Post"] 
                        |}
                    }
                }
          )
          .Route(
            "/users/id/:userId/posts/:postId",
            fun (result) ->
                let userId = result.Result.PathParams.["userId"]
                let postId = result.Result.PathParams.["postId"]
                div {
                    h1 { sprintf "Post %s by User %s" postId userId }
                    p { "Post details go here." }
                    div {
                        FreeFrameLink {| 
                            appDb = appDb
                            destination = "/users"
                            className = Some "nav-link"
                            children = [str "Back to Users"] 
                        |}
                    }
                }
          )
          .Route("/users/admin", fun (result) -> div { h1 { "Admin Panel" } })
          .Route(
            "/search",
            fun result ->
                let query =
                    match result.Result.QueryParams.TryFind "q" with
                    | Some [ q ] -> q
                    | _ -> ""
                div {
                    h1 { sprintf "Search results for: %s" query }
                    p { "Search results go here." }
                }
          )
          .Route(
            "/freeframecomposition", 
            fun _ -> 
                div {
                    h1 { "FreeFrame Composition Demo" }
                    // Render the component directly
                    // FreeFrameDemo()
                    // CompositionDemo.CompositionDemo()
                }
          )
          .Route(
            "/tododemo", 
            fun _ -> 
                div {
                    h1 { "FreeFrame ToDo Demo" }
                    // Render the component directly
                    // FreeFrameDemo
                    TodoDemo()
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

// FreeAct.Router.Tests.testNestedRoutes()

testCssInFsharp()
