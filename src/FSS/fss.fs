module Fss

open FreeAct
open Fable.Core.JS
open System.Text.RegularExpressions

let NESTED_MARKER = "$_$_"

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

let rules =
    // css "root" {
    css "root" {
        flex
        backgroundColor "#f0f0f0"
    }
// }

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
        |> fun x -> cssText + x + "\n}"

    cssText

let testCssInFsharp () =
    console.log ("rules", rules)
    // let cssText = toCss "test" rules
    // console.log ("cssText\n", cssText)

    console.log ("rules2", rules2 |> List.toArray)
