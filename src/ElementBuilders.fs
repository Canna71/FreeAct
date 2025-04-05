namespace FreeAct

[<AutoOpen>]
module Builders =
    open Fable.React

    // Core structure elements
    let div = DivBuilder()
    let nav = NavBuilder()
    let main = MainBuilder()
    let section = SectionBuilder()
    let article = ArticleBuilder()
    let aside = AsideBuilder()
    let header = HeaderBuilder()
    let footer = FooterBuilder()

    // List elements
    let ol = OrderedListBuilder()
    let ul = UnorderedListBuilder()
    let li = ListItemBuilder()

    // Heading elements
    let h1 = HeadingBuilder(1)
    let h2 = HeadingBuilder(2)
    let h3 = HeadingBuilder(3)
    let h4 = HeadingBuilder(4)
    let h5 = HeadingBuilder(5)
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
    let button = ButtonElement() // Correct - has button type and form attributes
    let input = InputElementBuilder() // Correct - has input specific attributes
    let form = FormElement() // Correct - has form events and attributes
    let select = SelectElementBuilder() // Correct - has multiple/size attributes
    let textarea = TextAreaElementBuilder() // Correct - has rows/cols attributes
    let fieldset = FieldsetElement() // Correct
    let legend = LegendElement() // Correct
    let option = OptionElementBuilder() // Correct - has selected attribute
    let optgroup = OptionGroupBuilder() // Correct
    let datalist = DataListElementBuilder() // Correct
    let output = OutputElementBuilder() // Correct - has htmlFor attribute

    // Media elements - All need MediaElementBuilder base
    let img = ImageElementBuilder() // Correct - has image specific attributes
    let audio = AudioBuilder() // Correct - inherits from MediaElementBuilder
    let video = VideoBuilder() // Correct - inherits from MediaElementBuilder
    let source = SourceElementBuilder() // Correct - has media attributes
    let track = TrackElementBuilder() // Correct - has track attributes

    // Container and semantic elements
    let fragment = FragmentBuilder()
    let dialog = DialogElementBuilder() // Correct - has dialog specific events
    let details = DetailsElementBuilder() // Correct - has toggle event
    let summary = SummaryBuilder()

    // Table elements
    let table = TableElementBuilder()
    let thead = TableHeaderBuilder()
    let tbody = TableBodyBuilder()
    let tfoot = TableFooterBuilder()
    let tr = TableRowBuilder()
    let td = TableDataCellBuilder()
    let th = TableHeaderCellBuilder()
    let colgroup = ColGroupBuilder()
    let col = ColBuilder()

    // Document elements
    let html = DocumentBuilder()
    let head = HeadBuilder()
    let body = BodyBuilder()
    let title = TitleBuilder()

    // Metadata and script elements
    let meta = MetaElementBuilder()
    let link = LinkElementBuilder() // Correct - has rel/href attributes
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
    let a = AnchorElement() // Correct - has href/target attributes
