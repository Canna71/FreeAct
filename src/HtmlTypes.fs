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

    [<RequireQualifiedAccess>]
    type PreloadStrategy =
        | None
        | Metadata
        | Auto

    let preloadStrategyToString =
        function
        | PreloadStrategy.None -> "none"
        | PreloadStrategy.Metadata -> "metadata"
        | PreloadStrategy.Auto -> "auto"

    [<RequireQualifiedAccess>]
    type InputType =
        | Button
        | Checkbox
        | Color
        | Date
        | DatetimeLocal
        | Email
        | File
        | Hidden
        | Image
        | Month
        | Number
        | Password
        | Radio
        | Range
        | Reset
        | Search
        | Submit
        | Tel
        | Text
        | Time
        | Url
        | Week

        override this.ToString() =
            match this with
            | Button -> "button"
            | Checkbox -> "checkbox"
            | Color -> "color"
            | Date -> "date"
            | DatetimeLocal -> "datetime-local"
            | Email -> "email"
            | File -> "file"
            | Hidden -> "hidden"
            | Image -> "image"
            | Month -> "month"
            | Number -> "number"
            | Password -> "password"
            | Radio -> "radio"
            | Range -> "range"
            | Reset -> "reset"
            | Search -> "search"
            | Submit -> "submit"
            | Tel -> "tel"
            | Text -> "text"
            | Time -> "time"
            | Url -> "url"
            | Week -> "week"

    [<RequireQualifiedAccess>]
    type WrapMode =
        | Soft
        | Hard
        | Off

    let wrapModeToString =
        function
        | WrapMode.Soft -> "soft"
        | WrapMode.Hard -> "hard"
        | WrapMode.Off -> "off"

    [<RequireQualifiedAccess>]
    type HttpEquiv =
        | ContentType
        | DefaultStyle
        | Refresh
        | ContentSecurityPolicy
        | XUACompatible

    let httpEquivToString =
        function
        | HttpEquiv.ContentType -> "content-type"
        | HttpEquiv.DefaultStyle -> "default-style"
        | HttpEquiv.Refresh -> "refresh"
        | HttpEquiv.ContentSecurityPolicy -> "content-security-policy"
        | HttpEquiv.XUACompatible -> "x-ua-compatible"

    [<RequireQualifiedAccess>]
    type LinkRelationType =
        | Stylesheet
        | Icon
        | Manifest
        | Alternate
        | Author
        | Canonical
        | DnsPrefetch
        | Help
        | License
        | Next
        | Pingback
        | Preconnect
        | Prefetch
        | Preload
        | Prev
        | Search

    let linkRelationTypeToString =
        function
        | LinkRelationType.Stylesheet -> "stylesheet"
        | LinkRelationType.Icon -> "icon"
        | LinkRelationType.Manifest -> "manifest"
        | LinkRelationType.Alternate -> "alternate"
        | LinkRelationType.Author -> "author"
        | LinkRelationType.Canonical -> "canonical"
        | LinkRelationType.DnsPrefetch -> "dns-prefetch"
        | LinkRelationType.Help -> "help"
        | LinkRelationType.License -> "license"
        | LinkRelationType.Next -> "next"
        | LinkRelationType.Pingback -> "pingback"
        | LinkRelationType.Preconnect -> "preconnect"
        | LinkRelationType.Prefetch -> "prefetch"
        | LinkRelationType.Preload -> "preload"
        | LinkRelationType.Prev -> "prev"
        | LinkRelationType.Search -> "search"

    [<RequireQualifiedAccess>]
    type TableCellScope =
        | Row
        | Col
        | RowGroup
        | ColGroup
        | Auto

    let tableCellScopeToString =
        function
        | TableCellScope.Row -> "row"
        | TableCellScope.Col -> "col"
        | TableCellScope.RowGroup -> "rowgroup"
        | TableCellScope.ColGroup -> "colgroup"
        | TableCellScope.Auto -> "auto"

    [<RequireQualifiedAccess>]
    type LoadingStrategy =
        | Eager
        | Lazy

    let loadingStrategyToString =
        function
        | LoadingStrategy.Eager -> "eager"
        | LoadingStrategy.Lazy -> "lazy"

    [<RequireQualifiedAccess>]
    type ImageDecoding =
        | Sync
        | Async
        | Auto

    let imageDecodingToString =
        function
        | ImageDecoding.Sync -> "sync"
        | ImageDecoding.Async -> "async"
        | ImageDecoding.Auto -> "auto"

    [<RequireQualifiedAccess>]
    type CanvasContextType =
        | TwoD
        | ThreeD
        | WebGL
        | WebGL2

    let canvasContextTypeToString =
        function
        | CanvasContextType.TwoD -> "2d"
        | CanvasContextType.ThreeD -> "3d"
        | CanvasContextType.WebGL -> "webgl"
        | CanvasContextType.WebGL2 -> "webgl2"

    [<RequireQualifiedAccess>]
    type PreserveAspectRatio =
        | None
        | XMinYMin
        | XMidYMin
        | XMaxYMin
        | XMinYMid
        | XMidYMid
        | XMaxYMid
        | XMinYMax
        | XMidYMax
        | XMaxYMax

    let preserveAspectRatioToString =
        function
        | PreserveAspectRatio.None -> "none"
        | PreserveAspectRatio.XMinYMin -> "xMinYMin"
        | PreserveAspectRatio.XMidYMin -> "xMidYMin"
        | PreserveAspectRatio.XMaxYMin -> "xMaxYMin"
        | PreserveAspectRatio.XMinYMid -> "xMinYMid"
        | PreserveAspectRatio.XMidYMid -> "xMidYMid"
        | PreserveAspectRatio.XMaxYMid -> "xMaxYMid"
        | PreserveAspectRatio.XMinYMax -> "xMinYMax"
        | PreserveAspectRatio.XMidYMax -> "xMidYMax"
        | PreserveAspectRatio.XMaxYMax -> "xMaxYMax"

    [<RequireQualifiedAccess>]
    type LengthAdjust =
        | Spacing
        | SpacingAndGlyphs

    let lengthAdjustToString =
        function
        | LengthAdjust.Spacing -> "spacing"
        | LengthAdjust.SpacingAndGlyphs -> "spacingAndGlyphs"

    [<RequireQualifiedAccess>]
    type GradientUnits =
        | UserSpaceOnUse
        | ObjectBoundingBox

    let gradientUnitsToString =
        function
        | GradientUnits.UserSpaceOnUse -> "userSpaceOnUse"
        | GradientUnits.ObjectBoundingBox -> "objectBoundingBox"

    [<RequireQualifiedAccess>]
    type SpreadMethod =
        | Pad
        | Reflect
        | Repeat

    let spreadMethodToString =
        function
        | SpreadMethod.Pad -> "pad"
        | SpreadMethod.Reflect -> "reflect"
        | SpreadMethod.Repeat -> "repeat"

    [<RequireQualifiedAccess>]
    type TrackKind =
        | Subtitles
        | Captions
        | Descriptions
        | Chapters
        | Metadata

    let trackKindToString =
        function
        | TrackKind.Subtitles -> "subtitles"
        | TrackKind.Captions -> "captions"
        | TrackKind.Descriptions -> "descriptions"
        | TrackKind.Chapters -> "chapters"
        | TrackKind.Metadata -> "metadata"
