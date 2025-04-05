namespace FreeAct

open FreeAct

[<AutoOpen>]
module SvgElements =
    type SvgElementBuilder(tag: string) =
        inherit ElementWithChildrenBuilder(tag)

        [<CustomOperation("viewBox")>]
        member inline _.ViewBox(props, value: string) = Prop("viewBox", value :> obj) :: props

        [<CustomOperation("preserveAspectRatio")>]
        member inline _.PreserveAspectRatio(props, value: PreserveAspectRatio) =
            Prop("preserveAspectRatio", preserveAspectRatioToString value :> obj) :: props

        [<CustomOperation("fill")>]
        member inline _.Fill(props, value: string) = Prop("fill", value :> obj) :: props

        [<CustomOperation("stroke")>]
        member inline _.Stroke(props, value: string) = Prop("stroke", value :> obj) :: props

        [<CustomOperation("strokeWidth")>]
        member inline _.StrokeWidth(props, value: float) =
            Prop("stroke-width", value :> obj) :: props

        [<CustomOperation("transform")>]
        member inline _.Transform(props, value: string) =
            Prop("transform", value :> obj) :: props

    type SvgShapeBuilder(tag: string) =
        inherit SvgElementBuilder(tag)

        [<CustomOperation("x")>]
        member inline _.X(props, value: float) = Prop("x", value :> obj) :: props

        [<CustomOperation("y")>]
        member inline _.Y(props, value: float) = Prop("y", value :> obj) :: props

    type SvgCircleBuilder() =
        inherit SvgShapeBuilder("circle")

        [<CustomOperation("cx")>]
        member inline _.Cx(props, value: float) = Prop("cx", value :> obj) :: props

        [<CustomOperation("cy")>]
        member inline _.Cy(props, value: float) = Prop("cy", value :> obj) :: props

        [<CustomOperation("r")>]
        member inline _.R(props, value: float) = Prop("r", value :> obj) :: props

    type SvgEllipseBuilder() =
        inherit SvgShapeBuilder("ellipse")

        [<CustomOperation("cx")>]
        member inline _.Cx(props, value: float) = Prop("cx", value :> obj) :: props

        [<CustomOperation("cy")>]
        member inline _.Cy(props, value: float) = Prop("cy", value :> obj) :: props

        [<CustomOperation("rx")>]
        member inline _.Rx(props, value: float) = Prop("rx", value :> obj) :: props

        [<CustomOperation("ry")>]
        member inline _.Ry(props, value: float) = Prop("ry", value :> obj) :: props

    type SvgLineBuilder() =
        inherit SvgElementBuilder("line")

        [<CustomOperation("x1")>]
        member inline _.X1(props, value: float) = Prop("x1", value :> obj) :: props

        [<CustomOperation("y1")>]
        member inline _.Y1(props, value: float) = Prop("y1", value :> obj) :: props

        [<CustomOperation("x2")>]
        member inline _.X2(props, value: float) = Prop("x2", value :> obj) :: props

        [<CustomOperation("y2")>]
        member inline _.Y2(props, value: float) = Prop("y2", value :> obj) :: props

    type SvgPathBuilder() =
        inherit SvgElementBuilder("path")

        [<CustomOperation("d")>]
        member inline _.D(props, value: string) = Prop("d", value :> obj) :: props

    type SvgPolygonBuilder() =
        inherit SvgElementBuilder("polygon")

        [<CustomOperation("points")>]
        member inline _.Points(props, value: string) = Prop("points", value :> obj) :: props

    type SvgPolylineBuilder() =
        inherit SvgElementBuilder("polyline")

        [<CustomOperation("points")>]
        member inline _.Points(props, value: string) = Prop("points", value :> obj) :: props

    // Instantiate builders
    let svg = SvgElementBuilder("svg")
    let circle = SvgCircleBuilder()
    let ellipse = SvgEllipseBuilder()
    let line = SvgLineBuilder()
    let path = SvgPathBuilder()
    let polygon = SvgPolygonBuilder()
    let polyline = SvgPolylineBuilder()
    let rect = SvgShapeBuilder("rect")
