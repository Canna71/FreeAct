namespace FreeAct

open FreeAct
open FreeAct.Interop

[<AutoOpen>]
module CoreElement =
    open Fable.Core.JsInterop

    type CoreElementBuilder(tag: string) =
        inherit ElementWithChildrenBuilder(tag)

        /// id
        /// <summary>
        /// Used to define the id of an element
        /// </summary>
        [<CustomOperation("id")>]
        member inline _.Id(props, value: string) = Prop("id", value :> obj) :: props

        /// <summary>
        /// Use it to assign a class name to an element
        /// </summary>
        [<CustomOperation("className")>]
        member inline x.ClassName(props, value: string) =
            Prop("className", value :> obj) :: props

        /// className as seq of strings
        [<CustomOperation("className")>]
        member inline x.ClassName(props, values: string seq) =
            Prop("className", String.concat " " values :> obj) :: props

        // slot
        /// <summary>
        /// Used to define the slot of an element
        /// </summary>
        [<CustomOperation("slot")>]
        member inline _.Slot(props, value: string) = Prop("slot", value :> obj) :: props

        // aria-* attributes
        /// <summary>
        /// Used to define a custom aria attribute on an element
        /// </summary>
        [<CustomOperation("aria")>]
        member inline _.Aria(props, ariaName: string, ariaValue: string) =
            Prop(sprintf "aria-%s" ariaName, ariaValue :> obj) :: props

        // dangerouslySetInnerHTML
        /// <summary>
        /// Used to define the inner HTML of an element
        /// </summary>
        [<CustomOperation("dangerouslySetInnerHTML")>]
        member inline _.DangerouslySetInnerHTML(props, value: string) =
            Prop("dangerouslySetInnerHTML", createObj [ "__html" ==> value ]) :: props

        // scrollLeft
        /// <summary>
        /// Used to define the scroll left of an element
        /// </summary>
        [<CustomOperation("scrollLeft")>]
        member inline _.ScrollLeft(props, value: int) =
            Prop("scrollLeft", value :> obj) :: props

        // scrollTop
        /// <summary>
        /// Used to define the scroll top of an element
        /// </summary>
        [<CustomOperation("scrollTop")>]
        member inline _.ScrollTop(props, value: int) =
            Prop("scrollTop", value :> obj) :: props

        // key property
        [<CustomOperation("key")>]
        member inline x.Key(props, value: string) = Prop("key", value :> obj) :: props

    type HtmlElementBuilder(tag: string) =
        inherit CoreElementBuilder(tag)

        // autoCapitalize
        /// <summary>
        /// Used to define the auto capitalization of an element
        /// </summary>
        [<CustomOperation("autoCapitalize")>]
        member inline _.AutoCapitalize(props, value: AutoCapitalize) =
            Prop("autoCapitalize", autoCapitalizeToString value :> obj) :: props

        // autoCorrect
        /// <summary>
        /// Used to define whether an element should be auto corrected or not
        /// </summary>
        [<CustomOperation("autoCorrect")>]
        member inline _.AutoCorrect(props, value: bool) =
            Prop("autoCorrect", value :> obj) :: props

        // autoFocus
        /// <summary>
        /// Used to define whether an element should be auto focused or not
        /// </summary>
        [<CustomOperation("autoFocus")>]
        member inline _.AutoFocus(props, value: bool) =
            Prop("autoFocus", value :> obj) :: props

        // data-* attributes
        /// <summary>
        /// Used to define a custom data attribute on an element
        /// </summary>
        [<CustomOperation("data")>]
        member inline _.Data(props, dataName: string, dataValue: string) =
            Prop(sprintf "data-%s" dataName, dataValue :> obj) :: props

        /// contentEditable
        [<CustomOperation("contentEditable")>]
        member inline _.ContentEditable(props, value: bool) =
            Prop("contentEditable", value :> obj) :: props

        /// contentEditable
        [<CustomOperation("contentEditable")>]
        member inline _.ContentEditable(props, value: PlaintextOnly) =
            Prop("contentEditable", "plaintext-only" :> obj) :: props

        /// dir
        [<CustomOperation("dir")>]
        member inline _.Dir(props, value: Direction) =
            Prop("dir", directionToString value :> obj) :: props

        /// draggable
        /// <summary>
        /// Used to define whether an element is draggable or not
        /// </summary>
        [<CustomOperation("draggable")>]
        member inline _.Draggable(props, value: bool) =
            Prop("draggable", value :> obj) :: props

        /// hidden
        [<CustomOperation("hidden")>]
        member inline _.Hidden(props, value: bool) = Prop("hidden", value :> obj) :: props

        /// inert
        /// <summary>
        /// Used to define whether an element is inert or not
        /// </summary>
        [<CustomOperation("inert")>]
        member inline _.Inert(props, value: bool) = Prop("inert", value :> obj) :: props

        // inputMode
        /// <summary>
        /// Used to define the input mode of an element
        /// </summary>
        [<CustomOperation("inputMode")>]
        member inline _.InputMode(props, value: InputMode) =
            Prop("inputMode", inputModeToString value :> obj) :: props

        // itemscope
        /// <summary>
        /// Used to define whether an element is a part of a microdata item or not
        /// </summary>
        [<CustomOperation("itemscope")>]
        member inline _.ItemScope(props, value: bool) =
            Prop("itemscope", value :> obj) :: props

        // itemType
        /// <summary>
        /// Used to define the type of an item in a microdata item
        /// </summary>
        [<CustomOperation("itemType")>]
        member inline _.ItemType(props, value: string) = Prop("itemType", value :> obj) :: props

        // itemID
        /// <summary>
        /// Used to define the ID of an item in a microdata item
        /// </summary>
        [<CustomOperation("itemID")>]
        member inline _.ItemID(props, value: string) = Prop("itemID", value :> obj) :: props

        // itemProp
        /// <summary>
        /// Used to define the properties of an item in a microdata item
        /// </summary>
        [<CustomOperation("itemProp")>]
        member inline _.ItemProp(props, value: string) = Prop("itemProp", value :> obj) :: props

        // itemRef
        /// <summary>
        /// Used to define the reference of an item in a microdata item
        /// </summary>
        [<CustomOperation("itemRef")>]
        member inline _.ItemRef(props, value: string) = Prop("itemRef", value :> obj) :: props

        /// lang
        [<CustomOperation("lang")>]
        member inline _.Lang(props, value: string) = Prop("lang", value :> obj) :: props

        /// <summary>
        /// Used to define whether an element is spell checked or not
        /// </summary>
        [<CustomOperation("spellCheck")>]
        member inline _.SpellCheck(props, value: bool) =
            Prop("spellCheck", value :> obj) :: props

        /// tabIndex
        [<CustomOperation("tabIndex")>]
        member inline _.TabIndex(props, value: int) = Prop("tabIndex", value :> obj) :: props

        /// translate
        /// <summary>
        /// Used to define whether the content of an element should be translated or not
        /// </summary>
        [<CustomOperation("translate")>]
        member inline _.Translate(props, value: bool) =
            Prop(
                "translate",
                if value then
                    "yes"
                else
                    "no" :> obj
            )
            :: props

        /// title
        [<CustomOperation("title")>]
        member inline _.Title(props, value: string) = Prop("title", value :> obj) :: props

        /// accessKey
        [<CustomOperation("accessKey")>]
        member inline _.AccessKey(props, value: string) =
            Prop("accessKey", value :> obj) :: props

        // TODO: The following should go in a derived type

        // src
        [<CustomOperation("src")>]
        member inline _.Src(props, value: string) = Prop("src", value :> obj) :: props

        // type as InputType
        [<CustomOperation("type'")>]
        member inline _.Type(props, value: InputType) =
            Prop("type", value.ToString() :> obj) :: props

        // height as int
        [<CustomOperation("height")>]
        member inline _.Height(props, value: int) = Prop("height", value :> obj) :: props

        /// height as float
        [<CustomOperation("height")>]
        member inline _.Height(props, value: float) = Prop("height", value :> obj) :: props
        // height as ICSSUnit
        [<CustomOperation("height")>]
        member inline _.Height(props, value: ICSSUnit) = Prop("height", value :> obj) :: props
        // width as int
        [<CustomOperation("width")>]
        member inline _.Width(props, value: int) = Prop("width", value :> obj) :: props
        // width as float
        [<CustomOperation("width")>]
        member inline _.Width(props, value: float) = Prop("width", value :> obj) :: props
        // width as ICSSUnit
        [<CustomOperation("width")>]
        member inline _.Width(props, value: ICSSUnit) = Prop("width", value :> obj) :: props
        // placeholder
        [<CustomOperation("placeholder")>]
        member inline _.Placeholder(props, value: string) =
            Prop("placeholder", value :> obj) :: props
        // value
        [<CustomOperation("value")>]
        member inline _.Value(props, value: obj) = Prop("value", value) :: props
        // defaultValue
        [<CustomOperation("defaultValue")>]
        member inline _.DefaultValue(props, value: obj) = Prop("defaultValue", value) :: props

    type AnchorElement() =
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

    type ScriptElement() =
        inherit CoreElementBuilder("script")

        [<CustomOperation("src")>]
        member inline _.Src(props, value: string) = Prop("src", value :> obj) :: props

        [<CustomOperation("async")>]
        member inline _.Async(props, value: bool) = Prop("async", value :> obj) :: props

        [<CustomOperation("defer")>]
        member inline _.Defer(props, value: bool) = Prop("defer", value :> obj) :: props

        [<CustomOperation("crossOrigin")>]
        member inline _.CrossOrigin(props, value: CrossOrigin) =
            Prop("crossOrigin", crossOriginToString value :> obj) :: props

        [<CustomOperation("type")>]
        member inline _.Type(props, value: string) = Prop("type", value :> obj) :: props

        [<CustomOperation("noModule")>]
        member inline _.NoModule(props, value: bool) = Prop("noModule", value :> obj) :: props

        [<CustomOperation("integrity")>]
        member inline _.Integrity(props, value: string) =
            Prop("integrity", value :> obj) :: props

        [<CustomOperation("referrerPolicy")>]
        member inline _.ReferrerPolicy(props, value: ReferrerPolicy) =
            Prop("referrerPolicy", referrerPolicyToString value :> obj) :: props

    type FormElement() =
        inherit HtmlElementBuilder("form")

        [<CustomOperation("action")>]
        member inline _.Action(props, value: string) = Prop("action", value :> obj) :: props

        [<CustomOperation("method")>]
        member inline _.Method(props, value: string) = Prop("method", value :> obj) :: props

        [<CustomOperation("target")>]
        member inline _.Target(props, value: Target) =
            Prop("target", targetToString value :> obj) :: props

        [<CustomOperation("enctype")>]
        member inline _.Enctype(props, value: string) = Prop("enctype", value :> obj) :: props

        [<CustomOperation("autocomplete")>]
        member inline _.AutoComplete(props, value: string) =
            Prop("autocomplete", value :> obj) :: props

        [<CustomOperation("novalidate")>]
        member inline _.NoValidate(props, value: bool) =
            Prop("novalidate", value :> obj) :: props

        [<CustomOperation("acceptCharset")>]
        member inline _.AcceptCharset(props, value: string) =
            Prop("acceptCharset", value :> obj) :: props

    type InteractiveFormElement(tag: string) =
        inherit HtmlElementBuilder(tag)

        [<CustomOperation("disabled")>]
        member inline _.Disabled(props, value: bool) = Prop("disabled", value :> obj) :: props

        [<CustomOperation("required")>]
        member inline _.Required(props, value: bool) = Prop("required", value :> obj) :: props

    type ButtonElement() =
        inherit InteractiveFormElement "button"

        [<CustomOperation("type'")>]
        member inline _.Type(props, value: ButtonType) =
            Prop("type", buttonTypeToString value :> obj) :: props

        [<CustomOperation("value")>]
        member inline _.Value(props, value: string) = Prop("value", value :> obj) :: props

    /// Media element builder with common media properties
    type MediaElementBuilder(tag: string) =
        inherit HtmlElementBuilder(tag)

        /// Source URL for media content
        [<CustomOperation("src")>]
        member inline _.Src(props, value: string) = Prop("src", value :> obj) :: props

        /// Whether media should play automatically
        [<CustomOperation("autoPlay")>]
        member inline _.AutoPlay(props, value: bool) = Prop("autoPlay", value :> obj) :: props

        /// Whether media controls should be displayed
        [<CustomOperation("controls")>]
        member inline _.Controls(props, value: bool) = Prop("controls", value :> obj) :: props

        /// Whether media should loop
        [<CustomOperation("loop")>]
        member inline _.Loop(props, value: bool) = Prop("loop", value :> obj) :: props

        /// Whether media should be muted initially
        [<CustomOperation("muted")>]
        member inline _.Muted(props, value: bool) = Prop("muted", value :> obj) :: props

        /// Media preload strategy
        [<CustomOperation("preload")>]
        member inline _.Preload(props, value: PreloadStrategy) =
            Prop("preload", preloadStrategyToString value :> obj) :: props

    type InputElementBuilder() =
        inherit InteractiveFormElement("input")

        /// Input type
        [<CustomOperation("type'")>]
        member inline _.Type(props, value: InputType) =
            Prop("type", value.ToString :> obj) :: props

        /// Input mode
        [<CustomOperation("inputMode")>]
        member inline _.InputMode(props, value: InputMode) =
            Prop("inputMode", inputModeToString value :> obj) :: props

        /// Pattern for validation
        [<CustomOperation("pattern")>]
        member inline _.Pattern(props, value: string) = Prop("pattern", value :> obj) :: props

        /// Maximum value
        [<CustomOperation("max")>]
        member inline _.Max(props, value: string) = Prop("max", value :> obj) :: props

        /// Minimum value
        [<CustomOperation("min")>]
        member inline _.Min(props, value: string) = Prop("min", value :> obj) :: props

        /// Step value
        [<CustomOperation("step")>]
        member inline _.Step(props, value: string) = Prop("step", value :> obj) :: props

    type SelectElementBuilder() =
        inherit InteractiveFormElement("select")

        /// Whether multiple selection is allowed
        [<CustomOperation("multiple")>]
        member inline _.Multiple(props, value: bool) = Prop("multiple", value :> obj) :: props

        /// Size of the visible options
        [<CustomOperation("size")>]
        member inline _.Size(props, value: int) = Prop("size", value :> obj) :: props

    type TextAreaElementBuilder() =
        inherit InteractiveFormElement("textarea")

        /// Number of rows
        [<CustomOperation("rows")>]
        member inline _.Rows(props, value: int) = Prop("rows", value :> obj) :: props

        /// Number of columns
        [<CustomOperation("cols")>]
        member inline _.Cols(props, value: int) = Prop("cols", value :> obj) :: props

        /// Whether to wrap text
        [<CustomOperation("wrap")>]
        member inline _.Wrap(props, value: WrapMode) =
            Prop("wrap", wrapModeToString value :> obj) :: props

    type TableElementBuilder() =
        inherit HtmlElementBuilder("table")

        /// Border width
        [<CustomOperation("border")>]
        member inline _.Border(props, value: int) = Prop("border", value :> obj) :: props

        /// Cell spacing
        [<CustomOperation("cellSpacing")>]
        member inline _.CellSpacing(props, value: int) =
            Prop("cellSpacing", value :> obj) :: props

        /// Cell padding
        [<CustomOperation("cellPadding")>]
        member inline _.CellPadding(props, value: int) =
            Prop("cellPadding", value :> obj) :: props

    type MetaElementBuilder() =
        inherit HtmlElementBuilder("meta")

        /// Name of the metadata
        [<CustomOperation("name")>]
        member inline _.Name(props, value: string) = Prop("name", value :> obj) :: props

        /// Content of the metadata
        [<CustomOperation("content")>]
        member inline _.Content(props, value: string) = Prop("content", value :> obj) :: props

        /// Character encoding
        [<CustomOperation("charset")>]
        member inline _.Charset(props, value: string) = Prop("charset", value :> obj) :: props

        /// HTTP-equiv value
        [<CustomOperation("httpEquiv")>]
        member inline _.HttpEquiv(props, value: HttpEquiv) =
            Prop("http-equiv", httpEquivToString value :> obj) :: props

    type LinkElementBuilder() =
        inherit HtmlElementBuilder("link")

        /// Relationship between current document and linked resource
        [<CustomOperation("rel")>]
        member inline _.Rel(props, value: LinkRelationType) =
            Prop("rel", linkRelationTypeToString value :> obj) :: props

        /// Media type the resource applies to
        [<CustomOperation("media")>]
        member inline _.Media(props, value: string) = Prop("media", value :> obj) :: props

        /// URL of the linked resource
        [<CustomOperation("href")>]
        member inline _.Href(props, value: string) = Prop("href", value :> obj) :: props

    type DetailsElementBuilder() =
        inherit HtmlElementBuilder("details")

        /// Whether the details are visible
        [<CustomOperation("open")>]
        member inline _.Open(props, value: bool) = Prop("open", value :> obj) :: props

    type DialogElementBuilder() =
        inherit HtmlElementBuilder("dialog")

        /// Whether the dialog is visible
        [<CustomOperation("open")>]
        member inline _.Open(props, value: bool) = Prop("open", value :> obj) :: props

        /// Return value when dialog is closed
        [<CustomOperation("returnValue")>]
        member inline _.ReturnValue(props, value: string) =
            Prop("returnValue", value :> obj) :: props

    type TableCellElementBuilder(tag: string) =
        inherit HtmlElementBuilder(tag)

        /// Number of columns this cell should span
        [<CustomOperation("colSpan")>]
        member inline _.ColSpan(props, value: int) = Prop("colSpan", value :> obj) :: props

        /// Number of rows this cell should span
        [<CustomOperation("rowSpan")>]
        member inline _.RowSpan(props, value: int) = Prop("rowSpan", value :> obj) :: props

        /// Defines the cells that the header cell relates to
        [<CustomOperation("scope")>]
        member inline _.Scope(props, value: TableCellScope) =
            Prop("scope", tableCellScopeToString value :> obj) :: props

    type ImageElementBuilder() =
        inherit HtmlElementBuilder("img")

        /// Alternative text description
        [<CustomOperation("alt")>]
        member inline _.Alt(props, value: string) = Prop("alt", value :> obj) :: props

        /// Image loading strategy
        [<CustomOperation("loading")>]
        member inline _.Loading(props, value: LoadingStrategy) =
            Prop("loading", loadingStrategyToString value :> obj) :: props

        /// Image decoding hint
        [<CustomOperation("decoding")>]
        member inline _.Decoding(props, value: ImageDecoding) =
            Prop("decoding", imageDecodingToString value :> obj) :: props

    type ProgressElementBuilder() =
        inherit HtmlElementBuilder("progress")

        /// Current value
        [<CustomOperation("value")>]
        member inline _.Value(props, value: float) = Prop("value", value :> obj) :: props

        /// Maximum value
        [<CustomOperation("max")>]
        member inline _.Max(props, value: float) = Prop("max", value :> obj) :: props

    type MeterElementBuilder() =
        inherit HtmlElementBuilder("meter")

        /// Current value
        [<CustomOperation("value")>]
        member inline _.Value(props, value: float) = Prop("value", value :> obj) :: props

        /// Minimum value
        [<CustomOperation("min")>]
        member inline _.Min(props, value: float) = Prop("min", value :> obj) :: props

        /// Maximum value
        [<CustomOperation("max")>]
        member inline _.Max(props, value: float) = Prop("max", value :> obj) :: props

        /// Low value range
        [<CustomOperation("low")>]
        member inline _.Low(props, value: float) = Prop("low", value :> obj) :: props

        /// High value range
        [<CustomOperation("high")>]
        member inline _.High(props, value: float) = Prop("high", value :> obj) :: props

        /// Optimal value
        [<CustomOperation("optimum")>]
        member inline _.Optimum(props, value: float) = Prop("optimum", value :> obj) :: props

    type OptionElementBuilder() =
        inherit InteractiveFormElement("option")

        /// Whether the option is selected by default
        [<CustomOperation("selected")>]
        member inline _.Selected(props, value: bool) = Prop("selected", value :> obj) :: props

        /// The value to be submitted with the form
        [<CustomOperation("value")>]
        member inline _.Value(props, value: string) = Prop("value", value :> obj) :: props

    type CanvasElementBuilder() =
        inherit HtmlElementBuilder("canvas")

        /// Context type for canvas
        [<CustomOperation("contextType")>]
        member inline _.ContextType(props, value: CanvasContextType) =
            Prop("contextType", canvasContextTypeToString value :> obj) :: props

    type DataListElementBuilder() =
        inherit HtmlElementBuilder("datalist")

    type OutputElementBuilder() =
        inherit HtmlElementBuilder("output")

        /// Form elements that contribute to the output's value
        [<CustomOperation("for")>]
        member inline _.For(props, value: string) = Prop("htmlFor", value :> obj) :: props

    type RubyBaseBuilder() =
        inherit HtmlElementBuilder("rb")

    type RubyTextBuilder() =
        inherit HtmlElementBuilder("rt")

    type RubyParenthesisBuilder() =
        inherit HtmlElementBuilder("rp")

    type TemplateElementBuilder() =
        inherit HtmlElementBuilder("template")
