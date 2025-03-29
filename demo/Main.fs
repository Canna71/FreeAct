module Demo.Main

printfn "Hello, from the demo!"

// render a simple HTML page using the Fable.React library
open Fable.React
open FreeAct
open Browser
open Fable.React.ReactBindings
open Fable.Core.JS

open FreeAct.Router


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

let app = 

    let url = "/about"
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

    let content = router.Match(url) |> Option.defaultValue (div { h1 { "404 Not Found" } })

    div {
      "Hello, world!"
      section {
        h1 { "Welcome to the demo!" }

        content
      }
    }

let container = Browser.Dom.document.getElementById("root")
if isNull container then
    Browser.Dom.console.error("No element with id 'root' found.")
else
    let root = ReactDomClient.createRoot(container)
    root.render(app)

