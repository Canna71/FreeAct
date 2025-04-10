module FreeSS

open FreeAct
open Fable.Core.JS
open System.Text.RegularExpressions
open System.Dynamic
open Fable.Core.JsInterop
open Microsoft.FSharp.Collections

let NESTED_MARKER = "$_$_"

// Define a partial active pattern for prefix matching
let (|Nested|_|) (s: string) =
    if s.StartsWith NESTED_MARKER then
        Some(s.Substring NESTED_MARKER.Length)
    else
        None

// type CssDeclaration =
//     | StyleDeclaration of string * obj
//     | NestedRule of CssRule

// and CssRule = { selector: string; rules: CssDeclaration list }

type CssClassBuilder(selector: string) =
    inherit StyleBuilder()

    member inline _.For(css: string * list<HtmlProp>, f: unit -> HtmlProp list) =
        console.log ("CssClassBuilder For 1", selector, css, f ())
        let selector, rules = css
        let ret = (selector, box rules) :: f ()
        ret

    member inline _.For(props: list<HtmlProp>, f: unit -> string * list<HtmlProp>) =
        console.log ("CssClassBuilder For 2", selector, props, f ())
        let selector, rules = f ()
        let ret = (selector, box rules) :: props
        ret

    member inline _.For(props: list<HtmlProp>, f: unit -> list<HtmlProp>) =
        console.log ("CssClassBuilder For 3 ", selector, props, f ())
        props @ f ()

    member inline _.Yield(nested: string * list<HtmlProp>) =
        let selector, props = nested
        let ret = ($"{NESTED_MARKER}{selector}", box props) :: []
        console.log ("CssClassBuilder Yield", selector, nested, ret)
        ret

    member inline _.Delay(f: unit -> list<HtmlProp>) =
        console.log ("CssClassBuilder Delay", selector, f ())
        let props = f ()
        props

    member inline _.Run(props: HtmlProp list) =
        let ret = selector, props
        console.log ("CssClassBuilder Run", selector, ret)
        ret

let css = CssClassBuilder

let rules2 =
    [

        css "main" {
            flex
            backgroundColor "#f0f0f0"

            css "nested" { color "#f0f0f0" }
        }

        css "header" {
            flex
            backgroundColor "#f0f0f0"
        }
    ]

let uppercaseRegex = Regex("([A-Z])", RegexOptions.Compiled)

let toCss (selector: string) (props: HtmlProp list) =

    let toCssProp prop =
        uppercaseRegex.Replace(prop, "-$1").ToLower()

    let cssText = $"{selector} {{\n"

    let cssText =
        props
        |> List.map (fun (k, v) -> $"{toCssProp k}: {v};")
        |> String.concat "\n"
        |> fun x -> cssText + x + "\n}\n"

    cssText

// process a rule, flattening nested rules and concatenating with the parent selector
let rec flattenRule (selector: string) (props: list<HtmlProp>)  =
    let nestedRules =
        props
        |> List.choose (function
            | Nested selector, prop-> Some((selector, unbox<list<HtmlProp>> prop))
            | _ -> None)
    let otherProps =
        props
        |> List.choose (function
            | Nested _,_ -> None
            | prop -> Some(prop))
    let thisRule = selector,otherProps
    
    let nestedRules =
        nestedRules
        |> List.map (fun (nested, props) ->
            let newSelector = $"{selector} {nested}"
            flattenRule newSelector props)
        |> List.concat
    thisRule :: nestedRules


let processRules (rules) =
    rules
    |> List.map (fun (selector, props) ->
        let flattenedRules = flattenRule selector props
        flattenedRules)
    |> List.concat
    |> List.map (fun (selector, props) ->
        let cssText = toCss selector props
        cssText)
    |> String.concat "\n"  // Add this line to concatenate all CSS rules
    

let testCssInFsharp () =
    // let cssText = toCss "test" rules
    // console.log ("cssText\n", cssText)

    console.log ("rules2", rules2 |> List.toArray)
    let cssRules = rules2 |> processRules
    printf "%A" cssRules

    let classMap =
        Map.empty |> Map.add "Header" "Header_0_1" |> Map.add "Main" "Main_0_2"

    // classMap["Header"]<-"Header_0_1";
    // classMap["Main"]<-"Main_0_2";

    console.log ("classMap", classMap)

    console.log ("Header", classMap["Header"])
    console.log ("Main", classMap["Main"])

    let otherClasses = createEmpty<obj>
    otherClasses?Header <- string "Header_0_1"
    otherClasses?Main <- "Main_0_2"

    console.log ("otherClasses", otherClasses)
    // Access with dynamic operator
    let f = otherClasses?Header
    console.log ("Header", string f)
    console.log ("Main", otherClasses?Main)
