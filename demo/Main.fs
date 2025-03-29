module Demo.Main

printfn "Hello, from the demo!"

// render a simple HTML page using the Fable.React library
open Fable.React
open FreeAct
open Browser
open Fable.React.ReactBindings
open Fable.Core.JS

open FreeAct.Router
open FreeAct.Router.BrowserRouter

let home =
    div {
        h1 { "Home" }
        p { "Welcome to the home page!" }
    }
let about =
    div {
        h1 { "About" }
        p { "This is the about page." }
    }
let users =
    div {
        h1 { "Users" }
        p { "List of users." }
    }

let notFound =
    div {
        h1 { "404 Not Found" }
        p { "The page you are looking for does not exist." }
    }

/// Creates a router for a React application
let createRouter (router: Router<ReactElement>) (notFoundElement: ReactElement) =
    let mutable currentElement = null

    let root = ReactDomClient.createRoot(Browser.Dom.document.getElementById("app"))

    let render result =
        let element = 
            match result with
            | Some element -> element
            | None -> notFoundElement
        
        currentElement <- element
        root.render(currentElement)
        // ReactDomClient..render(element, Browser.Dom.document.getElementById("app"))
    
    BrowserRouter.create {
        Router = router
        Render = render
        BasePath = ""
        UseHashRouting = false
        NotFoundHandler = Some (fun () -> notFoundElement)
    }

let app = 

    let url = "/users/42/posts/123?sort=asc"
    let router = Router<ReactElement>()
    
    router
        .Route("/", fun _ -> home)
        .Route("/about", fun _ -> about)
        .Route("/users", fun _ -> users)
        .Route(
            "/users/:userId",
            fun result ->
                let userId = result.PathParams.["userId"]
                div {
                    h1 { sprintf "User: %s" userId }
                    p { "User details go here." }
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
      |> ignore

    // Create the browser router
    let browserRouter = createRouter router notFound
    BrowserRouter.start browserRouter |> ignore

    div {
      section {
        h1 { "Welcome to the demo!" }

        div {
          id "app"
        }
      }
    }

let container = Browser.Dom.document.getElementById("root")
if isNull container then
    Browser.Dom.console.error("No element with id 'root' found.")
else
    let root = ReactDomClient.createRoot(container)
    root.render(app)

