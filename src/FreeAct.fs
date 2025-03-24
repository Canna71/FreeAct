module FreeAct.Elements

open Fable.Core
open Fable.Core.JsInterop
open Fable.React
open Browser.Types
open Fable.Core.JS

type IReactProperty = interface end

// Represents a generic CSSUnit
type ICSSUnit = interface end

[<Import("createElement", "react")>]
[<Emit "$0.apply(null, [$1, $2, ...$3])">]
let inline reactElementApply
    (name: string)
    (props: 'a)
    (nested: #seq<ReactElement>)
    : ReactElement
    =
    jsNative

let inline createElement
    name
    (properties: IReactProperty list)
    (children: ReactElement list)
    : ReactElement
    =
    let props: obj = createObj !!properties
    console.log ("Creating element", props)
    // let nested = emitJsExpr (props) "$0.nested || []"
    emitJsStatement (props) "delete $0.nested"
    reactElementApply name props children

// let div (properties: IReactProperty list) (nested: ReactElement list) =
//     reactElementApply "div" properties nested

let d = 2

// Core types
type HtmlProp = string * obj
// TODO: refactor builder to use list of ElementProperty

type ChildElement =
    | Element of ReactElement
    | ElementList of ReactElement list

type ElementProperty =
    | Prop of HtmlProp
    | Child of ChildElement

// type Element =
//     {
//         tag: string
//         props: Prop list
//         children: ChildElement list
//     }

// let toElement (tag: string, props: Prop list, children: ChildElement list) =
//     {
//         tag = tag
//         props = props
//         children = children
//     }

type EventHandler = Event -> unit

// Add this helper function to convert ReactElement lists to fragments automatically
// let inline asReactElement (elements: ReactElement list) : ReactElement = unbox elements

// Convert custom Element type to React elements
// let rec toReactElement (element: Element) : ReactElement =
//     // Convert props list to IReactProperty list
//     let reactProps =
//         element.props
//         // |> List.map (fun (name, value) -> name ==> value)
//         |> List.map (fun prop -> unbox<IReactProperty> prop)

//     let children =
//         element.children
//         |> List.map (fun child ->
//             match child with
//             | Element el -> el
//             | ElementList els -> unbox els
//         )

// createElement element.tag reactProps children

// Element builder using method chaining for props
type ElementBuilder(tag: string) =

    // member inline _.Yield(prop) = HtmlProp prop

    member inline _.Yield(prop: HtmlProp) = Prop prop
    // member inline _.Yield(prop: ElementProperty) =
    //     [prop]

    member inline _.Yield(reactElement: ReactElement) = [ Child(Element reactElement) ]

    member inline _.Yield(reactElementList: ReactElement list) =
        [ Child(ElementList reactElementList) ]

    member inline _.Yield(()) : ElementProperty list = []

    member inline _.Combine(prop: ElementProperty, props: ElementProperty list) = prop :: props
    member inline _.Combine(props: ElementProperty list, prop: ElementProperty) = props @ [ prop ]
    member inline _.Combine(a: ElementProperty list, b: ElementProperty list) = a @ b

    member inline x.For(prop: ElementProperty, f: unit -> ElementProperty list) =
        x.Combine(prop, f ())

    member inline x.For(prop: ElementProperty list, f: unit -> ElementProperty) =
        x.Combine(f (), prop)

    member inline x.For(props: ElementProperty list, f: unit -> ElementProperty list) =
        x.Combine(f (), props)

    member inline _.Delay(f: unit -> ElementProperty) = [ f () ]

    member inline _.Delay(f: unit -> ElementProperty list) = f ()

    //  member inline _.Delay(f) = f()

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

    // key property
    [<CustomOperation("key")>]
    member inline x.Key(props, value: string) = Prop("key", value :> obj) :: props

    /// Used to define a custom property on an element
    [<CustomOperation("prop")>]
    member inline _.Prop(props, prop: HtmlProp) = Prop prop :: props

    /// Used to define a custom property on an element
    [<CustomOperation("prop")>]
    member inline _.Prop(props, propName: string, propValue: obj) =
        Prop(propName, propValue) :: props

    /// onClick event
    [<CustomOperation("onClick")>]
    member inline _.OnClick(props, handler) =
        Prop("onClick", handler :> obj) :: props

    // onchange
    [<CustomOperation("onChange")>]
    member inline _.OnChange(props, handler: EventHandler) =
        Prop("onChange", handler :> obj) :: props

    [<CustomOperation("text")>]
    member inline x.Text(props, value: string) = Child(Element(unbox value)) :: props

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

    // zero operation
    [<CustomOperation("zero")>]
    member inline _.Zero() = []

    // inline operation

    member inline x.Run(prop: ElementProperty) = x.Run [ prop ]

    member x.Run(props: ElementProperty list) =
        let properties =
            List.choose
                (fun prop ->
                    match prop with
                    | Prop p -> Some p
                    | _ -> None
                )
                props

        let children =
            List.choose
                (fun prop ->
                    match prop with
                    | Child c ->
                        match c with
                        | Element e -> Some e
                        | ElementList els -> Some(unbox els)
                    | _ -> None
                )
                props

        createElement tag (unbox properties) (unbox children)

// Derived builder with children operation
type ElementWithChildrenBuilder(tag: string) =
    inherit ElementBuilder(tag)

    [<CustomOperation("children")>]
    member this.Children(props, elements: ReactElement list) = Child(ElementList elements) :: props

// HTML element builders

/// Defines a HTML div element
let div = ElementWithChildrenBuilder "div"
let h1 = ElementWithChildrenBuilder "h1"
let button = ElementWithChildrenBuilder("button")
let p = ElementWithChildrenBuilder("p")
let section = ElementWithChildrenBuilder("section")
let span = ElementWithChildrenBuilder("span")
let i = ElementWithChildrenBuilder("i")

let label = ElementWithChildrenBuilder("label")
let img = ElementBuilder("img")

let input = ElementBuilder("input")

let none: ReactElement = null
