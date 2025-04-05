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

    /// <summary>
    /// Creates a header element (&lt;h1&gt;) for top-level headings
    /// </summary>
    /// <example>
    /// <code>
    /// h1 {
    ///     className "title"
    ///     text "Main Title"
    /// }
    /// </code>
    /// </example>
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
    /// <summary>
    /// Creates an audio element (&lt;audio&gt;) for playing sound content
    /// </summary>
    /// <example>
    /// <code>
    /// audio {
    ///     src "audio.mp3"
    ///     controls true
    ///     autoPlay false
    ///     preload PreloadStrategy.Auto
    /// }
    /// </code>
    /// </example>
    let audio = MediaElementBuilder("audio")

    /// <summary>
    /// Creates a video element (&lt;video&gt;) for playing video content
    /// </summary>
    /// <example>
    /// <code>
    /// video {
    ///     src "video.mp4"
    ///     controls true
    ///     width 640
    ///     height 360
    ///     autoPlay false
    ///     muted true
    /// }
    /// </code>
    /// </example>
    let video = MediaElementBuilder("video")

    // Media-related elements
    /// <summary>
    /// Creates a source element (&lt;source&gt;) for specifying multiple media resources
    /// </summary>
    /// <example>
    /// <code>
    /// video {
    ///     source {
    ///         src "video.webm"
    ///         type' "video/webm"
    ///     }
    ///     source {
    ///         src "video.mp4"
    ///         type' "video/mp4"
    ///     }
    /// }
    /// </code>
    /// </example>
    let source = SourceElementBuilder()

    /// <summary>
    /// Creates a track element (&lt;track&gt;) for specifying timed text tracks
    /// </summary>
    /// <example>
    /// <code>
    /// video {
    ///     track {
    ///         kind TrackKind.Subtitles
    ///         src "captions.vtt"
    ///         srclang "en"
    ///         label "English"
    ///         default true
    ///     }
    /// }
    /// </code>
    /// </example>
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

    /// <summary>
    /// Creates a fieldset element (&lt;fieldset&gt;) to group form controls
    /// </summary>
    /// <example>
    /// <code>
    /// fieldset {
    ///     legend { text "Personal Information" }
    ///     input {
    ///         type' InputType.Text
    ///         placeholder "Name"
    ///     }
    /// }
    /// </code>
    /// </example>
    let fieldset = HtmlElementBuilder("fieldset")
    let legend = HtmlElementBuilder("legend")

    /// <summary>
    /// Creates a table element (&lt;table&gt;) for data presentation
    /// </summary>
    /// <example>
    /// <code>
    /// table {
    ///     className "data-table"
    ///     thead {
    ///         tr {
    ///             th { text "Header 1" }
    ///             th { text "Header 2" }
    ///         }
    ///     }
    ///     tbody {
    ///         tr {
    ///             td { text "Data 1" }
    ///             td { text "Data 2" }
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    let table = TableElementBuilder()
    let thead = HtmlElementBuilder("thead")
    let tbody = HtmlElementBuilder("tbody")
    let tfoot = HtmlElementBuilder("tfoot")
    let tr = HtmlElementBuilder("tr")
    let td = TableCellElementBuilder("td")

    /// <summary>
    /// Creates a table header cell (&lt;th&gt;) with optional scope and spanning
    /// </summary>
    /// <example>
    /// <code>
    /// th {
    ///     scope TableCellScope.Col
    ///     colSpan 2
    ///     text "Header spanning two columns"
    /// }
    /// </code>
    /// </example>
    let th = TableCellElementBuilder("th")

    // Table-related elements
    let colgroup = ColGroupBuilder()
    let col = ColBuilder()

    // Meta elements
    let meta = MetaElementBuilder()
    let link = LinkElementBuilder()

    // Semantic elements
    /// <summary>
    /// Creates a semantic article element (&lt;article&gt;) for self-contained content
    /// </summary>
    /// <example>
    /// <code>
    /// article {
    ///     header {
    ///         h1 { text "Article Title" }
    ///         time {
    ///             datetime "2024-01-01"
    ///             text "January 1, 2024"
    ///         }
    ///     }
    ///     p { text "Article content" }
    /// }
    /// </code>
    /// </example>
    let article = HtmlElementBuilder("article")

    /// <summary>
    /// Creates an aside element (&lt;aside&gt;) for related content
    /// </summary>
    /// <example>
    /// <code>
    /// aside {
    ///     className "sidebar"
    ///     h2 { text "Related Articles" }
    ///     ul {
    ///         li { a { href "#1"; text "Article 1" } }
    ///         li { a { href "#2"; text "Article 2" } }
    ///     }
    /// }
    /// </code>
    /// </example>
    let aside = HtmlElementBuilder("aside")

    /// <summary>
    /// Creates a header element (&lt;header&gt;) for introductory content
    /// </summary>
    /// <example>
    /// <code>
    /// header {
    ///     className "page-header"
    ///     h1 { text "Site Title" }
    ///     nav {
    ///         ul {
    ///             li { text "Menu Item" }
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    let header = HtmlElementBuilder("header")

    /// <summary>
    /// Creates a footer element (&lt;footer&gt;) for content that concludes a section
    /// </summary>
    /// <example>
    /// <code>
    /// footer {
    ///     className "site-footer"
    ///     p { text "Copyright © 2024" }
    ///     nav {
    ///         a { href "/privacy"; text "Privacy Policy" }
    ///     }
    /// }
    /// </code>
    /// </example>
    let footer = HtmlElementBuilder("footer")

    /// <summary>
    /// Creates a figure element (&lt;figure&gt;) for self-contained content like images, diagrams
    /// </summary>
    /// <example>
    /// <code>
    /// figure {
    ///     img {
    ///         src "diagram.png"
    ///         alt "System diagram"
    ///     }
    ///     figcaption { text "Figure 1: System Architecture" }
    /// }
    /// </code>
    /// </example>
    let figure = HtmlElementBuilder("figure")

    /// <summary>
    /// Creates a figure caption (&lt;figcaption&gt;) to describe a figure element
    /// </summary>
    let figcaption = HtmlElementBuilder("figcaption")

    /// <summary>
    /// Creates a time element (&lt;time&gt;) for dates and times
    /// </summary>
    /// <example>
    /// <code>
    /// time {
    ///     datetime "2024-01-01T15:00:00"
    ///     text "January 1st, 2024 at 3 PM"
    /// }
    /// </code>
    /// </example>
    let time = HtmlElementBuilder("time")

    /// <summary>
    /// Creates a mark element (&lt;mark&gt;) for highlighted text
    /// </summary>
    /// <example>
    /// <code>
    /// p {
    ///     text "This is "
    ///     mark { text "highlighted" }
    ///     text " text"
    /// }
    /// </code>
    /// </example>
    let mark = HtmlElementBuilder("mark")

    // Additional structural elements
    /// <summary>
    /// Creates an address element (&lt;address&gt;) for contact information
    /// </summary>
    /// <example>
    /// <code>
    /// address {
    ///     text "Contact us at: "
    ///     a {
    ///         href "mailto:example@example.com"
    ///         text "example@example.com"
    ///     }
    /// }
    /// </code>
    /// </example>
    let address = HtmlElementBuilder("address")

    /// <summary>
    /// Creates a blockquote element (&lt;blockquote&gt;) for quoted content
    /// </summary>
    /// <example>
    /// <code>
    /// blockquote {
    ///     cite "https://example.com/quote"
    ///     p { text "The quoted text goes here" }
    ///     footer {
    ///         text "— Author Name"
    ///     }
    /// }
    /// </code>
    /// </example>
    let blockquote = HtmlElementBuilder("blockquote")

    /// <summary>
    /// Creates a code element (&lt;code&gt;) for inline code
    /// </summary>
    /// <example>
    /// <code>
    /// p {
    ///     text "Use the "
    ///     code { text "console.log()" }
    ///     text " function to debug."
    /// }
    /// </code>
    /// </example>
    let code = HtmlElementBuilder("code")

    /// <summary>
    /// Creates a pre element (&lt;pre&gt;) for preformatted text
    /// </summary>
    /// <example>
    /// <code>
    /// pre {
    ///     code {
    ///         text "let x = 42
    ///               printfn \"%d\" x"
    ///     }
    /// }
    /// </code>
    /// </example>
    let pre = HtmlElementBuilder("pre")

    // Text formatting elements
    /// <summary>
    /// Creates an em element (&lt;em&gt;) for emphasized text
    /// </summary>
    /// <example>
    /// <code>
    /// p {
    ///     text "This is "
    ///     em { text "emphasized" }
    ///     text " text"
    /// }
    /// </code>
    /// </example>
    let em = HtmlElementBuilder("em")

    /// <summary>
    /// Creates a strong element (&lt;strong&gt;) for strongly emphasized text
    /// </summary>
    /// <example>
    /// <code>
    /// p {
    ///     text "This is "
    ///     strong { text "important" }
    ///     text " text"
    /// }
    /// </code>
    /// </example>
    let strong = HtmlElementBuilder("strong")

    // Additional semantic elements
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

    /// <summary>
    /// Creates a small element (&lt;small&gt;) for side-comments and fine print
    /// </summary>
    /// <example>
    /// <code>
    /// p {
    ///     text "Main text "
    ///     small { text "(additional info)" }
    /// }
    /// </code>
    /// </example>
    let small = HtmlElementBuilder("small")

    /// <summary>
    /// Creates a subscript element (&lt;sub&gt;) for subscript text
    /// </summary>
    /// <example>
    /// <code>
    /// p {
    ///     text "H"
    ///     sub { text "2" }
    ///     text "O"
    /// }
    /// </code>
    /// </example>
    let sub = HtmlElementBuilder("sub")

    /// <summary>
    /// Creates a superscript element (&lt;sup&gt;) for superscript text
    /// </summary>
    /// <example>
    /// <code>
    /// p {
    ///     text "E=mc"
    ///     sup { text "2" }
    ///     text ""
    /// }
    /// </code>
    /// </example>
    let sup = HtmlElementBuilder("sup")

    /// <summary>
    /// Creates a cite element (&lt;cite&gt;) for citation or reference
    /// </summary>
    /// <example>
    /// <code>
    /// p {
    ///     text "As mentioned in "
    ///     cite { text "The Art of Programming" }
    /// }
    /// </code>
    /// </example>
    let cite = HtmlElementBuilder("cite")

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
