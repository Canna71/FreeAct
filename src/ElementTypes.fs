namespace FreeAct

open FreeAct

/// Basic HTML element builders
[<AutoOpen>]
module ElementTypes =
    // Text formatting elements
    type EmphasisElement() =
        inherit HtmlElementBuilder("em")

    type StrongElement() =
        inherit HtmlElementBuilder("strong")

    type CitationElement() =
        inherit HtmlElementBuilder("cite")

    type SmallElement() =
        inherit HtmlElementBuilder("small")

    type SubscriptElement() =
        inherit HtmlElementBuilder("sub")

    type SuperscriptElement() =
        inherit HtmlElementBuilder("sup")

    type CodeElement() =
        inherit HtmlElementBuilder("code")

    type PreformattedElement() =
        inherit HtmlElementBuilder("pre")

    type BlockquoteElement() =
        inherit HtmlElementBuilder("blockquote")

    type AddressElement() =
        inherit HtmlElementBuilder("address")

    type RubyElement() =
        inherit HtmlElementBuilder("ruby")

    // Form and input elements
    type OptionGroupElement() =
        inherit HtmlElementBuilder("optgroup")

    type FieldsetElement() =
        inherit HtmlElementBuilder("fieldset")

    type LegendElement() =
        inherit HtmlElementBuilder("legend")

    // Additional HTML elements
    type TableHeaderElement() =
        inherit HtmlElementBuilder("thead")

    type TableBodyElement() =
        inherit HtmlElementBuilder("tbody")

    type TableFooterElement() =
        inherit HtmlElementBuilder("tfoot")

    type TableRowElement() =
        inherit HtmlElementBuilder("tr")

    type ArticleElement() =
        inherit HtmlElementBuilder("article")

    type AsideElement() =
        inherit HtmlElementBuilder("aside")

    type HeaderElement() =
        inherit HtmlElementBuilder("header")

    type FooterElement() =
        inherit HtmlElementBuilder("footer")

    type FigureElement() =
        inherit HtmlElementBuilder("figure")

    type FigCaptionElement() =
        inherit HtmlElementBuilder("figcaption")

    type TimeElement() =
        inherit HtmlElementBuilder("time")

    type MarkElement() =
        inherit HtmlElementBuilder("mark")

    type SummaryElement() =
        inherit HtmlElementBuilder("summary")

    /// Basic HTML document structure elements
    type DocumentTitleElement() =
        inherit HtmlElementBuilder("title")

    type DocumentHeadElement() =
        inherit HtmlElementBuilder("head")

    type DocumentBodyElement() =
        inherit HtmlElementBuilder("body")

    type DocumentHtmlElement() =
        inherit HtmlElementBuilder("html")

    /// Heading elements
    type HeadingElement(level: int) =
        inherit HtmlElementBuilder($"h{level}")

    /// Division and structural elements
    type DivisionElement() =
        inherit HtmlElementBuilder("div")

    type NavigationElement() =
        inherit HtmlElementBuilder("nav")

    type MainContentElement() =
        inherit HtmlElementBuilder("main")

    type SectionElement() =
        inherit HtmlElementBuilder("section")

    /// Text and inline elements
    type ParagraphElement() =
        inherit HtmlElementBuilder("p")

    type SpanElement() =
        inherit HtmlElementBuilder("span")

    type BoldElement() =
        inherit HtmlElementBuilder("b")

    type ItalicElement() =
        inherit HtmlElementBuilder("i")

    /// List elements
    type OrderedListElement() =
        inherit HtmlElementBuilder("ol")

    type UnorderedListElement() =
        inherit HtmlElementBuilder("ul")

    type ListItemElement() =
        inherit HtmlElementBuilder("li")

    // Add proper media element builders
    type AudioElementBuilder() =
        inherit MediaElementBuilder("audio")

    type VideoElementBuilder() =
        inherit MediaElementBuilder("video")

    // Add specific table cell builders
    type TableDataCellElementBuilder() =
        inherit TableCellElementBuilder("td")

    type TableHeaderCellElementBuilder() =
        inherit TableCellElementBuilder("th")
