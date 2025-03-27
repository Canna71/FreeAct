namespace FreeAct

// HTML element builders
[<AutoOpen>]
module Builders =
    open Fable.React

    /// Defines a HTML div element
    let div = HtmlElementBuilder "div"
    let h1 = HtmlElementBuilder "h1"
    let button = HtmlElementBuilder("button")
    let p = HtmlElementBuilder("p")

    let a = AnchorElement()
    let section = HtmlElementBuilder("section")
    let span = HtmlElementBuilder("span")
    let i = HtmlElementBuilder("i")

    let label = HtmlElementBuilder("label")
    let img = HtmlElementBuilder("img")

    let input = HtmlElementBuilder("input")
    let strong = HtmlElementBuilder "strong"

    let none: ReactElement = null
