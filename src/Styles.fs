namespace FreeAct

open Fable.Core.JsInterop
open FreeAct
open Fable.Core.JS

[<AutoOpen>]
module Styles =
    [<RequireQualifiedAccess>]
    type Unit =
        | Px
        | Em
        | Rem
        | Vw
        | Vh
        | Vmin

    let unitToString =
        function
        | Unit.Px -> "px"
        | Unit.Em -> "em"
        | Unit.Rem -> "rem"
        | Unit.Vw -> "vw"
        | Unit.Vh -> "vh"
        | Unit.Vmin -> "vmin"

    [<RequireQualifiedAccess>]
    type Length =
        | Units of int * Unit
        | Percent of int
        | Auto
        | MaxContent
        | MinContent
        | FitContent
        | FitContentPercent of int
        | Stretch

    let lengthToString =
        function
        | Length.Units(n, u) -> sprintf "%d%s" n (unitToString u)
        | Length.Percent(p) -> sprintf "%d%%" p
        | Length.Auto -> "auto"
        | Length.MaxContent -> "max-content"
        | Length.MinContent -> "min-content"
        | Length.FitContent -> "fit-content"
        | Length.FitContentPercent(p) -> sprintf "fit-content(%d%%)" p
        | Length.Stretch -> "stretch"

    [<RequireQualifiedAccess>]
    type TextAlign =
        | Left
        | Right
        | Center
        | Justify
        | MatchParent
        | Start
        | End
        | Inherit
        | Initial
        | Unset
        | Revert
        | RevertLayer

    let textAlignToString =
        function
        | TextAlign.Left -> "left"
        | TextAlign.Right -> "right"
        | TextAlign.Center -> "center"
        | TextAlign.Justify -> "justify"
        | TextAlign.MatchParent -> "match-parent"
        | TextAlign.Start -> "start"
        | TextAlign.End -> "end"
        | TextAlign.Inherit -> "inherit"
        | TextAlign.Initial -> "initial"
        | TextAlign.Unset -> "unset"
        | TextAlign.Revert -> "revert"
        | TextAlign.RevertLayer -> "revert-layer"

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

        member inline _.Zero() = []
        // Color
        [<CustomOperation("color")>]
        member inline _.Color(props, c: string) = ("color", c :> obj) :: props

        // Background color
        [<CustomOperation("backgroundColor")>]
        member inline _.BackgroundColor(props, c: string) = ("backgroundColor", c :> obj) :: props

        // Padding
        [<CustomOperation("padding")>]
        member inline _.Padding(props, p: string) = ("padding", p :> obj) :: props

        // Padding
        [<CustomOperation("padding")>]
        member inline _.Padding(props, n: int) =
            ("padding", sprintf "%dpx" n :> obj) :: props

        // TODO: Add support for other syntaxes
        [<CustomOperation("margin")>]
        member inline _.Margin(props, m: int) =
            ("margin", lengthToString (Length.Units(m, Unit.Px)) :> obj) :: props

        [<CustomOperation("margin")>]
        member inline _.Margin(props, m: Length) =
            ("margin", lengthToString m :> obj) :: props

        [<CustomOperation("margin")>]
        member inline _.Margin(props, m: string) = ("margin", m :> obj) :: props

        [<CustomOperation("textAlign")>]
        member inline _.TextAlign(props, align: TextAlign) =
            ("textAlign", textAlignToString align :> obj) :: props

        // width
        [<CustomOperation("width")>]
        member inline _.Width(props, w: Length) =
            ("width", lengthToString w :> obj) :: props

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
