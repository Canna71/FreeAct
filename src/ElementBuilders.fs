namespace FreeAct

[<AutoOpen>]
module Builders =
    open Fable.React

    // Core structure elements
    /// <summary>Creates a div element for grouping content</summary>
    let div = DivBuilder()
    /// <summary>Creates a navigation section</summary>
    let nav = NavBuilder()
    /// <summary>Creates the main content section</summary>
    let main = MainBuilder()
    /// <summary>Creates a standalone section</summary>
    let section = SectionBuilder()
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
    let ol = OrderedListBuilder()
    /// <summary>Creates an unordered list</summary>
    let ul = UnorderedListBuilder()
    /// <summary>Creates a list item</summary>
    let li = ListItemBuilder()

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
    let strong = StrongBuilder()
    /// <summary>Creates inline code text</summary>
    let code = CodeBuilder()
    /// <summary>Creates preformatted text block</summary>
    let pre = PreBuilder()
    /// <summary>Creates a block quotation</summary>
    let blockquote = BlockquoteBuilder()
    /// <summary>Creates contact information section</summary>
    let address = AddressBuilder()
    /// <summary>Creates small print text</summary>
    let small = SmallBuilder()
    /// <summary>Creates subscript text</summary>
    let sub = SubscriptBuilder()
    /// <summary>Creates superscript text</summary>
    let sup = SuperscriptBuilder()
    /// <summary>Creates a citation</summary>
    let cite = CitationBuilder()
    /// <summary>Creates italic text</summary>
    let i = ItalicBuilder()

    // Form elements - Fix form-related elements to use correct builders
    /// <summary>Creates a button with form submission capabilities</summary>
    let button = ButtonElement() // Correct - has button type and form attributes
    /// <summary>Creates an input field with various input types</summary>
    let input = InputElementBuilder() // Correct - has input specific attributes
    /// <summary>Creates a form element for data submission</summary>
    let form = FormElement() // Correct - has form events and attributes
    /// <summary>Creates a select dropdown element</summary>
    let select = SelectElementBuilder() // Correct - has multiple/size attributes
    /// <summary>Creates a multi-line text input area</summary>
    let textarea = TextAreaElementBuilder() // Correct - has rows/cols attributes
    /// <summary>Creates a fieldset to group form controls</summary>
    let fieldset = FieldsetElement() // Correct
    /// <summary>Creates a caption for a fieldset</summary>
    let legend = LegendElement() // Correct
    /// <summary>Creates an option for select elements</summary>
    let option = OptionElementBuilder() // Correct - has selected attribute
    /// <summary>Creates a group of related options</summary>
    let optgroup = OptionGroupBuilder() // Correct
    /// <summary>Creates a list of predefined options for input fields</summary>
    let datalist = DataListElementBuilder() // Correct
    /// <summary>Creates an output element for calculation results</summary>
    let output = OutputElementBuilder() // Correct - has htmlFor attribute

    // Media elements - All need MediaElementBuilder base
    /// <summary>Creates an image element with loading and accessibility options</summary>
    let img = ImageElementBuilder() // Correct - has image specific attributes
    /// <summary>Creates an audio player element</summary>
    let audio = AudioBuilder() // Correct - inherits from MediaElementBuilder
    /// <summary>Creates a video player element</summary>
    let video = VideoBuilder() // Correct - inherits from MediaElementBuilder
    /// <summary>Creates a source element for media resources</summary>
    let source = SourceElementBuilder() // Correct - has media attributes
    /// <summary>Creates a text track for media elements</summary>
    let track = TrackElementBuilder() // Correct - has track attributes

    // Container and semantic elements
    /// <summary>Creates a fragment for grouping elements without a container</summary>
    let fragment = FragmentBuilder()
    /// <summary>Creates a modal dialog element</summary>
    let dialog = DialogElementBuilder() // Correct - has dialog specific events
    /// <summary>Creates an expandable details section</summary>
    let details = DetailsElementBuilder() // Correct - has toggle event
    /// <summary>Creates a summary for a details element</summary>
    let summary = SummaryBuilder()

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
    let table = TableElementBuilder()

    /// <summary>Creates a table header section</summary>
    let thead = TableHeaderBuilder()

    /// <summary>Creates a table body section</summary>
    let tbody = TableBodyBuilder()

    /// <summary>Creates a table footer section</summary>
    let tfoot = TableFooterBuilder()

    /// <summary>Creates a table row</summary>
    let tr = TableRowBuilder()

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
    let colgroup = ColGroupBuilder()

    /// <summary>Creates a column definition</summary>
    let col = ColBuilder()

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
    let meta = MetaElementBuilder()

    /// <summary>Creates a link element for external resources</summary>
    /// <example>
    /// <code>
    /// link {
    ///     rel LinkRelationType.Stylesheet
    ///     href "styles.css"
    /// }
    /// </code>
    /// </example>
    let link = LinkElementBuilder()

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

    // Specialized elements
    let figure = FigureBuilder()
    let figcaption = FigCaptionBuilder()
    let time = TimeBuilder()
    let mark = MarkBuilder()
    let ruby = RubyBuilder()
    let rb = RubyBaseBuilder()
    let rt = RubyTextBuilder()
    let rp = RubyParenthesisBuilder()
    let template = TemplateElementBuilder()
    let progress = ProgressElementBuilder()
    let meter = MeterElementBuilder()
    let canvas = CanvasElementBuilder()

    let none: ReactElement = null

    // Core elements
    /// <summary>Creates an anchor element for navigation links</summary>
    let a = AnchorElement() // Correct - has href/target attributes
