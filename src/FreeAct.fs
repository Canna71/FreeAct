namespace FreeAct

open Fable.React
open Browser.Types
open Fable.Core.JS // for console.log
open FreeAct.Interop

// Core types
type HtmlProp = string * obj
// TODO: refactor builder to use list of ElementProperty

type ChildElement =
    | Element of ReactElement
    | ElementList of ReactElement list

type ElementProperty =
    | Prop of HtmlProp
    | Child of ChildElement

// Element builder using method chaining for props
type ElementBuilder(tag: string) =

    // member inline _.Yield(prop) = HtmlProp prop

    member inline _.Yield(prop: HtmlProp) = Prop prop
    // member inline _.Yield(prop: ElementProperty) =
    //     [prop]

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

    [<CustomOperation("text")>]
    member inline x.Text(props, value: string) = Child(Element(unbox value)) :: props

    /// Used to define a custom property on an element
    [<CustomOperation("prop")>]
    member inline _.Prop(props, propName: string, propValue: obj) =
        Prop(propName, propValue) :: props

    /// Used to define a custom property on an element
    [<CustomOperation("prop")>]
    member inline _.Prop(props, prop: HtmlProp) = Prop prop :: props

// Derived builder with children operation
type ElementWithChildrenBuilder(tag: string) =
    inherit ElementBuilder(tag)

    member inline _.Yield(reactElement: ReactElement) = [ Child(Element reactElement) ]

    member inline _.Yield(reactElementList: ReactElement list) =
        [ Child(ElementList reactElementList) ]

    /// <summary>
    /// Adds children elements to the current element. Note that this is provided
    /// for static lists. For dynamic lists, add them directly to the element
    /// </summary>
    /// <param name="elements">The children elements to spread</param>
    /// <returns>The combined properties and children elements</returns>
    [<CustomOperation("children")>]
    member _.Children(props, elements: ReactElement list) =
        // Child(ElementList elements) :: props
        (elements |> List.map (fun el -> Child(Element el))) @ props
