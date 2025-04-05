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

    [<RequireQualifiedAccess>]
    type Filter =
        | Blur of Length
        | Brightness of int
        | Contrast of int
        | Grayscale of int
        | Invert of int
        | Opacity of int
        | Saturate of int
        | Sepia of int
        | Multiple of Filter list

    let rec filterToString =
        function
        | Filter.Blur l -> sprintf "blur(%s)" (lengthToString l)
        | Filter.Brightness n -> sprintf "brightness(%d%%)" n
        | Filter.Contrast n -> sprintf "contrast(%d%%)" n
        | Filter.Grayscale n -> sprintf "grayscale(%d%%)" n
        | Filter.Invert n -> sprintf "invert(%d%%)" n
        | Filter.Opacity n -> sprintf "opacity(%d%%)" n
        | Filter.Saturate n -> sprintf "saturate(%d%%)" n
        | Filter.Sepia n -> sprintf "sepia(%d%%)" n
        | Filter.Multiple filters -> filters |> List.map filterToString |> String.concat " "

    [<RequireQualifiedAccess>]
    type WhiteSpace =
        | Normal
        | NoWrap
        | Pre
        | PreWrap
        | PreLine

    let whiteSpaceToString =
        function
        | WhiteSpace.Normal -> "normal"
        | WhiteSpace.NoWrap -> "nowrap"
        | WhiteSpace.Pre -> "pre"
        | WhiteSpace.PreWrap -> "pre-wrap"
        | WhiteSpace.PreLine -> "pre-line"

    [<RequireQualifiedAccess>]
    type ListStyle =
        | None
        | Disc
        | Circle
        | Square
        | Decimal
        | LowerAlpha
        | UpperAlpha
        | LowerRoman
        | UpperRoman

    let listStyleToString =
        function
        | ListStyle.None -> "none"
        | ListStyle.Disc -> "disc"
        | ListStyle.Circle -> "circle"
        | ListStyle.Square -> "square"
        | ListStyle.Decimal -> "decimal"
        | ListStyle.LowerAlpha -> "lower-alpha"
        | ListStyle.UpperAlpha -> "upper-alpha"
        | ListStyle.LowerRoman -> "lower-roman"
        | ListStyle.UpperRoman -> "upper-roman"

    [<RequireQualifiedAccess>]
    type BorderStyle =
        | None
        | Solid
        | Dashed
        | Dotted
        | Double
        | Groove
        | Ridge
        | Inset
        | Outset

    let borderStyleToString =
        function
        | BorderStyle.None -> "none"
        | BorderStyle.Solid -> "solid"
        | BorderStyle.Dashed -> "dashed"
        | BorderStyle.Dotted -> "dotted"
        | BorderStyle.Double -> "double"
        | BorderStyle.Groove -> "groove"
        | BorderStyle.Ridge -> "ridge"
        | BorderStyle.Inset -> "inset"
        | BorderStyle.Outset -> "outset"

    [<RequireQualifiedAccess>]
    type BackgroundRepeat =
        | Repeat
        | RepeatX
        | RepeatY
        | NoRepeat
        | Space
        | Round

    let backgroundRepeatToString =
        function
        | BackgroundRepeat.Repeat -> "repeat"
        | BackgroundRepeat.RepeatX -> "repeat-x"
        | BackgroundRepeat.RepeatY -> "repeat-y"
        | BackgroundRepeat.NoRepeat -> "no-repeat"
        | BackgroundRepeat.Space -> "space"
        | BackgroundRepeat.Round -> "round"

    [<RequireQualifiedAccess>]
    type ClipPath =
        | None
        | Circle of int // percentage
        | Ellipse of width: int * height: int // percentages
        | Inset of top: Length * right: Length * bottom: Length * left: Length
        | Polygon of points: (int * int) list // percentage points

    let clipPathToString =
        function
        | ClipPath.None -> "none"
        | ClipPath.Circle p -> sprintf "circle(%d%%)" p
        | ClipPath.Ellipse(w, h) -> sprintf "ellipse(%d%% %d%%)" w h
        | ClipPath.Inset(t, r, b, l) ->
            sprintf
                "inset(%s %s %s %s)"
                (lengthToString t)
                (lengthToString r)
                (lengthToString b)
                (lengthToString l)
        | ClipPath.Polygon points ->
            points
            |> List.map (fun (x, y) -> sprintf "%d%% %d%%" x y)
            |> String.concat ", "
            |> sprintf "polygon(%s)"

    [<RequireQualifiedAccess>]
    type Resize =
        | None
        | Both
        | Horizontal
        | Vertical
        | Block
        | Inline

    let resizeToString =
        function
        | Resize.None -> "none"
        | Resize.Both -> "both"
        | Resize.Horizontal -> "horizontal"
        | Resize.Vertical -> "vertical"
        | Resize.Block -> "block"
        | Resize.Inline -> "inline"

    [<RequireQualifiedAccess>]
    type ObjectFit =
        | Fill
        | Contain
        | Cover
        | ScaleDown
        | None

    let objectFitToString =
        function
        | ObjectFit.Fill -> "fill"
        | ObjectFit.Contain -> "contain"
        | ObjectFit.Cover -> "cover"
        | ObjectFit.ScaleDown -> "scale-down"
        | ObjectFit.None -> "none"

    [<RequireQualifiedAccess>]
    type PointerEvents =
        | Auto
        | None
        | Inherit
        | Initial
        | Unset

    let pointerEventsToString =
        function
        | PointerEvents.Auto -> "auto"
        | PointerEvents.None -> "none"
        | PointerEvents.Inherit -> "inherit"
        | PointerEvents.Initial -> "initial"
        | PointerEvents.Unset -> "unset"

    [<RequireQualifiedAccess>]
    type UserSelect =
        | None
        | Auto
        | Text
        | All
        | Contain

    let userSelectToString =
        function
        | UserSelect.None -> "none"
        | UserSelect.Auto -> "auto"
        | UserSelect.Text -> "text"
        | UserSelect.All -> "all"
        | UserSelect.Contain -> "contain"

    [<RequireQualifiedAccess>]
    type TimingFunction =
        | Linear
        | Ease
        | EaseIn
        | EaseOut
        | EaseInOut
        | StepStart
        | StepEnd
        | Custom of string

    let timingFunctionToString =
        function
        | TimingFunction.Linear -> "linear"
        | TimingFunction.Ease -> "ease"
        | TimingFunction.EaseIn -> "ease-in"
        | TimingFunction.EaseOut -> "ease-out"
        | TimingFunction.EaseInOut -> "ease-in-out"
        | TimingFunction.StepStart -> "step-start"
        | TimingFunction.StepEnd -> "step-end"
        | TimingFunction.Custom s -> s

    [<RequireQualifiedAccess>]
    type ScrollBehavior =
        | Auto
        | Smooth
        | Initial
        | Inherit

    let scrollBehaviorToString =
        function
        | ScrollBehavior.Auto -> "auto"
        | ScrollBehavior.Smooth -> "smooth"
        | ScrollBehavior.Initial -> "initial"
        | ScrollBehavior.Inherit -> "inherit"

    [<RequireQualifiedAccess>]
    type BackgroundBlendMode =
        | Normal
        | Multiply
        | Screen
        | Overlay
        | Darken
        | Lighten
        | ColorDodge
        | ColorBurn
        | Difference
        | Exclusion

    let backgroundBlendModeToString =
        function
        | BackgroundBlendMode.Normal -> "normal"
        | BackgroundBlendMode.Multiply -> "multiply"
        | BackgroundBlendMode.Screen -> "screen"
        | BackgroundBlendMode.Overlay -> "overlay"
        | BackgroundBlendMode.Darken -> "darken"
        | BackgroundBlendMode.Lighten -> "lighten"
        | BackgroundBlendMode.ColorDodge -> "color-dodge"
        | BackgroundBlendMode.ColorBurn -> "color-burn"
        | BackgroundBlendMode.Difference -> "difference"
        | BackgroundBlendMode.Exclusion -> "exclusion"

    [<RequireQualifiedAccess>]
    type FontStretch =
        | UltraCondensed
        | ExtraCondensed
        | Condensed
        | SemiCondensed
        | Normal
        | SemiExpanded
        | Expanded
        | ExtraExpanded
        | UltraExpanded

    let fontStretchToString =
        function
        | FontStretch.UltraCondensed -> "ultra-condensed"
        | FontStretch.ExtraCondensed -> "extra-condensed"
        | FontStretch.Condensed -> "condensed"
        | FontStretch.SemiCondensed -> "semi-condensed"
        | FontStretch.Normal -> "normal"
        | FontStretch.SemiExpanded -> "semi-expanded"
        | FontStretch.Expanded -> "expanded"
        | FontStretch.ExtraExpanded -> "extra-expanded"
        | FontStretch.UltraExpanded -> "ultra-expanded"

    [<RequireQualifiedAccess>]
    type TextOverflow =
        | Clip
        | Ellipsis
        | Custom of string

    let textOverflowToString =
        function
        | TextOverflow.Clip -> "clip"
        | TextOverflow.Ellipsis -> "ellipsis"
        | TextOverflow.Custom s -> s

    [<RequireQualifiedAccess>]
    type FontVariant =
        | Normal
        | SmallCaps
        | AllSmallCaps
        | Ordinal
        | Slashed
        | Stacked
        | Unicase

    let fontVariantToString =
        function
        | FontVariant.Normal -> "normal"
        | FontVariant.SmallCaps -> "small-caps"
        | FontVariant.AllSmallCaps -> "all-small-caps"
        | FontVariant.Ordinal -> "ordinal"
        | FontVariant.Slashed -> "slashed-zero"
        | FontVariant.Stacked -> "stacked-fractions"
        | FontVariant.Unicase -> "unicase"

    [<RequireQualifiedAccess>]
    type BoxSizing =
        | ContentBox
        | BorderBox

    let boxSizingToString =
        function
        | BoxSizing.ContentBox -> "content-box"
        | BoxSizing.BorderBox -> "border-box"

    [<RequireQualifiedAccess>]
    type Visibility =
        | Visible
        | Hidden
        | Collapse

    let visibilityToString =
        function
        | Visibility.Visible -> "visible"
        | Visibility.Hidden -> "hidden"
        | Visibility.Collapse -> "collapse"

    [<RequireQualifiedAccess>]
    type TextDecoration =
        | None
        | Underline
        | Overline
        | LineThrough
        | Blink

    let textDecorationToString =
        function
        | TextDecoration.None -> "none"
        | TextDecoration.Underline -> "underline"
        | TextDecoration.Overline -> "overline"
        | TextDecoration.LineThrough -> "line-through"
        | TextDecoration.Blink -> "blink"

    [<RequireQualifiedAccess>]
    type BackgroundAttachment =
        | Scroll
        | Fixed
        | Local

    let backgroundAttachmentToString =
        function
        | BackgroundAttachment.Scroll -> "scroll"
        | BackgroundAttachment.Fixed -> "fixed"
        | BackgroundAttachment.Local -> "local"

    [<RequireQualifiedAccess>]
    type AspectRatio =
        | Auto
        | Square
        | Video
        | Custom of float

    let aspectRatioToString =
        function
        | AspectRatio.Auto -> "auto"
        | AspectRatio.Square -> "1/1"
        | AspectRatio.Video -> "16/9"
        | AspectRatio.Custom ratio -> string ratio

    [<RequireQualifiedAccess>]
    type Isolation =
        | Auto
        | Isolate

    let isolationToString =
        function
        | Isolation.Auto -> "auto"
        | Isolation.Isolate -> "isolate"

    /// CSS Duration helper type for time values
    [<RequireQualifiedAccess>]
    type Duration =
        | Ms of int
        | Sec of float

    let durationToString =
        function
        | Duration.Ms ms -> sprintf "%dms" ms
        | Duration.Sec s -> sprintf "%.1fs" s

    /// CSS Animation Direction type
    [<RequireQualifiedAccess>]
    type AnimationDirection =
        | Normal
        | Reverse
        | Alternate
        | AlternateReverse

    let animationDirectionToString =
        function
        | AnimationDirection.Normal -> "normal"
        | AnimationDirection.Reverse -> "reverse"
        | AnimationDirection.Alternate -> "alternate"
        | AnimationDirection.AlternateReverse -> "alternate-reverse"

    /// CSS Animation Fill Mode type
    [<RequireQualifiedAccess>]
    type AnimationFillMode =
        | None
        | Forwards
        | Backwards
        | Both

    let animationFillModeToString =
        function
        | AnimationFillMode.None -> "none"
        | AnimationFillMode.Forwards -> "forwards"
        | AnimationFillMode.Backwards -> "backwards"
        | AnimationFillMode.Both -> "both"

    /// Helper type for grid template areas
    type GridArea = string

    [<RequireQualifiedAccess>]
    type GridTemplate =
        | Areas of GridArea list list // Each inner list represents a row
        | Named of string

    let gridTemplateToString =
        function
        | GridTemplate.Areas areas ->
            areas
            |> List.map (fun row -> sprintf "\"%s\"" (String.concat " " row))
            |> String.concat " "
        | GridTemplate.Named template -> template

    /// Helper type for color stops in gradients
    type ColorStop =
        | Stop of string // Just color
        | StopAt of string * Length // Color with position
        | StopBetween of string * Length * Length // Color with position range

    let colorStopToString =
        function
        | Stop color -> color
        | StopAt(color, pos) -> sprintf "%s %s" color (lengthToString pos)
        | StopBetween(color, start, end') ->
            sprintf "%s %s %s" color (lengthToString start) (lengthToString end')

    [<RequireQualifiedAccess>]
    type Gradient =
        | Linear of angle: int * stops: ColorStop list
        | Radial of shape: string * stops: ColorStop list
        | Conic of angle: int * stops: ColorStop list
        | RepeatingLinear of angle: int * stops: ColorStop list

    let gradientToString =
        function
        | Gradient.Linear(angle, stops) ->
            stops
            |> List.map colorStopToString
            |> String.concat ", "
            |> sprintf "linear-gradient(%ddeg, %s)" angle
        | Gradient.Radial(shape, stops) ->
            stops
            |> List.map colorStopToString
            |> String.concat ", "
            |> sprintf "radial-gradient(%s, %s)" shape
        | Gradient.Conic(angle, stops) ->
            stops
            |> List.map colorStopToString
            |> String.concat ", "
            |> sprintf "conic-gradient(from %ddeg, %s)" angle
        | Gradient.RepeatingLinear(angle, stops) ->
            stops
            |> List.map colorStopToString
            |> String.concat ", "
            |> sprintf "repeating-linear-gradient(%ddeg, %s)" angle

    /// Enhanced transform operations
    [<RequireQualifiedAccess>]
    type Transform3D =
        | TranslateX of Length
        | TranslateY of Length
        | TranslateZ of Length
        | Translate3d of x: Length * y: Length * z: Length
        | Scale3d of x: float * y: float * z: float
        | RotateX of degrees: int
        | RotateY of degrees: int
        | RotateZ of degrees: int
        | Perspective of Length
        | Multiple of Transform3D list

    let rec transform3DToString =
        function
        | Transform3D.TranslateX x -> sprintf "translateX(%s)" (lengthToString x)
        | Transform3D.TranslateY y -> sprintf "translateY(%s)" (lengthToString y)
        | Transform3D.TranslateZ z -> sprintf "translateZ(%s)" (lengthToString z)
        | Transform3D.Translate3d(x, y, z) ->
            sprintf
                "translate3d(%s, %s, %s)"
                (lengthToString x)
                (lengthToString y)
                (lengthToString z)
        | Transform3D.Scale3d(x, y, z) -> sprintf "scale3d(%f, %f, %f)" x y z
        | Transform3D.RotateX deg -> sprintf "rotateX(%ddeg)" deg
        | Transform3D.RotateY deg -> sprintf "rotateY(%ddeg)" deg
        | Transform3D.RotateZ deg -> sprintf "rotateZ(%ddeg)" deg
        | Transform3D.Perspective l -> sprintf "perspective(%s)" (lengthToString l)
        | Transform3D.Multiple transforms ->
            transforms |> List.map transform3DToString |> String.concat " "

    /// Helper type for shadow offsets and blur
    [<RequireQualifiedAccess>]
    type Shadow =
        | Simple of color: string
        | Offset of x: Length * y: Length * color: string
        | Full of x: Length * y: Length * blur: Length * spread: Length * color: string
        | Multiple of Shadow list
        | Inset of Shadow

    let rec shadowToString =
        function
        | Shadow.Simple color -> color
        | Shadow.Offset(x, y, color) ->
            sprintf "%s %s %s" (lengthToString x) (lengthToString y) color
        | Shadow.Full(x, y, blur, spread, color) ->
            sprintf
                "%s %s %s %s %s"
                (lengthToString x)
                (lengthToString y)
                (lengthToString blur)
                (lengthToString spread)
                color
        | Shadow.Multiple shadows -> shadows |> List.map shadowToString |> String.concat ", "
        | Shadow.Inset shadow -> sprintf "inset %s" (shadowToString shadow)

    /// Helper type for font weight
    [<RequireQualifiedAccess>]
    type FontWeight =
        | Thin
        | ExtraLight
        | Light
        | Regular
        | Medium
        | SemiBold
        | Bold
        | ExtraBold
        | Black
        | Custom of int

    let fontWeightToString =
        function
        | FontWeight.Thin -> "100"
        | FontWeight.ExtraLight -> "200"
        | FontWeight.Light -> "300"
        | FontWeight.Regular -> "400"
        | FontWeight.Medium -> "500"
        | FontWeight.SemiBold -> "600"
        | FontWeight.Bold -> "700"
        | FontWeight.ExtraBold -> "800"
        | FontWeight.Black -> "900"
        | FontWeight.Custom weight -> string weight

    /// Helper type for line height
    [<RequireQualifiedAccess>]
    type LineHeight =
        | Normal
        | Length of Length
        | Multiple of float
        | Inherit

    let lineHeightToString =
        function
        | LineHeight.Normal -> "normal"
        | LineHeight.Length l -> lengthToString l
        | LineHeight.Multiple m -> string m
        | LineHeight.Inherit -> "inherit"

    /// Helper type for keyframe steps
    [<RequireQualifiedAccess>]
    type KeyframeStep =
        | From of (string * obj) list
        | To of (string * obj) list
        | Percentage of int * (string * obj) list

    let keyframeStepToString =
        function
        | KeyframeStep.From props ->
            props
            |> List.map (fun (k, v) -> sprintf "%s: %A" k v)
            |> String.concat "; "
            |> sprintf "from { %s }"
        | KeyframeStep.To props ->
            props
            |> List.map (fun (k, v) -> sprintf "%s: %A" k v)
            |> String.concat "; "
            |> sprintf "to { %s }"
        | KeyframeStep.Percentage(p, props) ->
            props
            |> List.map (fun (k, v) -> sprintf "%s: %A" k v)
            |> String.concat "; "
            |> sprintf "%d%% { %s }" p

    /// Helper type for text stroke
    [<RequireQualifiedAccess>]
    type TextStroke =
        | Width of Length
        | Color of string
        | Combined of Length * string

    let textStrokeToString =
        function
        | TextStroke.Width l -> lengthToString l
        | TextStroke.Color c -> c
        | TextStroke.Combined(w, c) -> sprintf "%s %s" (lengthToString w) c

    /// Helper type for flex basis
    [<RequireQualifiedAccess>]
    type FlexBasis =
        | Auto
        | Content
        | Length of Length
        | Fraction of int

    let flexBasisToString =
        function
        | FlexBasis.Auto -> "auto"
        | FlexBasis.Content -> "content"
        | FlexBasis.Length l -> lengthToString l
        | FlexBasis.Fraction n -> sprintf "%dfr" n

    /// Helper type for box spacing (margin/padding)
    [<RequireQualifiedAccess>]
    type Spacing =
        | All of Length
        | HorizontalVertical of horizontal: Length * vertical: Length
        | Sides of top: Length * right: Length * bottom: Length * left: Length
        | Zero

    let spacingToString =
        function
        | Spacing.All l -> lengthToString l
        | Spacing.HorizontalVertical(h, v) -> sprintf "%s %s" (lengthToString v) (lengthToString h)
        | Spacing.Sides(t, r, b, l) ->
            sprintf
                "%s %s %s %s"
                (lengthToString t)
                (lengthToString r)
                (lengthToString b)
                (lengthToString l)
        | Spacing.Zero -> "0"

    /// Helper type for border properties
    [<RequireQualifiedAccess>]
    type Border =
        | Width of Length
        | Style of BorderStyle
        | Color of string
        | Full of Length * BorderStyle * string
        | Shorthand of string // For cases like "1px solid red"
        | None

    let borderToString =
        function
        | Border.Width l -> lengthToString l
        | Border.Style s -> borderStyleToString s
        | Border.Color c -> c
        | Border.Full(w, s, c) -> sprintf "%s %s %s" (lengthToString w) (borderStyleToString s) c
        | Border.Shorthand s -> s
        | Border.None -> "none"

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

        /// Sets filter effects
        /// Example: filter (Filter.Blur(Length.Units(5, Unit.Px)))
        /// Example: filter (Filter.Multiple [Filter.Brightness 120; Filter.Contrast 110])
        [<CustomOperation("filter")>]
        member inline _.Filter(props, value: Filter) =
            ("filter", filterToString value :> obj) :: props

        /// Sets animation name
        /// Example: animationName "fadeIn"
        [<CustomOperation("animationName")>]
        member inline _.AnimationName(props, value: string) =
            ("animationName", value :> obj) :: props

        /// Sets animation duration
        /// Example: animationDuration "0.3s"
        [<CustomOperation("animationDuration")>]
        member inline _.AnimationDuration(props, value: string) =
            ("animationDuration", value :> obj) :: props

        /// Sets animation timing function
        /// Example: animationTimingFunction "ease-in-out"
        [<CustomOperation("animationTimingFunction")>]
        member inline _.AnimationTimingFunction(props, value: string) =
            ("animationTimingFunction", value :> obj) :: props

        /// Sets letter spacing
        /// Example: letterSpacing (Length.Units(2, Unit.Px))
        [<CustomOperation("letterSpacing")>]
        member inline _.LetterSpacing(props, value: Length) =
            ("letterSpacing", lengthToString value :> obj) :: props

        /// Sets line height
        /// Example: lineHeight "1.5"
        [<CustomOperation("lineHeight")>]
        member inline _.LineHeight(props, value: string) = ("lineHeight", value :> obj) :: props

        /// Sets white space handling
        /// Example: whiteSpace WhiteSpace.NoWrap
        [<CustomOperation("whiteSpace")>]
        member inline _.WhiteSpace(props, value: WhiteSpace) =
            ("whiteSpace", whiteSpaceToString value :> obj) :: props

        /// Sets list style type
        /// Example: listStyleType ListStyle.Decimal
        [<CustomOperation("listStyleType")>]
        member inline _.ListStyleType(props, value: ListStyle) =
            ("listStyleType", listStyleToString value :> obj) :: props

        /// Sets object fit
        /// Example: objectFit "cover"
        [<CustomOperation("objectFit")>]
        member inline _.ObjectFit(props, value: string) = ("objectFit", value :> obj) :: props

        /// Sets backdrop filter
        /// Example: backdropFilter (Filter.Blur(Length.Units(10, Unit.Px)))
        [<CustomOperation("backdropFilter")>]
        member inline _.BackdropFilter(props, value: Filter) =
            ("backdropFilter", filterToString value :> obj) :: props

        /// Sets mix blend mode
        /// Example: mixBlendMode "multiply"
        [<CustomOperation("mixBlendMode")>]
        member inline _.MixBlendMode(props, value: string) = ("mixBlendMode", value :> obj) :: props

        /// Sets border style
        /// Example: borderStyle BorderStyle.Solid
        [<CustomOperation("borderStyle")>]
        member inline _.BorderStyle(props, value: BorderStyle) =
            ("borderStyle", borderStyleToString value :> obj) :: props

        /// Sets border width
        /// Example: borderWidth (Length.Units(1, Unit.Px))
        [<CustomOperation("borderWidth")>]
        member inline _.BorderWidth(props, value: Length) =
            ("borderWidth", lengthToString value :> obj) :: props

        /// Sets border color
        /// Example: borderColor Color.red
        [<CustomOperation("borderColor")>]
        member inline _.BorderColor(props, value: string) = ("borderColor", value :> obj) :: props

        /// Sets outline style
        /// Example: outline "1px solid red"
        [<CustomOperation("outline")>]
        member inline _.Outline(props, value: string) = ("outline", value :> obj) :: props

        /// Sets outline offset
        /// Example: outlineOffset (Length.Units(2, Unit.Px))
        [<CustomOperation("outlineOffset")>]
        member inline _.OutlineOffset(props, value: Length) =
            ("outlineOffset", lengthToString value :> obj) :: props

        /// Sets background repeat
        /// Example: backgroundRepeat BackgroundRepeat.NoRepeat
        [<CustomOperation("backgroundRepeat")>]
        member inline _.BackgroundRepeat(props, value: BackgroundRepeat) =
            ("backgroundRepeat", backgroundRepeatToString value :> obj) :: props

        /// Sets background attachment
        /// Example: backgroundAttachment "fixed"
        [<CustomOperation("backgroundAttachment")>]
        member inline _.BackgroundAttachment(props, value: string) =
            ("backgroundAttachment", value :> obj) :: props

        /// Sets pointer events
        /// Example: pointerEvents "none"
        [<CustomOperation("pointerEvents")>]
        member inline _.PointerEvents(props, value: string) =
            ("pointerEvents", value :> obj) :: props

        /// Sets user select
        /// Example: userSelect "none"
        [<CustomOperation("userSelect")>]
        member inline _.UserSelect(props, value: string) = ("userSelect", value :> obj) :: props

        /// Sets individual border properties
        /// Example: borderLeft "1px solid red"
        [<CustomOperation("borderLeft")>]
        member inline _.BorderLeft(props, value: string) = ("borderLeft", value :> obj) :: props

        [<CustomOperation("borderRight")>]
        member inline _.BorderRight(props, value: string) = ("borderRight", value :> obj) :: props

        [<CustomOperation("borderTop")>]
        member inline _.BorderTop(props, value: string) = ("borderTop", value :> obj) :: props

        [<CustomOperation("borderBottom")>]
        member inline _.BorderBottom(props, value: string) = ("borderBottom", value :> obj) :: props

        /// Sets clip path
        /// Example: clipPath (ClipPath.Circle 50)
        [<CustomOperation("clipPath")>]
        member inline _.ClipPath(props, value: ClipPath) =
            ("clipPath", clipPathToString value :> obj) :: props

        /// Sets perspective
        /// Example: perspective (Length.Units(1000, Unit.Px))
        [<CustomOperation("perspective")>]
        member inline _.Perspective(props, value: Length) =
            ("perspective", lengthToString value :> obj) :: props

        /// Sets perspective origin
        /// Example: perspectiveOrigin "center"
        [<CustomOperation("perspectiveOrigin")>]
        member inline _.PerspectiveOrigin(props, value: string) =
            ("perspectiveOrigin", value :> obj) :: props

        /// Sets resize behavior
        /// Example: resize Resize.Both
        [<CustomOperation("resize")>]
        member inline _.Resize(props, value: Resize) =
            ("resize", resizeToString value :> obj) :: props

        /// Sets scrollbar width
        /// Example: scrollbarWidth "thin"
        [<CustomOperation("scrollbarWidth")>]
        member inline _.ScrollbarWidth(props, value: string) =
            ("scrollbarWidth", value :> obj) :: props

        /// Sets scroll behavior
        /// Example: scrollBehavior "smooth"
        [<CustomOperation("scrollBehavior")>]
        member inline _.ScrollBehavior(props, value: string) =
            ("scrollBehavior", value :> obj) :: props

        /// Sets scroll margin
        /// Example: scrollMargin (Length.Units(20, Unit.Px))
        [<CustomOperation("scrollMargin")>]
        member inline _.ScrollMargin(props, value: Length) =
            ("scrollMargin", lengthToString value :> obj) :: props

        /// Sets scroll padding
        /// Example: scrollPadding (Length.Units(20, Unit.Px))
        [<CustomOperation("scrollPadding")>]
        member inline _.ScrollPadding(props, value: Length) =
            ("scrollPadding", lengthToString value :> obj) :: props

        /// Sets scroll snap align
        /// Example: scrollSnapAlign "center"
        [<CustomOperation("scrollSnapAlign")>]
        member inline _.ScrollSnapAlign(props, value: string) =
            ("scrollSnapAlign", value :> obj) :: props

        /// Sets scroll snap type
        /// Example: scrollSnapType "x mandatory"
        [<CustomOperation("scrollSnapType")>]
        member inline _.ScrollSnapType(props, value: string) =
            ("scrollSnapType", value :> obj) :: props

        /// Sets background blend mode
        /// Example: backgroundBlendMode BackgroundBlendMode.Multiply
        [<CustomOperation("backgroundBlendMode")>]
        member inline _.BackgroundBlendMode(props, value: BackgroundBlendMode) =
            ("backgroundBlendMode", backgroundBlendModeToString value :> obj) :: props

        /// Sets font stretch
        /// Example: fontStretch FontStretch.Condensed
        [<CustomOperation("fontStretch")>]
        member inline _.FontStretch(props, value: FontStretch) =
            ("fontStretch", fontStretchToString value :> obj) :: props

        /// Sets text overflow
        /// Example: textOverflow TextOverflow.Ellipsis
        [<CustomOperation("textOverflow")>]
        member inline _.TextOverflow(props, value: TextOverflow) =
            ("textOverflow", textOverflowToString value :> obj) :: props

        /// Sets column count
        /// Example: columnCount 3
        [<CustomOperation("columnCount")>]
        member inline _.ColumnCount(props, value: int) = ("columnCount", value :> obj) :: props

        /// Sets column gap
        /// Example: columnGap (Length.Units(20, Unit.Px))
        [<CustomOperation("columnGap")>]
        member inline _.ColumnGap(props, value: Length) =
            ("columnGap", lengthToString value :> obj) :: props

        /// Sets column rule
        /// Example: columnRule "1px solid #ccc"
        [<CustomOperation("columnRule")>]
        member inline _.ColumnRule(props, value: string) = ("columnRule", value :> obj) :: props

        /// Sets column width
        /// Example: columnWidth (Length.Units(200, Unit.Px))
        [<CustomOperation("columnWidth")>]
        member inline _.ColumnWidth(props, value: Length) =
            ("columnWidth", lengthToString value :> obj) :: props

        /// Sets word spacing
        /// Example: wordSpacing (Length.Units(2, Unit.Px))
        [<CustomOperation("wordSpacing")>]
        member inline _.WordSpacing(props, value: Length) =
            ("wordSpacing", lengthToString value :> obj) :: props

        /// Sets font variant
        /// Example: fontVariant FontVariant.SmallCaps
        [<CustomOperation("fontVariant")>]
        member inline _.FontVariant(props, value: FontVariant) =
            ("fontVariant", fontVariantToString value :> obj) :: props

        /// Sets box sizing
        /// Example: boxSizing BoxSizing.BorderBox
        [<CustomOperation("boxSizing")>]
        member inline _.BoxSizing(props, value: BoxSizing) =
            ("boxSizing", boxSizingToString value :> obj) :: props

        /// Sets visibility
        /// Example: visibility Visibility.Hidden
        [<CustomOperation("visibility")>]
        member inline _.Visibility(props, value: Visibility) =
            ("visibility", visibilityToString value :> obj) :: props

        /// Sets isolation
        /// Example: isolation "isolate"
        [<CustomOperation("isolation")>]
        member inline _.Isolation(props, value: string) = ("isolation", value :> obj) :: props

        /// Sets aspect ratio
        /// Example: aspectRatio "16/9"
        [<CustomOperation("aspectRatio")>]
        member inline _.AspectRatio(props, value: string) = ("aspectRatio", value :> obj) :: props

        /// Sets text decoration
        /// Example: textDecoration TextDecoration.Underline
        [<CustomOperation("textDecoration")>]
        member inline _.TextDecoration(props, value: TextDecoration) =
            ("textDecoration", textDecorationToString value :> obj) :: props

        /// Sets background attachment
        /// Example: backgroundAttachment BackgroundAttachment.Fixed
        [<CustomOperation("backgroundAttachment")>]
        member inline _.BackgroundAttachment(props, value: BackgroundAttachment) =
            ("backgroundAttachment", backgroundAttachmentToString value :> obj) :: props

        /// Sets isolation
        /// Example: isolation Isolation.Isolate
        [<CustomOperation("isolation")>]
        member inline _.Isolation(props, value: Isolation) =
            ("isolation", isolationToString value :> obj) :: props

        /// Sets aspect ratio
        /// Example: aspectRatio AspectRatio.Video
        [<CustomOperation("aspectRatio")>]
        member inline _.AspectRatio(props, value: AspectRatio) =
            ("aspectRatio", aspectRatioToString value :> obj) :: props

        /// Sets animation direction
        /// Example: animationDirection AnimationDirection.Alternate
        [<CustomOperation("animationDirection")>]
        member inline _.AnimationDirection(props, value: AnimationDirection) =
            ("animationDirection", animationDirectionToString value :> obj) :: props

        /// Sets animation fill mode
        /// Example: animationFillMode AnimationFillMode.Forwards
        [<CustomOperation("animationFillMode")>]
        member inline _.AnimationFillMode(props, value: AnimationFillMode) =
            ("animationFillMode", animationFillModeToString value :> obj) :: props

        /// Sets animation duration using type-safe Duration
        /// Example: animationDurationTyped (Duration.Ms 300)
        /// Example: animationDurationTyped (Duration.Sec 0.3)
        [<CustomOperation("animationDurationTyped")>]
        member inline _.AnimationDurationTyped(props, value: Duration) =
            ("animationDuration", durationToString value :> obj) :: props

        /// Sets grid template areas
        /// Example: gridTemplateAreas (GridTemplate.Areas [["header"; "header"]; ["nav"; "main"]; ["footer"; "footer"]])
        [<CustomOperation("gridTemplateAreas")>]
        member inline _.GridTemplateAreas(props, value: GridTemplate) =
            ("gridTemplateAreas", gridTemplateToString value :> obj) :: props

        /// Sets grid area
        /// Example: gridArea "header"
        [<CustomOperation("gridArea")>]
        member inline _.GridArea(props, value: GridArea) = ("gridArea", value :> obj) :: props

        /// Sets background with gradient
        /// Example: backgroundGradient (Gradient.Linear(45, [Stop Color.red; StopAt(Color.blue, Length.Percent 50)]))
        [<CustomOperation("backgroundGradient")>]
        member inline _.BackgroundGradient(props, value: Gradient) =
            ("background", gradientToString value :> obj) :: props

        /// Sets 3D transforms
        /// Example: transform3D (Transform3D.Multiple [Transform3D.TranslateZ(Length.Px 100); Transform3D.RotateY 45])
        [<CustomOperation("transform3D")>]
        member inline _.Transform3D(props, value: Transform3D) =
            ("transform", transform3DToString value :> obj) :: props

        /// Sets transform origin with separate x, y values
        /// Example: transformOrigin (Length.Percent 50) (Length.Percent 50)
        [<CustomOperation("transformOrigin")>]
        member inline _.TransformOrigin(props, x: Length, y: Length) =
            ("transformOrigin", sprintf "%s %s" (lengthToString x) (lengthToString y) :> obj)
            :: props

        /// Sets box shadow using type-safe Shadow type
        /// Example: boxShadowTyped (Shadow.Offset(Length.Px 2, Length.Px 2, Color.rgba(0,0,0,0.2)))
        [<CustomOperation("boxShadowTyped")>]
        member inline _.BoxShadowTyped(props, value: Shadow) =
            ("boxShadow", shadowToString value :> obj) :: props

        /// Sets text shadow using type-safe Shadow type
        /// Example: textShadowTyped (Shadow.Offset(Length.Px 1, Length.Px 1, Color.rgba(0,0,0,0.5)))
        [<CustomOperation("textShadowTyped")>]
        member inline _.TextShadowTyped(props, value: Shadow) =
            ("textShadow", shadowToString value :> obj) :: props

        /// Sets font weight using type-safe FontWeight type
        /// Example: fontWeightTyped FontWeight.Bold
        [<CustomOperation("fontWeightTyped")>]
        member inline _.FontWeightTyped(props, value: FontWeight) =
            ("fontWeight", fontWeightToString value :> obj) :: props

        /// Sets line height using type-safe LineHeight type
        /// Example: lineHeightTyped (LineHeight.Multiple 1.5)
        [<CustomOperation("lineHeightTyped")>]
        member inline _.LineHeightTyped(props, value: LineHeight) =
            ("lineHeight", lineHeightToString value :> obj) :: props

        /// Sets text stroke width
        /// Example: textStrokeWidth (Length.Units(1, Unit.Px))
        [<CustomOperation("textStrokeWidth")>]
        member inline _.TextStrokeWidth(props, value: Length) =
            ("-webkit-text-stroke-width", lengthToString value :> obj) :: props

        /// Sets text stroke color
        /// Example: textStrokeColor Color.black
        [<CustomOperation("textStrokeColor")>]
        member inline _.TextStrokeColor(props, value: string) =
            ("-webkit-text-stroke-color", value :> obj) :: props

        /// Sets combined text stroke
        /// Example: textStroke (TextStroke.Combined(Length.Units(1, Unit.Px), Color.black))
        [<CustomOperation("textStroke")>]
        member inline _.TextStroke(props, value: TextStroke) =
            ("-webkit-text-stroke", textStrokeToString value :> obj) :: props

        /// Sets flex basis
        /// Example: flexBasis (FlexBasis.Length(Length.Units(200, Unit.Px)))
        [<CustomOperation("flexBasis")>]
        member inline _.FlexBasis(props, value: FlexBasis) =
            ("flexBasis", flexBasisToString value :> obj) :: props

        /// Sets flex grow
        /// Example: flexGrow 1
        [<CustomOperation("flexGrow")>]
        member inline _.FlexGrow(props, value: int) = ("flexGrow", value :> obj) :: props

        /// Sets flex shrink
        /// Example: flexShrink 0
        [<CustomOperation("flexShrink")>]
        member inline _.FlexShrink(props, value: int) = ("flexShrink", value :> obj) :: props

        /// Sets combined flex properties (grow, shrink, basis)
        /// Example: flexCombined (2, 1, FlexBasis.Length(Length.Units(100, Unit.Px)))
        [<CustomOperation("flexCombined")>]
        member inline _.FlexCombined(props, grow: int, shrink: int, basis: FlexBasis) =
            ("flex", sprintf "%d %d %s" grow shrink (flexBasisToString basis) :> obj)
            :: props

        /// Sets margin using spacing helper
        /// Example: marginSpacing (Spacing.All(Length.Px 10))
        /// Example: marginSpacing (Spacing.Horizontal(Length.Px 20, Length.Px 10))
        [<CustomOperation("marginSpacing")>]
        member inline _.MarginSpacing(props, value: Spacing) =
            ("margin", spacingToString value :> obj) :: props

        /// Sets individual margin sides
        /// Example: margins { top = Length.Px 10; right = Length.Px 20; bottom = Length.Px 10; left = Length.Px 20 }
        [<CustomOperation("margins")>]
        member inline _.Margins
            (
                props,
                value:
                    {|
                        top: Length
                        right: Length
                        bottom: Length
                        left: Length
                    |}
            )
            =
            [
                "marginTop", lengthToString value.top :> obj
                "marginRight", lengthToString value.right :> obj
                "marginBottom", lengthToString value.bottom :> obj
                "marginLeft", lengthToString value.left :> obj
            ]
            @ props

        /// Sets padding using spacing helper
        /// Example: paddingSpacing (Spacing.VerticalAndHorizontal(Length.Px 10, Length.Px 20))
        [<CustomOperation("paddingSpacing")>]
        member inline _.PaddingSpacing(props, value: Spacing) =
            ("padding", spacingToString value :> obj) :: props

        /// Sets individual padding sides
        /// Example: paddings { top = Length.Px 10; right = Length.Px 20; bottom = Length.Px 10; left = Length.Px 20 }
        [<CustomOperation("paddings")>]
        member inline _.Paddings
            (
                props,
                value:
                    {|
                        top: Length
                        right: Length
                        bottom: Length
                        left: Length
                    |}
            )
            =
            [
                "paddingTop", lengthToString value.top :> obj
                "paddingRight", lengthToString value.right :> obj
                "paddingBottom", lengthToString value.bottom :> obj
                "paddingLeft", lengthToString value.left :> obj
            ]
            @ props

        /// Sets border with full configuration
        /// Example: borderFull (Border.Full(Length.Px 1, BorderStyle.Solid, Color.red))
        [<CustomOperation("borderFull")>]
        member inline _.BorderFull(props, value: Border) =
            ("border", borderToString value :> obj) :: props

        /// Sets individual border sides with full configuration
        /// Example: borders {
        ///   top = Border.Full(Length.Px 1, BorderStyle.Solid, Color.red)
        ///   right = Border.None
        ///   bottom = Border.Shorthand "2px dashed blue"
        ///   left = Border.Style BorderStyle.Dotted
        /// }
        [<CustomOperation("borders")>]
        member inline _.Borders
            (
                props,
                value:
                    {|
                        top: Border
                        right: Border
                        bottom: Border
                        left: Border
                    |}
            )
            =
            [
                "borderTop", borderToString value.top :> obj
                "borderRight", borderToString value.right :> obj
                "borderBottom", borderToString value.bottom :> obj
                "borderLeft", borderToString value.left :> obj
            ]
            @ props

        // Convert to IReactProperty with "style" prop name
        member inline _.Run(props) : HtmlProp =
            let styleObj = createObj props
            let ret = unbox<HtmlProp> ("style", styleObj)
            ret

    let style = StyleBuilder()

    type StyleBuilder with
        // Replace duplicate margin/padding/border operations with new names

        /// Sets margin with four values
        /// Example: margin4 (Length.Px 10) (Length.Px 20) (Length.Px 10) (Length.Px 20)
        [<CustomOperation("margin4")>]
        member inline _.Margin4(props, top: Length, right: Length, bottom: Length, left: Length) =
            ("margin",
             sprintf
                 "%s %s %s %s"
                 (lengthToString top)
                 (lengthToString right)
                 (lengthToString bottom)
                 (lengthToString left)
             :> obj)
            :: props

        /// Sets margin with two values for X and Y
        /// Example: marginXY (Length.Px 20) (Length.Px 10)
        [<CustomOperation("marginXY")>]
        member inline _.MarginXY(props, horizontal: Length, vertical: Length) =
            ("margin", sprintf "%s %s" (lengthToString vertical) (lengthToString horizontal) :> obj)
            :: props

        /// Sets padding with four values
        /// Example: padding4 (Length.Px 10) (Length.Px 20) (Length.Px 10) (Length.Px 20)
        [<CustomOperation("padding4")>]
        member inline _.Padding4(props, top: Length, right: Length, bottom: Length, left: Length) =
            ("padding",
             sprintf
                 "%s %s %s %s"
                 (lengthToString top)
                 (lengthToString right)
                 (lengthToString bottom)
                 (lengthToString left)
             :> obj)
            :: props

        /// Sets padding with two values for X and Y
        /// Example: paddingXY (Length.Px 20) (Length.Px 10)
        [<CustomOperation("paddingXY")>]
        member inline _.PaddingXY(props, horizontal: Length, vertical: Length) =
            ("padding", sprintf "%s %s" (lengthToString vertical) (lengthToString horizontal) :> obj)
            :: props

        /// Sets complete border style in one operation
        /// Example: borderComplete (Length.Px 1) BorderStyle.Solid Color.red
        [<CustomOperation("borderComplete")>]
        member inline _.BorderComplete(props, width: Length, style: BorderStyle, color: string) =
            ("border",
             sprintf "%s %s %s" (lengthToString width) (borderStyleToString style) color :> obj)
            :: props

        /// Sets border style for a specific side
        /// Example: borderSide "top" (Length.Px 1) BorderStyle.Solid Color.red
        [<CustomOperation("borderSide")>]
        member inline _.BorderSide
            (props, side: string, width: Length, style: BorderStyle, color: string)
            =
            (sprintf "border%s" (side.Substring(0, 1).ToUpper() + side.Substring(1)),
             sprintf "%s %s %s" (lengthToString width) (borderStyleToString style) color :> obj)
            :: props

        /// Sets border radius for all corners individually
        /// Example: borderRadius4 (Length.Px 10) (Length.Px 20) (Length.Px 10) (Length.Px 20)
        [<CustomOperation("borderRadius4")>]
        member inline _.BorderRadius4
            (props, topLeft: Length, topRight: Length, bottomRight: Length, bottomLeft: Length)
            =
            ("borderRadius",
             sprintf
                 "%s %s %s %s"
                 (lengthToString topLeft)
                 (lengthToString topRight)
                 (lengthToString bottomRight)
                 (lengthToString bottomLeft)
             :> obj)
            :: props

        // Margin shorthands
        [<CustomOperation("mt")>]
        member inline _.MarginTop(props, value: Length) =
            ("marginTop", lengthToString value :> obj) :: props

        [<CustomOperation("mr")>]
        member inline _.MarginRight(props, value: Length) =
            ("marginRight", lengthToString value :> obj) :: props

        [<CustomOperation("mb")>]
        member inline _.MarginBottom(props, value: Length) =
            ("marginBottom", lengthToString value :> obj) :: props

        [<CustomOperation("ml")>]
        member inline _.MarginLeft(props, value: Length) =
            ("marginLeft", lengthToString value :> obj) :: props

        // Padding shorthands
        [<CustomOperation("pt")>]
        member inline _.PaddingTop(props, value: Length) =
            ("paddingTop", lengthToString value :> obj) :: props

        [<CustomOperation("pr")>]
        member inline _.PaddingRight(props, value: Length) =
            ("paddingRight", lengthToString value :> obj) :: props

        [<CustomOperation("pb")>]
        member inline _.PaddingBottom(props, value: Length) =
            ("paddingBottom", lengthToString value :> obj) :: props

        [<CustomOperation("pl")>]
        member inline _.PaddingLeft(props, value: Length) =
            ("paddingLeft", lengthToString value :> obj) :: props

        // Border radius shorthands
        [<CustomOperation("roundedTop")>]
        member inline _.RoundedTop(props, value: Length) =
            [
                "borderTopLeftRadius", lengthToString value :> obj
                "borderTopRightRadius", lengthToString value :> obj
            ]
            @ props

        [<CustomOperation("roundedBottom")>]
        member inline _.RoundedBottom(props, value: Length) =
            [
                "borderBottomLeftRadius", lengthToString value :> obj
                "borderBottomRightRadius", lengthToString value :> obj
            ]
            @ props

        [<CustomOperation("rounded")>]
        member inline _.Rounded(props, value: Length) =
            ("borderRadius", lengthToString value :> obj) :: props

        // Common border shorthands
        [<CustomOperation("border1px")>]
        member inline _.Border1px(props, style: BorderStyle, color: string) =
            ("border", sprintf "1px %s %s" (borderStyleToString style) color :> obj)
            :: props

        [<CustomOperation("borderTop1px")>]
        member inline _.BorderTop1px(props, style: BorderStyle, color: string) =
            ("borderTop", sprintf "1px %s %s" (borderStyleToString style) color :> obj)
            :: props

        // Width/Height shorthands
        [<CustomOperation("w")>]
        member inline _.ShortWidth(props, value: Length) =
            ("width", lengthToString value :> obj) :: props

        [<CustomOperation("h")>]
        member inline _.ShortHeight(props, value: Length) =
            ("height", lengthToString value :> obj) :: props

        [<CustomOperation("wh")>]
        member inline _.WidthHeight(props, w: Length, h: Length) =
            [ "width", lengthToString w :> obj; "height", lengthToString h :> obj ] @ props

        // Display shorthands
        [<CustomOperation("block")>]
        member inline _.DisplayBlock(props) = ("display", "block" :> obj) :: props

        [<CustomOperation("flex")>]
        member inline _.DisplayFlex(props) = ("display", "flex" :> obj) :: props

        [<CustomOperation("grid")>]
        member inline _.DisplayGrid(props) = ("display", "grid" :> obj) :: props

        [<CustomOperation("hidden")>]
        member inline _.DisplayNone(props) = ("display", "none" :> obj) :: props

        // Flexbox shorthands
        [<CustomOperation("row")>]
        member inline _.FlexRow(props) =
            ("flexDirection", "row" :> obj) :: props

        [<CustomOperation("col")>]
        member inline _.FlexColumn(props) =
            ("flexDirection", "column" :> obj) :: props

        [<CustomOperation("center")>]
        member inline _.FlexCenter(props) =
            [ "justifyContent", "center" :> obj; "alignItems", "center" :> obj ] @ props

        [<CustomOperation("between")>]
        member inline _.SpaceBetween(props) =
            ("justifyContent", "space-between" :> obj) :: props

        // Position shorthands
        [<CustomOperation("absolute")>]
        member inline _.PositionAbsolute(props) =
            ("position", "absolute" :> obj) :: props

        [<CustomOperation("relative")>]
        member inline _.PositionRelative(props) =
            ("position", "relative" :> obj) :: props

        [<CustomOperation("fixed")>]
        member inline _.PositionFixed(props) = ("position", "fixed" :> obj) :: props

        [<CustomOperation("sticky")>]
        member inline _.PositionSticky(props) = ("position", "sticky" :> obj) :: props

        // Background shorthands
        [<CustomOperation("bg")>]
        member inline _.Background(props, color: string) =
            ("backgroundColor", color :> obj) :: props

        [<CustomOperation("bgCover")>]
        member inline _.BackgroundCover(props) =
            ("backgroundSize", "cover" :> obj) :: props

        // Font shorthands
        [<CustomOperation("text")>]
        member inline _.TextColor(props, color: string) = ("color", color :> obj) :: props

        [<CustomOperation("fontSize")>]
        member inline _.FontSizeShort(props, size: int) =
            ("fontSize", sprintf "%dpx" size :> obj) :: props

        [<CustomOperation("bold")>]
        member inline _.FontBold(props) = ("fontWeight", "bold" :> obj) :: props

        // Common combinations
        [<CustomOperation("centerContent")>]
        member inline _.CenterContent(props) =
            [
                "display", "flex" :> obj
                "justifyContent", "center" :> obj
                "alignItems", "center" :> obj
            ]
            @ props

        [<CustomOperation("fullScreen")>]
        member inline _.FullScreen(props) =
            [
                "width", "100vw" :> obj
                "height", "100vh" :> obj
                "position", "fixed" :> obj
                "top", "0" :> obj
                "left", "0" :> obj
            ]
            @ props

        [<CustomOperation("pointer")>]
        member inline _.Pointer(props) = ("cursor", "pointer" :> obj) :: props

        // Z-index layer shortcuts
        [<CustomOperation("layer")>]
        member inline _.Layer(props, value: int) = ("zIndex", value :> obj) :: props

        [<CustomOperation("front")>]
        member inline _.Front(props) = ("zIndex", 999999 :> obj) :: props

        [<CustomOperation("back")>]
        member inline _.Back(props) = ("zIndex", -1 :> obj) :: props

        // Position shortcuts with coordinates
        [<CustomOperation("inset")>]
        member inline _.Inset(props, value: Length) =
            [
                "top", lengthToString value :> obj
                "right", lengthToString value :> obj
                "bottom", lengthToString value :> obj
                "left", lengthToString value :> obj
            ]
            @ props

        // Transition shortcuts
        [<CustomOperation("fadeIn")>]
        member inline _.FadeIn(props) =
            [ "opacity", 1 :> obj; "transition", "opacity 0.2s ease-in-out" :> obj ] @ props

        [<CustomOperation("fadeOut")>]
        member inline _.FadeOut(props) =
            [ "opacity", 0 :> obj; "transition", "opacity 0.2s ease-in-out" :> obj ] @ props

        // Quick transitions
        [<CustomOperation("transitionAll")>]
        member inline _.TransitionAll(props) =
            ("transition", "all 0.2s ease-in-out" :> obj) :: props

        [<CustomOperation("transitionColors")>]
        member inline _.TransitionColors(props) =
            ("transition", "background-color 0.2s ease, color 0.2s ease" :> obj) :: props

        // Text shortcuts
        [<CustomOperation("ellipsis")>]
        member inline _.Ellipsis(props) =
            [
                "overflow", "hidden" :> obj
                "textOverflow", "ellipsis" :> obj
                "whiteSpace", "nowrap" :> obj
            ]
            @ props

        [<CustomOperation("noSelect")>]
        member inline _.NoSelect(props) =
            [ "userSelect", "none" :> obj; "webkitUserSelect", "none" :> obj ] @ props

        // Layout helpers
        [<CustomOperation("fullWidth")>]
        member inline _.FullWidth(props) = ("width", "100%" :> obj) :: props

        [<CustomOperation("fullHeight")>]
        member inline _.FullHeight(props) = ("height", "100%" :> obj) :: props

        [<CustomOperation("square")>]
        member inline _.Square(props, size: Length) =
            [ "width", lengthToString size :> obj; "height", lengthToString size :> obj ]
            @ props

        // Box model shortcuts
        [<CustomOperation("content")>]
        member inline _.ContentBox(props) =
            ("boxSizing", "content-box" :> obj) :: props

        [<CustomOperation("borderBox")>]
        member inline _.BorderBox(props) =
            ("boxSizing", "border-box" :> obj) :: props

        // Overflow shortcuts
        [<CustomOperation("scroll")>]
        member inline _.Scroll(props) = ("overflow", "auto" :> obj) :: props

        [<CustomOperation("scrollX")>]
        member inline _.ScrollX(props) =
            [ "overflowX", "auto" :> obj; "overflowY", "hidden" :> obj ] @ props

        [<CustomOperation("scrollY")>]
        member inline _.ScrollY(props) =
            [ "overflowX", "hidden" :> obj; "overflowY", "auto" :> obj ] @ props
