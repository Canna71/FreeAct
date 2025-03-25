namespace FreeAct

open FreeAct
open FreeAct.Interop

[<AutoOpen>]
module CoreHtmlElement =

    type CoreHtmlElementBuilder(tag: string) =
        inherit ElementWithChildrenBuilder(tag)

        /// id
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

        /// title
        [<CustomOperation("title")>]
        member inline _.Title(props, value: string) = Prop("title", value :> obj) :: props

        /// lang
        [<CustomOperation("lang")>]
        member inline _.Lang(props, value: string) = Prop("lang", value :> obj) :: props

        /// dir
        [<CustomOperation("dir")>]
        member inline _.Dir(props, value: Direction) =
            Prop("dir", directionToString value :> obj) :: props

        /// tabIndex
        [<CustomOperation("tabIndex")>]
        member inline _.TabIndex(props, value: int) = Prop("tabIndex", value :> obj) :: props

        /// hidden
        [<CustomOperation("hidden")>]
        member inline _.Hidden(props, value: bool) = Prop("hidden", value :> obj) :: props

        /// accessKey
        [<CustomOperation("accessKey")>]
        member inline _.AccessKey(props, value: string) =
            Prop("accessKey", value :> obj) :: props

        /// contentEditable
        [<CustomOperation("contentEditable")>]
        member inline _.ContentEditable(props, value: bool) =
            Prop("contentEditable", value :> obj) :: props

        /// contentEditable
        [<CustomOperation("contentEditable")>]
        member inline _.ContentEditable(props, value: PlaintextOnly) =
            Prop("contentEditable", "plaintext-only" :> obj) :: props

        /// draggable
        /// <summary>
        /// Used to define whether an element is draggable or not
        /// </summary>
        [<CustomOperation("draggable")>]
        member inline _.Draggable(props, value: bool) =
            Prop("draggable", value :> obj) :: props

        /// spellCheck
        /// <summary>
        /// Used to define whether an element is spell checked or not
        /// </summary>
        [<CustomOperation("spellCheck")>]
        member inline _.SpellCheck(props, value: bool) =
            Prop("spellCheck", value :> obj) :: props

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

        /// inert
        /// <summary>
        /// Used to define whether an element is inert or not
        /// </summary>
        [<CustomOperation("inert")>]
        member inline _.Inert(props, value: bool) = Prop("inert", value :> obj) :: props

        // slot
        /// <summary>
        /// Used to define the slot of an element
        /// </summary>
        [<CustomOperation("slot")>]
        member inline _.Slot(props, value: string) = Prop("slot", value :> obj) :: props

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

        // data-* attributes
        /// <summary>
        /// Used to define a custom data attribute on an element
        /// </summary>
        [<CustomOperation("data")>]
        member inline _.Data(props, dataName: string, dataValue: string) =
            Prop(sprintf "data-%s" dataName, dataValue :> obj) :: props

        // aria-* attributes
        /// <summary>
        /// Used to define a custom aria attribute on an element
        /// </summary>
        [<CustomOperation("aria")>]
        member inline _.Aria(props, ariaName: string, ariaValue: string) =
            Prop(sprintf "aria-%s" ariaName, ariaValue :> obj) :: props

        // TODO: The following should go in a derived type

        // key property
        [<CustomOperation("key")>]
        member inline x.Key(props, value: string) = Prop("key", value :> obj) :: props

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
