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

    [<RequireQualifiedAccess>]
    type ReferrerPolicy =
        | NoReferrer
        | NoReferrerWhenDowngrade
        | Origin
        | OriginWhenCrossOrigin
        | SameOrigin
        | StrictOrigin
        | StrictOriginWhenCrossOrigin
        | UnsafeUrl

    let referrerPolicyToString =
        function
        | ReferrerPolicy.NoReferrer -> "no-referrer"
        | ReferrerPolicy.NoReferrerWhenDowngrade -> "no-referrer-when-downgrade"
        | ReferrerPolicy.Origin -> "origin"
        | ReferrerPolicy.OriginWhenCrossOrigin -> "origin-when-cross-origin"
        | ReferrerPolicy.SameOrigin -> "same-origin"
        | ReferrerPolicy.StrictOrigin -> "strict-origin"
        | ReferrerPolicy.StrictOriginWhenCrossOrigin -> "strict-origin-when-cross-origin"
        | ReferrerPolicy.UnsafeUrl -> "unsafe-url"

    [<RequireQualifiedAccess>]
    type Target =
        | Blank
        | Parent
        | Self
        | Top
        | UnfencedTop

    let targetToString =
        function
        | Target.Blank -> "_blank"
        | Target.Parent -> "_parent"
        | Target.Self -> "_self"
        | Target.Top -> "_top"
        | Target.UnfencedTop -> "_unfencedTop"

    [<RequireQualifiedAccess>]
    type CrossOrigin =
        | Anonymous
        | UseCredentials

    let crossOriginToString =
        function
        | CrossOrigin.Anonymous -> "anonymous"
        | CrossOrigin.UseCredentials -> "use-credentials"

    [<RequireQualifiedAccess>]
    type ButtonType =
        | Button
        | Submit
        | Reset

    let buttonTypeToString =
        function
        | ButtonType.Button -> "button"
        | ButtonType.Submit -> "submit"
        | ButtonType.Reset -> "reset"
