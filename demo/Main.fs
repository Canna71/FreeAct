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
open Demo.FreeFrameDemo
open Demo

let home route =
    div {
        h1 { "Home" }
        p { "Welcome to the home page!" }
        
        // Example of using Link component
        div {
            Link {| destination = "/about"; className = Some "nav-link"; children = [str "Go to About"] |}
        }
    }
let about route =
    div {
        h1 { "About" }
        p { "This is the about page." }
        
        // Example of using Link component
        div {
            Link {| destination = "/users"; className = Some "nav-link"; children = [str "View Users"] |}
        }
    }

let users route =
    div {
        h1 { "Users" }
        p { "List of users." }
        
        // Example of using Link component
        div {
            Link {| destination = "/users/42"; className = Some "nav-link"; children = [str "View User 42"] |}
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

// Create a router instance
let setupRouter () =
    let router = Router<ReactElement>()
    
    router
        .Route("/", home)
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
                    // FreeFrameDemo()
                    TodoDemo.TodoDemo()
                }
        )

// Create a custom navigation component
let Navigation () =
    div {
        className "navigation"
        nav {
            ul {
                li { Link {| destination = "/"; className = None; children = [str "Home"] |} }
                li { Link {| destination = "/about"; className = None; children = [str "About"] |} }
                li { Link {| destination = "/users"; className = None; children = [str "Users"] |} }
                li { Link {| destination = "/search?q=example"; className = None; children = [str "Search"] |} }
                li { Link {| destination = "/freeframeeffects"; className = None; children = [str "FreeFrame Demo"] |} }
                li { Link {| destination = "/freeframecomposition"; className = None; children = [str "FreeFrame Composition"] |} }
                li { Link {| destination = "/tododemo"; className = None; children = [str "FreeFrame Todo"] |} }

            }
        }
    }

// Main app component
let App () =
    console.log("App component rendering...")
    let router = setupRouter()
    
    RouterProvider {
        Router = router
        Mode = RouterMode.HistoryAPI  // Change to HashBased if you prefer hash-based routing
        DefaultContent = notFound 
        Children = [
            div {
                className "app-container"
                Navigation()
                main {
                    className "content"
                    Routes {| DefaultContent = notFound |}
                }
            }
        ]
    }

// Initialize the application
let container = Browser.Dom.document.getElementById("root")
if isNull container then
    Browser.Dom.console.error("No element with id 'root' found.")
else
    let root = ReactDomClient.createRoot(container)
    console.log("Rendering App...")
    root.render(App())