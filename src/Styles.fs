namespace FreeAct

open Fable.Core.JsInterop
open FreeAct
open Fable.Core.JS

module Styles =

    [<RequireQualifiedAccess>]
    type Color =
        static member inline rgba(r: int, g: int, b: int, a: float) = $"rgba({r}, {g}, {b}, {a})"
        static member inline rgb(r: int, g: int, b: int) = $"rgb({r}, {g}, {b})"
        static member inline hex(hex: string) = hex

        static member inline black = "black"
        static member inline white = "white"
        static member inline red = "red"
        static member inline green = "green"
        static member inline blue = "blue"
        static member inline yellow = "yellow"
        static member inline pink = "pink"
        static member inline purple = "purple"
        static member inline orange = "orange"
        static member inline crimson = "crimson"

        static member inline transparent = "transparent"

    // Style builder that combines property and value
    type StyleBuilder() =
        member inline _.Yield(_) = []

        // Color
        [<CustomOperation("color")>]
        member inline _.Color(props, c: string) = ("color", c :> obj) :: props

        // Background color
        [<CustomOperation("backgroundColor")>]
        member inline _.BackgroundColor(props, c: string) = ("backgroundColor", c :> obj) :: props

        // Padding
        [<CustomOperation("padding")>]
        member inline _.Padding(props, p: string) = ("padding", p :> obj) :: props

        [<CustomOperation("margin")>]
        member inline _.Margin(props, m: string) = ("margin", m :> obj) :: props

        [<CustomOperation("textAlign")>]
        member inline _.TextAlign(props, m: string) = ("textAlign", m :> obj) :: props

        // width
        [<CustomOperation("width")>]
        member inline _.Width(props, w: string) = ("width", w :> obj) :: props

        // Support combinators
        [<CustomOperation("combine")>]
        member inline _.Combine(props1, props2) = props1 @ props2

        // member inline _.Delay(f) = f()

        // Convert to IReactProperty with "style" prop name
        member inline _.Run(props) : HtmlProp =
            let styleObj = createObj props
            let ret = unbox<HtmlProp> ("style", styleObj)
            ret

    let style = StyleBuilder()
