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
    let props = createObj !!properties
    // let nested = emitJsExpr (props) "$0.nested || []"
    emitJsStatement (props) "delete $0.nested"
    reactElementApply name props children

// let div (properties: IReactProperty list) (nested: ReactElement list) =
//     reactElementApply "div" properties nested

let d = 2

// Core types
type Prop = string * obj
// TODO: use record type for Element

type ChildElement =
    | Element of ReactElement
    | ElementList of ReactElement list

type Element =
    {
        tag: string
        props: Prop list
        children: ChildElement list
    }

let toElement (tag: string, props: Prop list, children: ChildElement list) =
    {
        tag = tag
        props = props
        children = children
    }

type EventHandler = Event -> unit

// Add this helper function to convert ReactElement lists to fragments automatically
let inline asReactElement (elements: ReactElement list) : ReactElement = unbox elements

// Convert custom Element type to React elements
let rec toReactElement (element: Element) : ReactElement =
    // Convert props list to IReactProperty list
    let reactProps =
        element.props
        // |> List.map (fun (name, value) -> name ==> value)
        |> List.map (fun prop -> unbox<IReactProperty> prop)

    let children =
        element.children
        |> List.map (fun child ->
            match child with
            | Element el -> el
            | ElementList els -> unbox els
        )

    createElement element.tag reactProps children

// Element builder using method chaining for props
type ElementBuilder(tag: string) =

    member inline _.Yield(prop: Prop) = prop

    member inline _.Yield(reactElement: ReactElement) =
        toElement (tag, [], [ Element reactElement ])

    member inline _.Yield(reactElementList: ReactElement list) =
        toElement (tag, [], [ ElementList reactElementList ])

    member inline _.Yield(()) = toElement (tag, [], [])

    member inline _.Combine(element, prop: Prop) =
        { element with props = prop :: element.props }

    member inline _.Combine(prop: Prop, element) =
        { element with props = prop :: element.props }

    member inline _.Combine(a: Element, b: Element) =
        { a with children = a.children @ b.children; props = a.props @ b.props }

    member inline x.For(prop: Prop, f: unit -> Element) = x.Combine(prop, f ())

    member inline x.For(element: Element, f: unit -> Prop) = x.Combine(f (), element)

    member inline x.For(element: Element, f: unit -> Element) = x.Combine(f (), element)

    member inline _.Delay(f: unit -> Prop) = toElement (tag, [ f () ], [])

    member inline _.Delay(f: unit -> Element) = f ()

    //  member inline _.Delay(f) = f()

    /// <summary>
    /// Use it to assign a class name to an element
    /// </summary>
    [<CustomOperation("className")>]
    member inline _.ClassName(element: Element, value: string) =
        { element with props = ("className", value :> obj) :: element.props }

    /// className as seq of strings
    [<CustomOperation("className")>]
    member inline _.ClassName(element, values: string seq) =
        { element with props = ("className", String.concat " " values :> obj) :: element.props }

    // key property
    [<CustomOperation("key")>]
    member inline _.Key(element, value: string) =
        { element with props = ("key", value :> obj) :: element.props }

    /// Used to define a custom property on an element
    [<CustomOperation("prop")>]
    member inline _.Prop(element, prop: Prop) =
        { element with props = prop :: element.props }
    // toElement(tag, prop :: props, children)

    /// Used to define a custom property on an element
    [<CustomOperation("prop")>]
    member inline _.Prop(element, propName: string, propValue: obj) =
        { element with props = (propName, propValue) :: element.props }

    /// onClick event
    [<CustomOperation("onClick")>]
    member inline _.OnClick(element, handler) =
        { element with props = ("onClick", handler :> obj) :: element.props }

    // onchange
    [<CustomOperation("onChange")>]
    member inline _.OnChange(element, handler: EventHandler) =
        { element with props = ("onChange", handler :> obj) :: element.props }

    [<CustomOperation("text")>]
    member inline _.Text(element, value: string) =
        { element with children = ElementList [ unbox value ] :: element.children }
    // toElement(tag, props,  [unbox value])

    // src
    [<CustomOperation("src")>]
    member inline _.Src(element, value: string) =
        { element with props = ("src", value :> obj) :: element.props }

    // type as InputType
    [<CustomOperation("type'")>]
    member inline _.Type(element, value: InputType) =
        { element with props = ("type", value.ToString() :> obj) :: element.props }

    // height as int
    [<CustomOperation("height")>]
    member inline _.Height(element, value: int) =
        { element with props = ("height", value :> obj) :: element.props }

    /// height as float
    [<CustomOperation("height")>]
    member inline _.Height(element, value: float) =
        { element with props = ("height", value :> obj) :: element.props }

    // height as ICSSUnit
    [<CustomOperation("height")>]
    member inline _.Height(element, value: ICSSUnit) =
        { element with props = ("height", value :> obj) :: element.props }

    // width as int
    [<CustomOperation("width")>]
    member inline _.Width(element, value: int) =
        { element with props = ("width", value :> obj) :: element.props }

    // width as float
    [<CustomOperation("width")>]
    member inline _.Width(element, value: float) =
        { element with props = ("width", value :> obj) :: element.props }

    // width as ICSSUnit
    [<CustomOperation("width")>]
    member inline _.Width(element, value: ICSSUnit) =
        { element with props = ("width", value :> obj) :: element.props }

    // placeholder
    [<CustomOperation("placeholder")>]
    member inline _.Placeholder(element, value: string) =
        { element with props = ("placeholder", value :> obj) :: element.props }

    // value
    [<CustomOperation("value")>]
    member inline _.Value(element, value: obj) =
        { element with props = ("value", value) :: element.props }

    // defaultValue
    [<CustomOperation("defaultValue")>]
    member inline _.DefaultValue(element, value: obj) =
        { element with props = ("defaultValue", value) :: element.props }

    // zero operation
    [<CustomOperation("zero")>]
    member inline _.Zero() = toElement (tag, [], [])

    // Add this Return member inline to automatically convert to ReactElement
    member inline _.Run(element: Element) = toReactElement element

// Derived builder with children operation
type ElementWithChildrenBuilder(tag: string) =
    inherit ElementBuilder(tag)

    [<CustomOperation("children")>]
    member _.Children(element: Element, elements: ReactElement list) =
        { element with children = ElementList elements :: element.children }

// [<CustomOperation("children")>]
// member _.Children((tag, props, children), elements: ReactElement list list) =
//     toElement(tag, props, children @ unbox elements)

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
