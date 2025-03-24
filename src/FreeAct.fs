namespace FreeAct

[<AutoOpen>]
module FreeAct =

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

    type EventHandler = Event -> unit

    // Element builder using method chaining for props
    type ElementBuilder(tag: string) =

        // member inline _.Yield(prop) = HtmlProp prop

        member inline _.Yield(prop: HtmlProp) = Prop prop
        // member inline _.Yield(prop: ElementProperty) =
        //     [prop]

        member inline _.Yield(()) : ElementProperty list = []

        member inline _.Combine(prop: ElementProperty, props: ElementProperty list) = prop :: props

        member inline _.Combine(props: ElementProperty list, prop: ElementProperty) =
            props @ [ prop ]

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
