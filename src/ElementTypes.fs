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
