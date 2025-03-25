namespace FreeAct

[<AutoOpen>]
module HtmlTypes =
    type Direction =
        | LTR
        | RTL
        | Auto

    let directionToString =
        function
        | LTR -> "ltr"
        | RTL -> "rtl"
        | Auto -> "auto"
