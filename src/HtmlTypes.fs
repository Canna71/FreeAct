namespace FreeAct

[<AutoOpen>]
module HtmlTypes =
    [<RequireQualifiedAccess>]
    type Direction =
        | LTR
        | RTL
        | Auto

    let directionToString =
        function
        | Direction.LTR -> "ltr"
        | Direction.RTL -> "rtl"
        | Direction.Auto -> "auto"

    type PlaintextOnly = PlaintextOnly

    let plainTextOnlyToString =
        function
        | PlaintextOnly -> "plaintext-only"

    [<RequireQualifiedAccess>]
    type AutoCapitalize =
        | Off
        | None
        | On
        | Sentences
        | Words
        | Characters

    let autoCapitalizeToString =
        function
        | AutoCapitalize.Off -> "off"
        | AutoCapitalize.None -> "none"
        | AutoCapitalize.On -> "on"
        | AutoCapitalize.Sentences -> "sentences"
        | AutoCapitalize.Words -> "words"
        | AutoCapitalize.Characters -> "characters"
