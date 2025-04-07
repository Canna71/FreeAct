module Fss

open FreeAct
open Fable.Core.JS
open System.Text.RegularExpressions

type CssClassBuilder(name: string) =
    inherit StyleBuilder(name)

    member inline _.Run(props: HtmlProp list) : string * HtmlProp list =

        let ret = (name, props)
        ret

let css = CssClassBuilder

let rules =
    // css "root" {
    css "my" {
        flex
        backgroundColor "#f0f0f0"
    }
// }

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
    let cssText = toCss (fst rules) (snd rules)
    console.log ("cssText", cssText)
