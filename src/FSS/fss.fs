module Fss

open FreeAct
open Fable.Core.JS
open System.Text.RegularExpressions

type CssRule =
    | CssRule of HtmlProp
    | NestedRule of string * CssRule list

let NESTED_MARKER = "$$$."

// Define a partial active pattern for prefix matching
let (|Nested|_|) (s: string) =
    if s.StartsWith(NESTED_MARKER) then
        Some(s.Substring NESTED_MARKER.Length)
    else
        None

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
        let processed =
            props
            |> List.map (fun (k, v) ->
                console.log ("CssClassBuilder Run match ", selector, k, v)

                match k with
                | Nested k -> NestedRule(k, unbox v)
                | _ -> CssRule(k, v)
            )

        let ret = NestedRule(selector, processed)
        console.log ("CssClassBuilder Run", selector, ret)
        ret

let css = CssClassBuilder

// let rules =
//     // css "root" {
//     css "root" {
//         flex
//         backgroundColor "#f0f0f0"
//     }
// // }

let rules2 =
    [

        css "main" {
            flex
            backgroundColor "#f0f0f0"

            css "nested" { color "#f0f0f0" }
        }

        css "header" {
            grid
            margin (Length.Units(10, Unit.Px))
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
        |> fun x -> cssText + x + "\n}"

    cssText

let rec CssRulesToString (rules: CssRule list) =
    let cssText =
        rules
        |> List.map (fun rule ->
            match rule with
            | CssRule(k, v) -> $"{k}: {v};"
            | NestedRule(selector, rules) ->
                let nestedCssText = CssRulesToString rules
                $"{selector} {{\n{nestedCssText}\n}}"
        )
        |> String.concat "\n"

    cssText

let testCssInFsharp () =
    // console.log ("rules", rules)
    // let cssText = toCss "test" rules
    // console.log ("cssText\n", cssText)

    console.log ("rules2", rules2 |> List.toArray)
    console.log (rules2 |> CssRulesToString)
