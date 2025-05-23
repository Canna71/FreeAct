module FreeSS

open FreeAct
open System.Text.RegularExpressions
open Fable.Core.JsInterop
open Microsoft.FSharp.Collections

[<Literal>]
let private NESTED_MARKER = "$_$_"

// type CssDeclaration =
//     | StyleDeclaration of string * obj
//     | NestedRule of CssRule

// and CssRule = { selector: string; rules: CssDeclaration list }

// Add the ClassName type
type ClassName =
    {
        Value: string
    }

    member this.AsCssSelector = $".{this.Value}"
    member this.AsClassName = this.Value
    // Add implicit conversion to string
    static member op_Implicit(cn: ClassName) : string = cn.AsClassName

type CssClassBuilder =
    inherit StyleBuilder

    val selector: string
    new(selector: string) = { selector = selector }
    // new (className: ClassName) = { selector = className.AsCssSelector }
    static member Create(selector: string) = CssClassBuilder(selector)

    static member Create(className: ClassName) =
        CssClassBuilder(className.AsCssSelector)

    member inline _.For(css: string * list<HtmlProp>, f: unit -> HtmlProp list) =
        let selector, rules = css
        let ret = (selector, box rules) :: f ()
        ret

    member inline _.For(props: list<HtmlProp>, f: unit -> string * list<HtmlProp>) =
        let selector, rules = f ()
        let ret = (selector, box rules) :: props
        ret

    member inline _.For(props: list<HtmlProp>, f: unit -> list<HtmlProp>) = props @ f ()

    member inline _.Yield(nested: string * list<HtmlProp>) =
        let selector, props = nested
        let ret = ($"{NESTED_MARKER}{selector}", box props) :: []
        ret

    member inline _.Delay(f: unit -> list<HtmlProp>) =
        let props = f ()
        props

    member inline this.Run(props: HtmlProp list) =
        let ret = this.selector, props
        ret

// let css = CssClassBuilder

// Define the shortcut using a function instead
let css (input: obj) =
    match input with
    | :? ClassName as cn -> CssClassBuilder.Create(cn)
    | :? string as s -> CssClassBuilder.Create(s)
    | _ -> failwithf "Invalid input type for css builder"

let mix = CssClassBuilder "&"

module private processor =

    // Define a partial active pattern for prefix matching
    let (|Nested|_|) (s: string) =
        if s.StartsWith NESTED_MARKER then
            Some(s.Substring NESTED_MARKER.Length)
        else
            None

    // active pattern for selector containing an ampersand
    let (|Ampersand|_|) (s: string) =
        if s.Contains "&" then
            Some(s)
        else
            None

    let uppercaseRegex = Regex("([A-Z])", RegexOptions.Compiled)

    let toCssProp prop =
        uppercaseRegex.Replace(prop, "-$1").ToLower()

    // substitute an ampesand with the parent selector
    let substituteAmpersand (selector: string) (s: string) = s.Replace("&", selector)

    let toCss (selector: string) (props: HtmlProp list) =

        let cssText = $"{selector} {{\n"

        let cssText =
            props
            |> List.map (fun (k, v) -> $"  {toCssProp k}: {v};")
            |> String.concat "\n"
            |> fun x -> cssText + x + "\n}\n"

        cssText

    // process a rule, flattening nested rules and concatenating with the parent selector
    let rec flattenRule (selector: string) (props: list<HtmlProp>) =
        let nestedRules =
            props
            |> List.choose (
                function
                | Nested selector, prop -> Some((selector, unbox<list<HtmlProp>> prop))
                | _ -> None
            )

        let otherProps =
            props
            |> List.choose (
                function
                | Nested _, _ -> None
                | prop -> Some(prop)
            )

        let thisRule = selector, otherProps
        // TODO: rename .classnames to something specific and add to a map
        let nestedRules =
            nestedRules
            |> List.map (fun (nested, props) ->
                let newSelector =
                    match nested with
                    | Ampersand _ -> substituteAmpersand selector nested
                    | _ -> $"{selector} {nested}"

                flattenRule newSelector props
            )
            |> List.concat

        thisRule :: nestedRules

    let processRules (rules) =
        rules
        |> List.collect (fun (selector, props) ->
            let flattenedRules = flattenRule selector props
            flattenedRules
        )
        |> List.map (fun (selector, props) ->
            let cssText = toCss selector props
            cssText
        )
        |> String.concat "\n" // concatenate all CSS rules

    open Browser

    let addCssToPage (cssText: string) =
        let _style = document.createElement "style"
        _style?``type`` <- "text/css"
        _style.innerHTML <- "/* This stylesheet was generated by FreeSS */\n" + cssText
        document.head.appendChild _style |> ignore

/// Main function to process and add CSS to the page
/// <summary>
/// Processes the provided CSS rules and adds them to the page.
/// </summary>
/// <param name="rules">A list of CSS rules to process.</param>
/// <returns>Unit</returns>
/// <example>
/// <code>
/// fss [ css "#root" { fontFamily "Arial, sans-serif" } ]
/// </code>
let fss rules =
    let cssRules = rules |> processor.processRules
    printf "%A" cssRules
    processor.addCssToPage cssRules

// tentative

type ClassNames = obj
type StyleMap<'T> = { Original: 'T; Unique: 'T }

open Microsoft.FSharp.Reflection
// Add helper functions
let inline private generateUniqueName (name: string) =
    $"""{name}_{System.Guid.NewGuid().ToString("N").Substring(0, 8)}"""

// Update makeStyles to return wrapped class names
let inline makeStyles<'T> () : 'T =
    let unique = createEmpty<'T>

    let fields = FSharpType.GetRecordFields(typeof<'T>)

    for field in fields do
        unique?(field.Name) <- { Value = generateUniqueName field.Name }

    unique

let testCssInFsharp () = "demo" |> ignore
// let cssText = toCss "test" rules
// console.log ("cssText\n", cssText)

// let classMap =
//     Map.empty |> Map.add "Header" "Header_0_1" |> Map.add "Main" "Main_0_2"

// classMap["Header"]<-"Header_0_1";
// classMap["Main"]<-"Main_0_2";

// console.log ("classMap", classMap)

// console.log ("Header", classMap["Header"])
// console.log ("Main", classMap["Main"])

// let otherClasses = createEmpty<obj>
// otherClasses?Header <- string "Header_0_1"
// otherClasses?Main <- "Main_0_2"

// console.log ("otherClasses", otherClasses)
// // Access with dynamic operator
// let f = otherClasses?Header
// console.log ("Header", string f)
// console.log ("Main", otherClasses?Main)
