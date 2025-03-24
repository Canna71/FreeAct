namespace FreeAct

open FreeAct

[<AutoOpen>]
module HtmlAttributes =

    type ElementBuilder with

        /// Used to define a custom property on an element
        [<CustomOperation("prop")>]
        member inline _.Prop(props, propName: string, propValue: obj) =
            Prop(propName, propValue) :: props

        /// Used to define a custom property on an element
        [<CustomOperation("prop")>]
        member inline _.Prop(props, prop: HtmlProp) = Prop prop :: props

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
