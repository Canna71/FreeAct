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

    type FieldsetBuilder() = // Was FieldsetElement
        inherit HtmlElementBuilder("fieldset")

    type LegendBuilder() = // Was LegendElement
        inherit HtmlElementBuilder("legend")

    type FormBuilder() = // Was FormElement
        inherit HtmlElementBuilder("form")

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

    type DialogBuilder() = // Was DialogElementBuilder
        inherit HtmlElementBuilder("dialog")

    type DetailsBuilder() = // Was DetailsElementBuilder
        inherit HtmlElementBuilder("details")

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
        inherit MediaElement("audio")

    type VideoBuilder() =
        inherit MediaElement("video")

    // Add specific table cell builders
    type TableDataCellBuilder() =
        inherit TableCellElement("td")

    type TableHeaderCellBuilder() =
        inherit TableCellElement("th")

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
    type ButtonBuilder() = // Was ButtonElement
        inherit InteractiveFormElement("button")

    type FragmentBuilder() =
        inherit ElementWithChildrenBuilder(Fable.React.ReactBindings.React.Fragment)

    type ScriptBuilder() = // Was ScriptElement
        inherit CoreElementBuilder("script")

    type ItalicBuilder() =
        inherit HtmlElementBuilder("i")

    // Change AnchorElement to AnchorBuilder for consistency but keep its functionality
    type AnchorBuilder() =
        inherit HtmlElementBuilder("a")

        [<CustomOperation("href")>]
        member inline _.Href(props, value: string) = Prop("href", value :> obj) :: props

        [<CustomOperation("target")>]
        member inline _.Target(props, value: Target) =
            Prop("target", targetToString value :> obj) :: props

        [<CustomOperation("download")>]
        member inline _.Download(props, value: string) = Prop("download", value :> obj) :: props

        [<CustomOperation("rel")>]
        member inline _.Rel(props, value: string) = Prop("rel", value :> obj) :: props

        [<CustomOperation("hreflang")>]
        member inline _.HrefLang(props, value: string) = Prop("hreflang", value :> obj) :: props

        [<CustomOperation("type")>]
        member inline _.Type(props, value: string) = Prop("type", value :> obj) :: props

        [<CustomOperation("referrerpolicy")>]
        member inline _.ReferrerPolicy(props, value: ReferrerPolicy) =
            Prop("referrerpolicy", referrerPolicyToString value :> obj) :: props

    // Additional elements that were missing
    type ImageBuilder() =
        inherit HtmlElementBuilder("img")

    type LinkBuilder() =
        inherit HtmlElementBuilder("link")
