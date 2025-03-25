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

    [<RequireQualifiedAccess>]
    type InputMode =
        | Decimal
        | Email
        | Numeric
        | Search
        | Tel
        | Text
        | Url
        | None

    let inputModeToString =
        function
        | InputMode.Decimal -> "decimal"
        | InputMode.Email -> "email"
        | InputMode.Numeric -> "numeric"
        | InputMode.Search -> "search"
        | InputMode.Tel -> "tel"
        | InputMode.Text -> "text"
        | InputMode.Url -> "url"
        | InputMode.None -> "none"
