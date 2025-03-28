module Demo.Main

printfn "Hello, from the demo!"

// render a simple HTML page using the Fable.React library
open Fable.React
open FreeAct
open Browser
open Fable.React.ReactBindings

let app = 
    div {
      "Hello, world!"
    }

let container = Browser.Dom.document.getElementById("root")
if isNull container then
    Browser.Dom.console.error("No element with id 'app' found.")
else
    let root = ReactDomClient.createRoot(container)
    root.render(app)

