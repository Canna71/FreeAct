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
    let img = ImageElementBuilder()

    let input = HtmlElementBuilder("input")
    let strong = HtmlElementBuilder "strong"

    let script = ScriptElement()

    let form = FormElement()

    let fragment = FragmentElement()

    let none: ReactElement = null

    // Media elements
    let audio = MediaElementBuilder("audio")
    let video = MediaElementBuilder("video")

    // Form elements
    let select = SelectElementBuilder()
    let textarea = TextAreaElementBuilder()
    let fieldset = HtmlElementBuilder("fieldset")
    let legend = HtmlElementBuilder("legend")

    // Table elements
    let table = TableElementBuilder()
    let thead = HtmlElementBuilder("thead")
    let tbody = HtmlElementBuilder("tbody")
    let tfoot = HtmlElementBuilder("tfoot")
    let tr = HtmlElementBuilder("tr")
    let td = TableCellElementBuilder("td")
    let th = TableCellElementBuilder("th")

    // Meta elements
    let meta = MetaElementBuilder()
    let link = LinkElementBuilder()

    // Semantic elements
    let article = HtmlElementBuilder("article")
    let aside = HtmlElementBuilder("aside")
    let footer = HtmlElementBuilder("footer")
    let header = HtmlElementBuilder("header")

    // Additional semantic elements
    let figure = HtmlElementBuilder("figure")
    let figcaption = HtmlElementBuilder("figcaption")
    let details = DetailsElementBuilder()
    let summary = HtmlElementBuilder("summary")
    let dialog = DialogElementBuilder()
    let time = HtmlElementBuilder("time")
    let mark = HtmlElementBuilder("mark")

    // Additional structural elements
    let address = HtmlElementBuilder("address")
    let blockquote = HtmlElementBuilder("blockquote")
    let cite = HtmlElementBuilder("cite")
    let code = HtmlElementBuilder("code")
    let pre = HtmlElementBuilder("pre")

    // Additional inline elements
    let em = HtmlElementBuilder("em")
    let small = HtmlElementBuilder("small")
    let sub = HtmlElementBuilder("sub")
    let sup = HtmlElementBuilder("sup")

    // Form-related elements
    let option = OptionElementBuilder()
    let optgroup = HtmlElementBuilder("optgroup")

    // Progress elements
    let progress = ProgressElementBuilder()
    let meter = MeterElementBuilder()

    // Canvas element
    let canvas = CanvasElementBuilder()

    // Form related elements
    let datalist = DataListElementBuilder()
    let output = OutputElementBuilder()

    // Ruby text elements
    let ruby = HtmlElementBuilder("ruby")
    let rb = RubyBaseBuilder()
    let rt = RubyTextBuilder()
    let rp = RubyParenthesisBuilder()

    // Template element
    let template = TemplateElementBuilder()
