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

    // Form elements
    let button = ButtonBuilder()
    let input = InputElementBuilder()
    let form = FormBuilder()
    let label = LabelBuilder()
    let select = SelectElementBuilder()
    let option = OptionBuilder()
    let optgroup = OptionGroupBuilder()
    let fieldset = FieldsetBuilder()
    let legend = LegendBuilder()
    let textarea = TextAreaElementBuilder()
    let datalist = DataListElementBuilder()
    let output = OutputElementBuilder()

    // Media elements
    let img = ImageBuilder()
    let audio = AudioBuilder()
    let video = VideoBuilder()
    let source = SourceBuilder()
    let track = TrackBuilder()
    let picture = PictureElementBuilder()

    // Container and semantic elements
    let fragment = FragmentBuilder()
    let dialog = DialogElementBuilder()
    let details = DetailsElementBuilder()
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
    let link = LinkBuilder()
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
    let a = AnchorBuilder()
