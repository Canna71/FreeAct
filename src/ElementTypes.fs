namespace FreeAct

open FreeAct

[<AutoOpen>]
module ElementTypes =
    /// Basic HTML element builders - enforce consistent naming

    // Change from Element to Builder suffix for consistency
    type DivBuilder() =
        inherit HtmlElementBuilder("div")

    type NavBuilder() =
        inherit HtmlElementBuilder("nav")

    type MainBuilder() =
        inherit HtmlElementBuilder("main")

    type SectionBuilder() =
        inherit HtmlElementBuilder("section")

    type ArticleBuilder() =
        inherit HtmlElementBuilder("article")

    type AsideBuilder() =
        inherit HtmlElementBuilder("aside")

    type HeaderBuilder() =
        inherit HtmlElementBuilder("header")

    type FooterBuilder() =
        inherit HtmlElementBuilder("footer")

    // Text elements
    type ParagraphBuilder() =
        inherit HtmlElementBuilder("p")

    type SpanBuilder() =
        inherit HtmlElementBuilder("span")

    type EmphasisBuilder() =
        inherit HtmlElementBuilder("em")

    type StrongBuilder() =
        inherit HtmlElementBuilder("strong")

    type CodeBuilder() =
        inherit HtmlElementBuilder("code")

    type PreBuilder() =
        inherit HtmlElementBuilder("pre")

    type BlockquoteBuilder() =
        inherit HtmlElementBuilder("blockquote")

    // List elements
    type OrderedListBuilder() =
        inherit HtmlElementBuilder("ol")

    type UnorderedListBuilder() =
        inherit HtmlElementBuilder("ul")

    type ListItemBuilder() =
        inherit HtmlElementBuilder("li")

    // Text formatting elements
    type CitationBuilder() =
        inherit HtmlElementBuilder("cite")

    type SmallBuilder() =
        inherit HtmlElementBuilder("small")

    type SubscriptBuilder() =
        inherit HtmlElementBuilder("sub")

    type SuperscriptBuilder() =
        inherit HtmlElementBuilder("sup")

    type AddressBuilder() =
        inherit HtmlElementBuilder("address")

    type RubyBuilder() =
        inherit HtmlElementBuilder("ruby")

    // Form and input elements
    type OptionGroupBuilder() =
        inherit HtmlElementBuilder("optgroup")

    type FieldsetBuilder() =
        inherit HtmlElementBuilder("fieldset")

    type LegendBuilder() =
        inherit HtmlElementBuilder("legend")

    // Additional HTML elements
    type TableHeaderBuilder() =
        inherit HtmlElementBuilder("thead")

    type TableBodyBuilder() =
        inherit HtmlElementBuilder("tbody")

    type TableFooterBuilder() =
        inherit HtmlElementBuilder("tfoot")

    type TableRowBuilder() =
        inherit HtmlElementBuilder("tr")

    type FigureBuilder() =
        inherit HtmlElementBuilder("figure")

    type FigCaptionBuilder() =
        inherit HtmlElementBuilder("figcaption")

    type TimeBuilder() =
        inherit HtmlElementBuilder("time")

    type MarkBuilder() =
        inherit HtmlElementBuilder("mark")

    type SummaryBuilder() =
        inherit HtmlElementBuilder("summary")

    /// Basic HTML document structure elements
    type DocumentTitleBuilder() =
        inherit HtmlElementBuilder("title")

    type DocumentHeadBuilder() =
        inherit HtmlElementBuilder("head")

    type DocumentBodyBuilder() =
        inherit HtmlElementBuilder("body")

    type DocumentHtmlBuilder() =
        inherit HtmlElementBuilder("html")

    /// Heading elements
    type HeadingBuilder(level: int) =
        inherit HtmlElementBuilder($"h{level}")

    // Add proper media element builders
    type AudioBuilder() =
        inherit MediaElementBuilder("audio")

    type VideoBuilder() =
        inherit MediaElementBuilder("video")

    // Add specific table cell builders
    type TableDataCellBuilder() =
        inherit TableCellElementBuilder("td")

    type TableHeaderCellBuilder() =
        inherit TableCellElementBuilder("th")

    // Core document elements
    type DocumentBuilder() =
        inherit HtmlElementBuilder("html")

    type HeadBuilder() =
        inherit HtmlElementBuilder("head")

    type BodyBuilder() =
        inherit HtmlElementBuilder("body")

    type TitleBuilder() =
        inherit HtmlElementBuilder("title")

    // Text and paragraph elements
    type LabelBuilder() =
        inherit HtmlElementBuilder("label")

    // Form elements

    type OptionBuilder() =
        inherit HtmlElementBuilder("option")

    // Media elements
    type SourceBuilder() =
        inherit HtmlElementBuilder("source")

    type TrackBuilder() =
        inherit HtmlElementBuilder("track")

    // Additional builders needed
    type ButtonBuilder() =
        inherit InteractiveFormElement("button")

    type FragmentBuilder() =
        inherit ElementWithChildrenBuilder(Fable.React.ReactBindings.React.Fragment)

    type ScriptBuilder() =
        inherit CoreElementBuilder("script")

    type ItalicBuilder() =
        inherit HtmlElementBuilder("i")

    type AnchorBuilder() =
        inherit HtmlElementBuilder("a")

    // Additional elements that were missing
    type ImageBuilder() =
        inherit HtmlElementBuilder("img")

    type LinkBuilder() =
        inherit HtmlElementBuilder("link")
