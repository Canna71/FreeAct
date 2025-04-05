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
    type Display =
        | None
        | Block
        | Inline
        | InlineBlock
        | Flex
        | Grid
        | Table

    let displayToString =
        function
        | Display.None -> "none"
        | Display.Block -> "block"
        | Display.Inline -> "inline"
        | Display.InlineBlock -> "inline-block"
        | Display.Flex -> "flex"
        | Display.Grid -> "grid"
        | Display.Table -> "table"

    [<RequireQualifiedAccess>]
    type Position =
        | Static
        | Relative
        | Absolute
        | Fixed
        | Sticky

    let positionToString =
        function
        | Position.Static -> "static"
        | Position.Relative -> "relative"
        | Position.Absolute -> "absolute"
        | Position.Fixed -> "fixed"
        | Position.Sticky -> "sticky"

    [<RequireQualifiedAccess>]
    type FlexDirection =
        | Row
        | RowReverse
        | Column
        | ColumnReverse

    let flexDirectionToString =
        function
        | FlexDirection.Row -> "row"
        | FlexDirection.RowReverse -> "row-reverse"
        | FlexDirection.Column -> "column"
        | FlexDirection.ColumnReverse -> "column-reverse"

    [<RequireQualifiedAccess>]
    type FlexWrap =
        | NoWrap
        | Wrap
        | WrapReverse

    let flexWrapToString =
        function
        | FlexWrap.NoWrap -> "nowrap"
        | FlexWrap.Wrap -> "wrap"
        | FlexWrap.WrapReverse -> "wrap-reverse"

    [<RequireQualifiedAccess>]
    type JustifyContent =
        | FlexStart
        | FlexEnd
        | Center
        | SpaceBetween
        | SpaceAround
        | SpaceEvenly

    let justifyContentToString =
        function
        | JustifyContent.FlexStart -> "flex-start"
        | JustifyContent.FlexEnd -> "flex-end"
        | JustifyContent.Center -> "center"
        | JustifyContent.SpaceBetween -> "space-between"
        | JustifyContent.SpaceAround -> "space-around"
        | JustifyContent.SpaceEvenly -> "space-evenly"

    [<RequireQualifiedAccess>]
    type AlignItems =
        | FlexStart
        | FlexEnd
        | Center
        | Baseline
        | Stretch

    let alignItemsToString =
        function
        | AlignItems.FlexStart -> "flex-start"
        | AlignItems.FlexEnd -> "flex-end"
        | AlignItems.Center -> "center"
        | AlignItems.Baseline -> "baseline"
        | AlignItems.Stretch -> "stretch"

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

    [<RequireQualifiedAccess>]
    type Overflow =
        | Visible
        | Hidden
        | Scroll
        | Auto

    let overflowToString =
        function
        | Overflow.Visible -> "visible"
        | Overflow.Hidden -> "hidden"
        | Overflow.Scroll -> "scroll"
        | Overflow.Auto -> "auto"

    [<RequireQualifiedAccess>]
    type Cursor =
        | Pointer
        | Default
        | Text
        | Move
        | NotAllowed
        | Grab
        | Grabbing
        | ZoomIn
        | ZoomOut

    let cursorToString =
        function
        | Cursor.Pointer -> "pointer"
        | Cursor.Default -> "default"
        | Cursor.Text -> "text"
        | Cursor.Move -> "move"
        | Cursor.NotAllowed -> "not-allowed"
        | Cursor.Grab -> "grab"
        | Cursor.Grabbing -> "grabbing"
        | Cursor.ZoomIn -> "zoom-in"
        | Cursor.ZoomOut -> "zoom-out"

    [<RequireQualifiedAccess>]
    type TextTransform =
        | None
        | Capitalize
        | Uppercase
        | Lowercase

    let textTransformToString =
        function
        | TextTransform.None -> "none"
        | TextTransform.Capitalize -> "capitalize"
        | TextTransform.Uppercase -> "uppercase"
        | TextTransform.Lowercase -> "lowercase"

    [<RequireQualifiedAccess>]
    type BackgroundSize =
        | Cover
        | Contain
        | Auto
        | Custom of string

    let backgroundSizeToString =
        function
        | BackgroundSize.Cover -> "cover"
        | BackgroundSize.Contain -> "contain"
        | BackgroundSize.Auto -> "auto"
        | BackgroundSize.Custom s -> s

    [<RequireQualifiedAccess>]
    type BackgroundPosition =
        | Center
        | Top
        | Bottom
        | Left
        | Right
        | Custom of string

    let backgroundPositionToString =
        function
        | BackgroundPosition.Center -> "center"
        | BackgroundPosition.Top -> "top"
        | BackgroundPosition.Bottom -> "bottom"
        | BackgroundPosition.Left -> "left"
        | BackgroundPosition.Right -> "right"
        | BackgroundPosition.Custom s -> s

    [<RequireQualifiedAccess>]
    type Transform =
        | Scale of float
        | Translate of x: Length * y: Length
        | Rotate of degrees: int
        | Multiple of Transform list

    let rec transformToString =
        function
        | Transform.Scale s -> sprintf "scale(%f)" s
        | Transform.Translate(x, y) ->
            sprintf "translate(%s, %s)" (lengthToString x) (lengthToString y)
        | Transform.Rotate deg -> sprintf "rotate(%ddeg)" deg
        | Transform.Multiple transforms ->
            transforms |> List.map transformToString |> String.concat " "

    // Style builder that combines property and value
    type StyleBuilder() =
        member inline _.Yield(_) = []

        member inline _.Zero() = []
        // Color
        /// Sets the text color
        /// Example: color Color.red
        /// Example: color (Color.rgba(255, 0, 0, 0.5))
        [<CustomOperation("color")>]
        member inline _.Color(props, c: string) = ("color", c :> obj) :: props

        // Background color
        /// Sets the background color
        /// Example: backgroundColor Color.blue
        /// Example: backgroundColor (Color.hex "#ff0000")
        [<CustomOperation("backgroundColor")>]
        member inline _.BackgroundColor(props, c: string) = ("backgroundColor", c :> obj) :: props

        // Padding
        /// Sets padding on all sides
        /// Example: padding 10
        /// Example: padding "10px 20px"
        [<CustomOperation("padding")>]
        member inline _.Padding(props, p: string) = ("padding", p :> obj) :: props

        // Padding
        /// Sets padding using pixel units
        /// Example: padding 10  // -> padding: 10px
        [<CustomOperation("padding")>]
        member inline _.Padding(props, n: int) =
            ("padding", sprintf "%dpx" n :> obj) :: props

        // TODO: Add support for other syntaxes
        /// Sets margin on all sides
        /// Example: margin (Length.Units(10, Unit.Px))
        /// Example: margin Length.Auto
        [<CustomOperation("margin")>]
        member inline _.Margin(props, m: Length) =
            ("margin", lengthToString m :> obj) :: props

        [<CustomOperation("margin")>]
        member inline _.Margin(props, m: string) = ("margin", m :> obj) :: props

        /// Sets text alignment
        /// Example: textAlign TextAlign.Center
        /// Example: textAlign TextAlign.Justify
        [<CustomOperation("textAlign")>]
        member inline _.TextAlign(props, align: TextAlign) =
            ("textAlign", textAlignToString align :> obj) :: props

        // width
        /// Sets element width
        /// Example: width (Length.Units(200, Unit.Px))
        /// Example: width (Length.Percent 100)
        [<CustomOperation("width")>]
        member inline _.Width(props, w: Length) =
            ("width", lengthToString w :> obj) :: props

        // width
        [<CustomOperation("width")>]
        member inline _.Width(props, w: string) = ("width", w :> obj) :: props

        // Font properties
        /// Sets font size
        /// Example: fontSize (Length.Units(16, Unit.Px))
        /// Example: fontSize (Length.Units(1, Unit.Rem))
        [<CustomOperation("fontSize")>]
        member inline _.FontSize(props, size: Length) =
            ("fontSize", lengthToString size :> obj) :: props

        /// Sets font weight (100-900)
        /// Example: fontWeight 700  // bold
        /// Example: fontWeight 400  // normal
        [<CustomOperation("fontWeight")>]
        member inline _.FontWeight(props, weight: int) = ("fontWeight", weight :> obj) :: props

        [<CustomOperation("fontFamily")>]
        member inline _.FontFamily(props, family: string) = ("fontFamily", family :> obj) :: props

        // Display properties
        /// Sets the display property
        /// Example: display Display.Flex
        /// Example: display Display.Grid
        [<CustomOperation("display")>]
        member inline _.Display(props, value: Display) =
            ("display", displayToString value :> obj) :: props

        /// Sets positioning behavior
        /// Example: position Position.Absolute
        /// Example: position Position.Sticky
        [<CustomOperation("position")>]
        member inline _.Position(props, value: Position) =
            ("position", positionToString value :> obj) :: props

        [<CustomOperation("top")>]
        member inline _.Top(props, value: Length) =
            ("top", lengthToString value :> obj) :: props

        [<CustomOperation("left")>]
        member inline _.Left(props, value: Length) =
            ("left", lengthToString value :> obj) :: props

        [<CustomOperation("right")>]
        member inline _.Right(props, value: Length) =
            ("right", lengthToString value :> obj) :: props

        [<CustomOperation("bottom")>]
        member inline _.Bottom(props, value: Length) =
            ("bottom", lengthToString value :> obj) :: props

        // Border properties
        [<CustomOperation("border")>]
        member inline _.Border(props, value: string) = ("border", value :> obj) :: props

        [<CustomOperation("borderRadius")>]
        member inline _.BorderRadius(props, value: Length) =
            ("borderRadius", lengthToString value :> obj) :: props

        // Flex properties
        /// Sets flex container direction
        /// Example: flexDirection FlexDirection.Row
        /// Example: flexDirection FlexDirection.Column
        [<CustomOperation("flexDirection")>]
        member inline _.FlexDirection(props, value: FlexDirection) =
            ("flexDirection", flexDirectionToString value :> obj) :: props

        /// Sets flex wrapping behavior
        /// Example: flexWrap FlexWrap.Wrap
        /// Example: flexWrap FlexWrap.NoWrap
        [<CustomOperation("flexWrap")>]
        member inline _.FlexWrap(props, value: FlexWrap) =
            ("flexWrap", flexWrapToString value :> obj) :: props

        /// Sets flex content justification
        /// Example: justifyContent JustifyContent.SpaceBetween
        /// Example: justifyContent JustifyContent.Center
        [<CustomOperation("justifyContent")>]
        member inline _.JustifyContent(props, value: JustifyContent) =
            ("justifyContent", justifyContentToString value :> obj) :: props

        /// Sets flex items alignment
        /// Example: alignItems AlignItems.Center
        /// Example: alignItems AlignItems.Stretch
        [<CustomOperation("alignItems")>]
        member inline _.AlignItems(props, value: AlignItems) =
            ("alignItems", alignItemsToString value :> obj) :: props

        [<CustomOperation("flex")>]
        member inline _.Flex(props, value: string) = ("flex", value :> obj) :: props

        /// Sets the overflow behavior
        /// Example: overflow Overflow.Hidden
        [<CustomOperation("overflow")>]
        member inline _.Overflow(props, value: Overflow) =
            ("overflow", overflowToString value :> obj) :: props

        /// Sets overflow behavior for x axis
        /// Example: overflowX Overflow.Scroll
        [<CustomOperation("overflowX")>]
        member inline _.OverflowX(props, value: Overflow) =
            ("overflowX", overflowToString value :> obj) :: props

        /// Sets overflow behavior for y axis
        /// Example: overflowY Overflow.Auto
        [<CustomOperation("overflowY")>]
        member inline _.OverflowY(props, value: Overflow) =
            ("overflowY", overflowToString value :> obj) :: props

        /// Sets the cursor style
        /// Example: cursor Cursor.Pointer
        [<CustomOperation("cursor")>]
        member inline _.Cursor(props, value: Cursor) =
            ("cursor", cursorToString value :> obj) :: props

        /// Sets text transformation
        /// Example: textTransform TextTransform.Uppercase
        [<CustomOperation("textTransform")>]
        member inline _.TextTransform(props, value: TextTransform) =
            ("textTransform", textTransformToString value :> obj) :: props

        /// Sets opacity (0.0 to 1.0)
        /// Example: opacity 0.5
        [<CustomOperation("opacity")>]
        member inline _.Opacity(props, value: float) = ("opacity", value :> obj) :: props

        /// Sets z-index
        /// Example: zIndex 100
        [<CustomOperation("zIndex")>]
        member inline _.ZIndex(props, value: int) = ("zIndex", value :> obj) :: props

        /// Sets box shadow
        /// Example: boxShadow "0 2px 4px rgba(0,0,0,0.1)"
        [<CustomOperation("boxShadow")>]
        member inline _.BoxShadow(props, value: string) = ("boxShadow", value :> obj) :: props

        /// Sets text decoration
        /// Example: textDecoration "underline"
        [<CustomOperation("textDecoration")>]
        member inline _.TextDecoration(props, value: string) =
            ("textDecoration", value :> obj) :: props

        // Support combinators
        [<CustomOperation("combine")>]
        member inline _.Combine(props1, props2) = props1 @ props2

        // member inline _.Delay(f) = f()

        /// Sets background size
        /// Example: backgroundSize BackgroundSize.Cover
        [<CustomOperation("backgroundSize")>]
        member inline _.BackgroundSize(props, value: BackgroundSize) =
            ("backgroundSize", backgroundSizeToString value :> obj) :: props

        /// Sets background position
        /// Example: backgroundPosition BackgroundPosition.Center
        [<CustomOperation("backgroundPosition")>]
        member inline _.BackgroundPosition(props, value: BackgroundPosition) =
            ("backgroundPosition", backgroundPositionToString value :> obj) :: props

        /// Sets background image
        /// Example: backgroundImage "url('/images/bg.png')"
        [<CustomOperation("backgroundImage")>]
        member inline _.BackgroundImage(props, value: string) =
            ("backgroundImage", value :> obj) :: props

        /// Sets transition
        /// Example: transition "all 0.3s ease"
        [<CustomOperation("transition")>]
        member inline _.Transition(props, value: string) = ("transition", value :> obj) :: props

        /// Sets transform
        /// Example: transform (Transform.Scale 1.1)
        /// Example: transform (Transform.Multiple [Transform.Scale 1.1; Transform.Rotate 45])
        [<CustomOperation("transform")>]
        member inline _.Transform(props, value: Transform) =
            ("transform", transformToString value :> obj) :: props

        /// Sets grid template columns
        /// Example: gridTemplateColumns "1fr 1fr 1fr"
        [<CustomOperation("gridTemplateColumns")>]
        member inline _.GridTemplateColumns(props, value: string) =
            ("gridTemplateColumns", value :> obj) :: props

        /// Sets grid template rows
        /// Example: gridTemplateRows "auto 1fr auto"
        [<CustomOperation("gridTemplateRows")>]
        member inline _.GridTemplateRows(props, value: string) =
            ("gridTemplateRows", value :> obj) :: props

        /// Sets grid gap
        /// Example: gap (Length.Units(16, Unit.Px))
        [<CustomOperation("gap")>]
        member inline _.Gap(props, value: Length) =
            ("gap", lengthToString value :> obj) :: props

        /// Sets grid column
        /// Example: gridColumn "1 / -1"
        [<CustomOperation("gridColumn")>]
        member inline _.GridColumn(props, value: string) = ("gridColumn", value :> obj) :: props

        /// Sets grid row
        /// Example: gridRow "1 / 3"
        [<CustomOperation("gridRow")>]
        member inline _.GridRow(props, value: string) = ("gridRow", value :> obj) :: props

        /// Sets min-width
        /// Example: minWidth (Length.Units(200, Unit.Px))
        [<CustomOperation("minWidth")>]
        member inline _.MinWidth(props, value: Length) =
            ("minWidth", lengthToString value :> obj) :: props

        /// Sets max-width
        /// Example: maxWidth (Length.Units(800, Unit.Px))
        [<CustomOperation("maxWidth")>]
        member inline _.MaxWidth(props, value: Length) =
            ("maxWidth", lengthToString value :> obj) :: props

        /// Sets min-height
        /// Example: minHeight (Length.Units(100, Unit.Px))
        [<CustomOperation("minHeight")>]
        member inline _.MinHeight(props, value: Length) =
            ("minHeight", lengthToString value :> obj) :: props

        /// Sets max-height
        /// Example: maxHeight (Length.Units(400, Unit.Px))
        [<CustomOperation("maxHeight")>]
        member inline _.MaxHeight(props, value: Length) =
            ("maxHeight", lengthToString value :> obj) :: props

        // Convert to IReactProperty with "style" prop name
        member inline _.Run(props) : HtmlProp =
            let styleObj = createObj props
            let ret = unbox<HtmlProp> ("style", styleObj)
            ret

    let style = StyleBuilder()
