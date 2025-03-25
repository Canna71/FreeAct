namespace FreeAct

open FreeAct

[<AutoOpen>]
module HtmlEvents =

    type CoreHtmlElementBuilder with

        /// onClick event
        [<CustomOperation("onClick")>]
        member inline _.OnClick(props, handler: PointerEventHandler) =
            Prop("onClick", handler :> obj) :: props

        // onchange
        [<CustomOperation("onChange")>]
        member inline _.OnChange(props, handler: EventHandler) =
            Prop("onChange", handler :> obj) :: props
