namespace FreeAct

// HTML element builders
[<AutoOpen>]
module Builders =
    open Fable.React

    /// Defines a HTML div element
    let div = CoreHtmlElementBuilder "div"
    let h1 = CoreHtmlElementBuilder "h1"
    let button = CoreHtmlElementBuilder("button")
    let p = CoreHtmlElementBuilder("p")
    let section = CoreHtmlElementBuilder("section")
    let span = CoreHtmlElementBuilder("span")
    let i = CoreHtmlElementBuilder("i")

    let label = CoreHtmlElementBuilder("label")
    let img = CoreHtmlElementBuilder("img")

    let input = CoreHtmlElementBuilder("input")

    let none: ReactElement = null
