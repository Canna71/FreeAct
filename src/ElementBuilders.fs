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
    let div = DivisionElement()

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
    let nav = NavigationElement()

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
    let main = MainContentElement()

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
    let ol = OrderedListElement("ol")

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
    let ul = UnorderedListElement("ul")

    /// <summary>
    /// Creates a list item element (&lt;li&gt;) for ordered and unordered lists
    /// </summary>
    /// <example>
    /// <code>
    /// li {
    ///     value 1
    ///     text "List item"
    /// }
    /// </code>
    /// </example>
    type ListItemElement = HtmlElementBuilder
    let li = ListItemElement("li") // Changed from direct HtmlElementBuilder to typed builder

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
    let h1 = HeadingElement(1)

    type Header2Element = HtmlElementBuilder
    let h2 = HeadingElement(2)

    type Header3Element = HtmlElementBuilder
    let h3 = HeadingElement(3)

    type Header4Element = HtmlElementBuilder
    let h4 = HeadingElement(4)

    type Header5Element = HtmlElementBuilder
    let h5 = HeadingElement(5)

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
    type ParagraphElement = HtmlElementBuilder
    let p = ParagraphElement("p")

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

    /// <summary>
    /// Creates a section element (&lt;section&gt;) to define a generic section of content
    /// </summary>
    /// <example>
    /// <code>
    /// section {
    ///     className "content-section"
    ///     h2 { text "Section Title" }
    ///     p { text "Section content" }
    /// }
    /// </code>
    /// </example>
    type SectionElement = HtmlElementBuilder
    let section = SectionElement("section")

    /// <summary>
    /// Creates a span element (&lt;span&gt;) for inline content grouping
    /// </summary>
    /// <example>
    /// <code>
    /// p {
    ///     text "This is "
    ///     span {
    ///         className "highlight"
    ///         text "highlighted"
    ///     }
    ///     text " text"
    /// }
    /// </code>
    /// </example>
    type SpanElement = HtmlElementBuilder
    let span = SpanElement("span")

    /// <summary>
    /// Creates an i element (&lt;i&gt;) typically used for icons or italic text
    /// </summary>
    /// <example>
    /// <code>
    /// i {
    ///     className "fas fa-star"
    ///     aria "hidden" "true"
    /// }
    /// </code>
    /// </example>
    type ItalicElement = HtmlElementBuilder
    let i = ItalicElement("i")

    /// <summary>
    /// Creates a label element (&lt;label&gt;) to define a label for a form control
    /// </summary>
    /// <example>
    /// <code>
    /// label {
    ///     htmlFor "username"
    ///     text "Username:"
    /// }
    /// </code>
    /// </example>
    type LabelElement = HtmlElementBuilder
    let label = LabelElement("label")

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

    /// <summary>
    /// Creates a script element (&lt;script&gt;) for JavaScript code or external scripts
    /// </summary>
    /// <example>
    /// <code>
    /// script {
    ///     src "app.js"
    ///     defer true
    ///     type "text/javascript"
    /// }
    /// </code>
    /// </example>
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

    /// <summary>
    /// Creates a fragment to group elements without adding a DOM container
    /// </summary>
    /// <example>
    /// <code>
    /// fragment {
    ///     div { text "First" }
    ///     div { text "Second" }
    /// }
    /// </code>
    /// </example>
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
    let audio = AudioElementBuilder()

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
    let video = VideoElementBuilder()

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
    let fieldset = FieldsetElement() // Change from HtmlElementBuilder to FieldsetElement

    /// <summary>
    /// Creates a legend element (&lt;legend&gt;) to define a caption for a fieldset
    /// </summary>
    /// <example>
    /// <code>
    /// fieldset {
    ///     legend { text "Personal Information" }
    /// }
    /// </code>
    /// </example>
    let legend = LegendElement() // Change from HtmlElementBuilder to LegendElement

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

    /// <summary>
    /// Creates a table header section (&lt;thead&gt;) for grouping header content
    /// </summary>
    /// <example>
    /// <code>
    /// thead {
    ///     tr {
    ///         th { text "Column 1" }
    ///         th { text "Column 2" }
    ///     }
    /// }
    /// </code>
    /// </example>
    let thead = TableHeaderElement()

    /// <summary>
    /// Creates a table body section (&lt;tbody&gt;) for grouping body content
    /// </summary>
    /// <example>
    /// <code>
    /// tbody {
    ///     tr {
    ///         td { text "Data 1" }
    ///         td { text "Data 2" }
    ///     }
    /// }
    /// </code>
    /// </example>
    let tbody = TableBodyElement()

    /// <summary>
    /// Creates a table footer section (&lt;tfoot&gt;) for summary rows
    /// </summary>
    /// <example>
    /// <code>
    /// tfoot {
    ///     tr {
    ///         td { text "Total:" }
    ///         td { text "100" }
    ///     }
    /// }
    /// </code>
    /// </example>
    let tfoot = TableFooterElement()

    /// <summary>
    /// Creates a table row (&lt;tr&gt;) to contain cells
    /// </summary>
    /// <example>
    /// <code>
    /// tr {
    ///     td { text "Cell 1" }
    ///     td { text "Cell 2" }
    /// }
    /// </code>
    /// </example>
    let tr = TableRowElement()

    /// <summary>
    /// Creates a table data cell (&lt;td&gt;) for regular content
    /// </summary>
    /// <example>
    /// <code>
    /// td {
    ///     colSpan 2
    ///     text "Spans two columns"
    /// }
    /// </code>
    /// </example>
    let td = TableDataCellElementBuilder()

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
    let th = TableHeaderCellElementBuilder()

    // Table-related elements
    /// <summary>
    /// Creates a column group (&lt;colgroup&gt;) to define column properties
    /// </summary>
    /// <example>
    /// <code>
    /// colgroup {
    ///     span 2
    ///     col { className "narrow" }
    ///     col { className "wide" }
    /// }
    /// </code>
    /// </example>
    let colgroup = ColGroupBuilder()

    /// <summary>
    /// Creates a column (&lt;col&gt;) to specify properties for table columns
    /// </summary>
    /// <example>
    /// <code>
    /// col {
    ///     span 3
    ///     className "highlight"
    /// }
    /// </code>
    /// </example>
    let col = ColBuilder()

    // Meta elements
    /// <summary>
    /// Creates a meta element (&lt;meta&gt;) for document metadata
    /// </summary>
    /// <example>
    /// <code>
    /// meta {
    ///     name "description"
    ///     content "Page description"
    /// }
    /// </code>
    /// </example>
    let meta = MetaElementBuilder()

    /// <summary>
    /// Creates a link element (&lt;link&gt;) for external resources
    /// </summary>
    /// <example>
    /// <code>
    /// link {
    ///     rel LinkRelationType.Stylesheet
    ///     href "styles.css"
    /// }
    /// </code>
    /// </example>
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
    let article = ArticleElement()

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
    let aside = AsideElement()

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
    let header = HeaderElement()

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
    let footer = FooterElement()

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
    let figure = FigureElement()

    /// <summary>
    /// Creates a figure caption (&lt;figcaption&gt;) to describe a figure element
    /// </summary>
    let figcaption = FigCaptionElement()

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
    let time = TimeElement()

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
    let mark = MarkElement()

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
    type DialogElement = DialogElementBuilder
    let dialog = DialogElement()

    /// <summary>
    /// Creates a details element (&lt;details&gt;) with expandable content
    /// </summary>
    /// <example>
    /// <code>
    /// details {
    ///     open' true
    ///     summary { text "Click to expand" }
    ///     div { text "Expanded content" }
    /// }
    /// </code>
    /// </example>
    type DetailsElement = DetailsElementBuilder
    let details = DetailsElement()

    /// <summary>
    /// Creates a summary element (&lt;summary&gt;) as a heading for details element
    /// </summary>
    /// <example>
    /// <code>
    /// details {
    ///     summary {
    ///         className "details-header"
    ///         text "Click here"
    ///     }
    /// }
    /// </code>
    /// </example>
    let summary = SummaryElement()

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
    let small = SmallElement()

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
    let sub = SubscriptElement()

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
    let sup = SuperscriptElement()

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
    let cite = CitationElement()

    // Form-related elements
    /// <summary>
    /// Creates an option element (&lt;option&gt;) for select elements
    /// </summary>
    /// <example>
    /// <code>
    /// option {
    ///     value "1"
    ///     selected true
    ///     text "Option 1"
    /// }
    /// </code>
    /// </example>
    let option = OptionElementBuilder()

    /// <summary>
    /// Creates an option group element (&lt;optgroup&gt;) to group related options
    /// </summary>
    /// <example>
    /// <code>
    /// select {
    ///     optgroup {
    ///         label "Group 1"
    ///         option { value "1"; text "Option 1" }
    ///         option { value "2"; text "Option 2" }
    ///     }
    /// }
    /// </code>
    /// </example>
    let optgroup = OptionGroupElement()

    /// <summary>
    /// Creates a datalist element (&lt;datalist&gt;) for input suggestions
    /// </summary>
    /// <example>
    /// <code>
    /// datalist {
    ///     id "browsers"
    ///     option { value "Chrome" }
    ///     option { value "Firefox" }
    ///     option { value "Safari" }
    /// }
    /// </code>
    /// </example>
    let datalist = DataListElementBuilder()

    /// <summary>
    /// Creates an output element (&lt;output&gt;) for calculation results
    /// </summary>
    /// <example>
    /// <code>
    /// output {
    ///     htmlFor "calculation"
    ///     text "Result: 42"
    /// }
    /// </code>
    /// </example>
    let output = OutputElementBuilder()

    // Progress elements
    /// <summary>
    /// Creates a progress element (&lt;progress&gt;) to show completion progress
    /// </summary>
    /// <example>
    /// <code>
    /// progress {
    ///     value 70.0
    ///     max 100.0
    ///     text "70%"
    /// }
    /// </code>
    /// </example>
    let progress = ProgressElementBuilder()

    /// <summary>
    /// Creates a meter element (&lt;meter&gt;) to represent a scalar value within a range
    /// </summary>
    /// <example>
    /// <code>
    /// meter {
    ///     value 0.6
    ///     min 0.0
    ///     max 1.0
    ///     low 0.2
    ///     high 0.8
    ///     optimum 0.5
    /// }
    /// </code>
    /// </example>
    let meter = MeterElementBuilder()

    // Canvas element
    /// <summary>
    /// Creates a canvas element (&lt;canvas&gt;) for drawing graphics
    /// </summary>
    /// <example>
    /// <code>
    /// canvas {
    ///     width 800
    ///     height 600
    ///     contextType CanvasContextType.TwoD
    /// }
    /// </code>
    /// </example>
    let canvas = CanvasElementBuilder()

    // Ruby text elements
    /// <summary>
    /// Creates a ruby element (&lt;ruby&gt;) for East Asian typography
    /// </summary>
    /// <example>
    /// <code>
    /// ruby {
    ///     text "漢"
    ///     rt { text "かん" }
    /// }
    /// </code>
    /// </example>
    let ruby = RubyElement()

    /// <summary>
    /// Creates a ruby base element (&lt;rb&gt;) for containing the base text in ruby annotations
    /// </summary>
    /// <example>
    /// <code>
    /// ruby {
    ///     rb { text "漢" }
    ///     rt { text "かん" }
    /// }
    /// </code>
    /// </example>
    let rb = RubyBaseBuilder()

    /// <summary>
    /// Creates a ruby text element (&lt;rt&gt;) for containing the pronunciation in ruby annotations
    /// </summary>
    let rt = RubyTextBuilder()

    /// <summary>
    /// Creates a ruby parenthesis element (&lt;rp&gt;) for containing fallback parentheses
    /// </summary>
    let rp = RubyParenthesisBuilder()

    // Template element
    /// <summary>
    /// Creates a template element (&lt;template&gt;) for holding client-side content
    /// </summary>
    /// <example>
    /// <code>
    /// template {
    ///     id "template-id"
    ///     div { text "Template content" }
    /// }
    /// </code>
    /// </example>
    let template = TemplateElementBuilder()

    // Image-related elements
    /// <summary>
    /// Creates a picture element (&lt;picture&gt;) for responsive images
    /// </summary>
    /// <example>
    /// <code>
    /// picture {
    ///     source {
    ///         media "(min-width: 800px)"
    ///         srcset "large.jpg"
    ///     }
    ///     img {
    ///         src "small.jpg"
    ///         alt "Responsive image"
    ///     }
    /// }
    /// </code>
    /// </example>
    let picture = PictureElementBuilder()

    // Text formatting elements
    let em = EmphasisElement()
    let strong = StrongElement()
    let code = CodeElement()
    let pre = PreformattedElement()
    let blockquote = BlockquoteElement()
    let address = AddressElement()
