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

    type OrderedListElement = HtmlElementBuilder
    let ol = OrderedListElement "ol"

    type UnorderedListElement = HtmlElementBuilder
    let ul = UnorderedListElement "ul"

    type ListItemElement = HtmlElementBuilder
    let li = ListItemElement "li"

    type Header1Element = HtmlElementBuilder
    let h1 = Header1Element "h1"

    type Header2Element = HtmlElementBuilder
    let h2 = Header2Element "h2"

    type Header3Element = HtmlElementBuilder
    let h3 = Header3Element "h3"

    type Header4Element = HtmlElementBuilder
    let h4 = Header4Element "h4"

    type Header5Element = HtmlElementBuilder
    let h5 = Header5Element "h5"

    let button = ButtonElement()
    let p = HtmlElementBuilder("p")

    let a = AnchorElement()
    let section = HtmlElementBuilder("section")
    let span = HtmlElementBuilder("span")
    let i = HtmlElementBuilder("i")

    let label = HtmlElementBuilder("label")
    let img = HtmlElementBuilder("img")

    let input = HtmlElementBuilder("input")
    let strong = HtmlElementBuilder "strong"

    let script = ScriptElement()

    let form = FormElement()

    let fragment = FragmentElement()

    let none: ReactElement = null
