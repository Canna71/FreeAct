[<AutoOpen>]
module FreeAct.Builders

open Fable.React

// Core structure elements
/// <summary>Creates a div element for grouping content</summary>
let div = DivElements()
/// <summary>Creates a navigation section</summary>
let nav = NavBuilder()
/// <summary>Creates the main content section</summary>
let main = MainElement()
/// <summary>Creates a standalone section</summary>
let section = SectionElement()
/// <summary>Creates an article section</summary>
let article = ArticleBuilder()
/// <summary>Creates an aside section for related content</summary>
let aside = AsideBuilder()
/// <summary>Creates a header section</summary>
let header = HeaderBuilder()
/// <summary>Creates a footer section</summary>
let footer = FooterBuilder()

// List elements
/// <summary>Creates an ordered list</summary>
let ol = OrderedListElement()
/// <summary>Creates an unordered list</summary>
let ul = UnorderedListElement()
/// <summary>Creates a list item</summary>
let li = ListItemElement()

// Heading elements
/// <summary>Creates a top-level heading (h1) for page or section titles</summary>
/// <example>
/// <code>
/// h1 {
///     className "main-title"
///     text "Welcome to My App"
/// }
/// </code>
/// </example>
let h1 = HeadingBuilder(1)

/// <summary>Creates a second-level heading (h2) for major sections</summary>
/// <example>
/// <code>
/// h2 {
///     id "section-features"
///     text "Features"
/// }
/// </code>
/// </example>
let h2 = HeadingBuilder(2)

/// <summary>Creates a third-level heading (h3) for subsections</summary>
let h3 = HeadingBuilder(3)

/// <summary>Creates a fourth-level heading (h4) for sub-subsections</summary>
let h4 = HeadingBuilder(4)

/// <summary>Creates a fifth-level heading (h5) for deeper nesting</summary>
let h5 = HeadingBuilder(5)

/// <summary>Creates a sixth-level heading (h6) for the deepest nesting level</summary>
let h6 = HeadingBuilder(6)

// Text formatting elements with documentation
/// <summary>Creates a paragraph element for text content</summary>
let p = ParagraphBuilder()
/// <summary>Creates an inline text span element</summary>
let span = SpanBuilder()
/// <summary>Creates emphasized text</summary>
let em = EmphasisBuilder()
/// <summary>Creates strongly emphasized text</summary>
let strong = StrongElement()
/// <summary>Creates inline code text</summary>
let code = CodeElement()
/// <summary>Creates preformatted text block</summary>
let pre = PreElement()
/// <summary>Creates a block quotation</summary>
let blockquote = BlockquoteElement()
/// <summary>Creates contact information section</summary>
let address = AddressElement()
/// <summary>Creates small print text</summary>
let small = SmallElement()
/// <summary>Creates subscript text</summary>
let sub = SubscriptElement()
/// <summary>Creates superscript text</summary>
let sup = SuperscriptElement()
/// <summary>Creates a citation</summary>
let cite = CitationElement()
/// <summary>Creates italic text</summary>
let i = ItalicBuilder()

// Form elements - Fix form-related elements to use correct builders
/// <summary>Creates a button with form submission capabilities</summary>
let button = ButtonElement() // Correct - has button type and form attributes
/// <summary>Creates an input field with various input types</summary>
let input = InputElement() // Correct - has input specific attributes
/// <summary>Creates a form element for data submission</summary>
let form = FormElement() // Correct - has form events and attributes
/// <summary>Creates a select dropdown element</summary>
let select = SelectElement() // Correct - has multiple/size attributes
/// <summary>Creates a multi-line text input area</summary>
let textarea = TextAreaElement() // Correct - has rows/cols attributes
/// <summary>Creates a fieldset to group form controls</summary>
let fieldset = FieldsetElement() // Correct
/// <summary>Creates a caption for a fieldset</summary>
let legend = LegendElement() // Correct
/// <summary>Creates an option for select elements</summary>
let option = OptionElement() // Correct - has selected attribute
/// <summary>Creates a group of related options</summary>
let optgroup = OptionGroupElement() // Correct

// Media elements - All need MediaElementBuilder base
/// <summary>Creates an image element with loading and accessibility options</summary>
let img = ImageElement() // Correct - has image specific attributes

/// <summary>Creates a text track for media elements</summary>
let track = TrackElement() // Correct - has track attributes

// Container and semantic elements
/// <summary>Creates a fragment for grouping elements without a container</summary>
let fragment = FragmentBuilder()
/// <summary>Creates a modal dialog element</summary>
let dialog = DialogElement() // Correct - has dialog specific events
/// <summary>Creates an expandable details section</summary>
let details = DetailsElement() // Correct - has toggle event
/// <summary>Creates a summary for a details element</summary>
let summary = SummaryElement()

// Table elements with documentation
/// <summary>Creates a table element for structured data presentation</summary>
/// <example>
/// <code>
/// table {
///     thead { tr { th { text "Header" } } }
///     tbody { tr { td { text "Cell" } } }
/// }
/// </code>
/// </example>
let table = TableElement()

/// <summary>Creates a table header section</summary>
let thead = TableHeaderElement()

/// <summary>Creates a table body section</summary>
let tbody = TableBodyElement()

/// <summary>Creates a table footer section</summary>
let tfoot = TableFooterElement()

/// <summary>Creates a table row</summary>
let tr = TableRowElement()

