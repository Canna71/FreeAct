namespace FreeAct

// HTML element builders
[<AutoOpen>]
module Builders =
    open Fable.React

    /// <summary>
    /// Creates a div element (&lt;div&gt;), the most common container element
    /// </summary>
    /// <example>
    /// <code>
    /// div {
    ///     className "container"
    ///     div {
    ///         className "inner"
    ///         text "Content"
    ///     }
    /// }
    /// </code>
    /// </example>
    type DivElement = HtmlElementBuilder
    let div = DivElement "div"

    /// <summary>
    /// Creates a navigation section (&lt;nav&gt;) containing navigation links
    /// </summary>
    /// <example>
    /// <code>
    /// nav {
    ///     className "main-nav"
    ///     ul {
    ///         li { a { href "/"; text "Home" } }
    ///         li { a { href "/about"; text "About" } }
    ///     }
    /// }
    /// </code>
    /// </example>
    type NavElement = HtmlElementBuilder
    let nav = NavElement "nav"

    /// <summary>
    /// Creates a main content section (&lt;main&gt;). There should be only one per page
    /// </summary>
    /// <example>
    /// <code>
    /// main {
    ///     id "main-content"
    ///     aria "role" "main"
    ///     text "Main content here"
    /// }
    /// </code>
    /// </example>
    type MainElement = HtmlElementBuilder
    let main = MainElement "main"

    /// <summary>
    /// Creates an ordered list element (&lt;ol&gt;) for numbered lists
    /// </summary>
    /// <example>
    /// <code>
    /// ol {
    ///     li { text "First item" }
    ///     li { text "Second item" }
    /// }
    /// </code>
    /// </example>
    type OrderedListElement = HtmlElementBuilder
    let ol = OrderedListElement "ol"

    /// <summary>
    /// Creates an unordered list element (&lt;ul&gt;) for bulleted lists
    /// </summary>
    /// <example>
    /// <code>
    /// ul {
    ///     className "menu"
    ///     li { text "Menu item 1" }
    ///     li { text "Menu item 2" }
    /// }
    /// </code>
    /// </example>
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

    /// <summary>
    /// Creates a paragraph element (&lt;p&gt;) for text content
    /// </summary>
    /// <example>
    /// <code>
    /// p {
    ///     className "text-body"
    ///     text "This is a paragraph of text"
    /// }
    /// </code>
    /// </example>
    let p = HtmlElementBuilder("p")

    /// <summary>
    /// Creates an anchor element (&lt;a&gt;) for links
    /// </summary>
    /// <example>
    /// <code>
    /// a {
    ///     href "/page"
    ///     target Target.Blank
    ///     text "Click here"
    /// }
    /// </code>
    /// </example>
    let a = AnchorElement()
    let section = HtmlElementBuilder("section")
    let span = HtmlElementBuilder("span")
    let i = HtmlElementBuilder("i")

    let label = HtmlElementBuilder("label")

    /// <summary>
    /// Creates an image element (&lt;img&gt;) with required accessibility attributes
    /// </summary>
    /// <example>
    /// <code>
    /// img {
    ///     src "image.jpg"
    ///     alt "Descriptive text"
    ///     loading LoadingStrategy.Lazy
    /// }
    /// </code>
    /// </example>
    let img = ImageElementBuilder()

    /// <summary>
    /// Creates an input element (&lt;input&gt;) for user input with various types
    /// </summary>
    /// <example>
    /// <code>
    /// input {
    ///     type' InputType.Text
    ///     placeholder "Enter text"
    ///     required true
    ///     onInput (fun ev -> // Handle input)
    /// }
    /// </code>
    /// </example>
    let input = InputElementBuilder()

    let strong = HtmlElementBuilder "strong"

    let script = ScriptElement()

    /// <summary>
    /// Creates a form element (&lt;form&gt;) for user input
    /// </summary>
    /// <example>
    /// <code>
    /// form {
    ///     onSubmit (fun ev -> ev.preventDefault())
    ///     input {
    ///         type' InputType.Text
    ///         required true
    ///     }
    ///     button {
    ///         type' ButtonType.Submit
    ///         text "Submit"
    ///     }
    /// }
    /// </code>
    /// </example>
    let form = FormElement()

    let fragment = FragmentElement()

    let none: ReactElement = null

    // Media elements
    let audio = MediaElementBuilder("audio")
    let video = MediaElementBuilder("video")

    // Media-related elements
    let source = SourceElementBuilder()
    let track = TrackElementBuilder()

    /// <summary>
    /// Creates a select element (&lt;select&gt;) for dropdown lists
    /// </summary>
    /// <example>
    /// <code>
    /// select {
    ///     value selectedValue
    ///     onChange (fun ev -> // Handle selection)
    ///     option { value "1"; text "Option 1" }
    ///     option { value "2"; text "Option 2" }
    /// }
    /// </code>
    /// </example>
    let select = SelectElementBuilder()

    /// <summary>
    /// Creates a textarea element (&lt;textarea&gt;) for multi-line text input
    /// </summary>
    /// <example>
    /// <code>
    /// textarea {
    ///     rows 4
    ///     cols 50
    ///     placeholder "Enter long text"
    ///     value currentText
    ///     onChange (fun ev -> // Handle change)
    /// }
    /// </code>
    /// </example>
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

    // Table-related elements
    let colgroup = ColGroupBuilder()
    let col = ColBuilder()

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

    /// <summary>
    /// Creates a dialog element (&lt;dialog&gt;) for modal or non-modal interactions
    /// </summary>
    /// <example>
    /// <code>
    /// dialog {
    ///     open' true
    ///     aria "labelledby" "dialog-title"
    ///     h2 {
    ///         id "dialog-title"
    ///         text "Dialog Title"
    ///     }
    ///     button {
    ///         onClick (fun _ -> ())
    ///         text "Close"
    ///     }
    /// }
    /// </code>
    /// </example>
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

    // Image-related elements
    let picture = PictureElementBuilder()
