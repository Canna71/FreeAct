namespace FreeAct

// HTML element builders
[<AutoOpen>]
module Builders =
    open Fable.React

    /// Defines a HTML div element
    type DivElement = HtmlElementBuilder
    let div = DivElement "div"

    type NavElement = HtmlElementBuilder
    let nav = NavElement "nav"

    type MainElement = HtmlElementBuilder
    let main = MainElement "main"

    type UnorderedListElement = HtmlElementBuilder
    let ul = UnorderedListElement "ul"

    type ListItemElement = HtmlElementBuilder
    let li = ListItemElement "li"

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