/// <summary>Creates a table data cell</summary>
let td = TableDataCellBuilder()

/// <summary>Creates a table header cell with scope control</summary>
/// <example>
/// <code>
/// th {
///     scope TableCellScope.Col
///     text "Column Header"
/// }
/// </code>
/// </example>
let th = TableHeaderCellBuilder()

/// <summary>Creates a column group for table columns</summary>
/// <example>
/// <code>
/// colgroup {
///     span 2
///     col { width 100 }
/// }
/// </code>
/// </example>
let colgroup = ColGroup()

/// <summary>Creates a column definition</summary>
let col = ColElement()

// Document elements with documentation
/// <summary>Creates the root HTML element</summary>
let html = DocumentBuilder()

/// <summary>Creates the document head section</summary>
let head = HeadBuilder()

/// <summary>Creates the document body</summary>
let body = BodyBuilder()

/// <summary>Creates the document title</summary>
let title = TitleBuilder()

// Metadata and script elements with documentation
/// <summary>Creates a meta tag for document metadata</summary>
/// <example>
/// <code>
/// meta {
///     name "description"
///     content "Page description"
/// }
/// </code>
/// </example>
let meta = MetaElement()

/// <summary>Creates a link element for external resources</summary>
/// <example>
/// <code>
/// link {
///     rel LinkRelationType.Stylesheet
///     href "styles.css"
/// }
/// </code>
/// </example>
let link = LinkElement()

/// <summary>Creates a script element for JavaScript</summary>
/// <example>
/// <code>
/// script {
///     src "app.js"
///     defer true
/// }
/// </code>
/// </example>
let script = ScriptBuilder()

// Specialized elements with documentation and examples
/// <summary>Creates a figure container for self-contained content like images or diagrams</summary>
/// <example>
/// <code>
/// figure {
///     img {
///         src "diagram.png"
///         alt "Architecture diagram"
///     }
///     figcaption { text "Figure 1: System Architecture" }
/// }
/// </code>
/// </example>
let figure = FigureElement()

/// <summary>Creates a caption for a figure element</summary>
/// <example>
/// <code>
/// figcaption {
///     className "caption"
///     text "Description of the above figure"
/// }
/// </code>
/// </example>
let figcaption = FigCaptionElement()

/// <summary>Creates a time element for dates and timestamps</summary>
/// <example>
/// <code>
/// time {
///     datetime "2024-01-01T12:00:00"
///     text "January 1st at noon"
/// }
/// </code>
/// </example>
let time = TimeElement()

/// <summary>Creates a mark element for highlighting text</summary>
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

/// <summary>Creates a ruby container for East Asian typography annotations</summary>
/// <example>
/// <code>
/// ruby {
///     rb { text "漢" }
///     rt { text "かん" }
/// }
/// </code>
/// </example>
let ruby = RubyElement()

/// <summary>Creates a ruby base text element</summary>
let rb = RubyBase()

/// <summary>Creates a ruby text annotation element</summary>
let rt = RubyText()

/// <summary>Creates a ruby fallback parenthesis element</summary>
let rp = RubyParenthesis()

/// <summary>Creates a template element for holding client-side content templates</summary>
/// <example>
/// <code>
/// template {
///     id "my-template"
///     div {
///         className "template-content"
///         text "Template content here"
///     }
/// }
/// </code>
/// </example>
let template = TemplateElement()

/// <summary>Creates a progress element for task completion indication</summary>
/// <example>
/// <code>
/// progress {
///     value 75.0
///     max 100.0
///     text "75%"
/// }
/// </code>
/// </example>
let progress = ProgressElement()

/// <summary>Creates a meter element for gauge visualization</summary>
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
let meter = MeterElement()

/// <summary>Creates a canvas element for drawing graphics</summary>
/// <example>
/// <code>
/// canvas {
///     width 800
///     height 600
///     contextType CanvasContextType.TwoD
/// }
/// </code>
/// </example>
let canvas = CanvasElement()

let none: ReactElement = null

// Core elements
/// <summary>Creates an anchor element for navigation links</summary>
let a = AnchorElement() // Correct - has href/target attributes

// Specialized elements with documentation and examples
/// <summary>Creates an audio player element with media controls</summary>
/// <example>
/// <code>
/// audio {
///     src "music.mp3"
///     controls true
///     autoPlay false
///     onCanPlay (fun _ -> console.log "Audio ready")
/// }
/// </code>
/// </example>
let audio = AudioBuilder()

/// <summary>Creates a video player element with playback controls</summary>
/// <example>
/// <code>
/// video {
///     src "video.mp4"
///     width 640
///     height 480
///     controls true
///     muted true
/// }
/// </code>
/// </example>
let video = VideoBuilder()

/// <summary>Creates a source element for specifying multiple media formats</summary>
/// <example>
/// <code>
/// source {
///     src "video.webm"
///     type' "video/webm"
///     media "(min-width: 1200px)"
/// }
/// </code>
/// </example>
let source = SourceBuilder()

/// <summary>Creates a datalist element for input suggestions</summary>
/// <example>
/// <code>
/// datalist {
///     id "browsers"
///     option { value "Chrome" }
///     option { value "Firefox" }
/// }
/// </code>
/// </example>
let datalist = DataListElement()

/// <summary>Creates an output element to display calculation results</summary>
/// <example>
/// <code>
/// output {
///     htmlFor "calc"
///     text "Result: 42"
/// }
/// </code>
/// </example>
let output = OutputElement()
