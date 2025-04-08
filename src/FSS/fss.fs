module Fss

open FreeAct
open Fable.Core.JS
open System.Text.RegularExpressions

type CssClassBuilder() =
    inherit StyleBuilder()

    member inline _.Run(props: HtmlProp list) : HtmlProp list =

        let ret = props
        ret

let css = CssClassBuilder()

let rules =
    // css "root" {
    css {
        flex
        backgroundColor "#f0f0f0"
    }
// }

let rules2 =
    {|
        main =
            css {
                flex
                backgroundColor "#f0f0f0"
            }
        header =
            css {
                flex
                backgroundColor "#f0f0f0"
            }
    |}

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
    let cssText = toCss "test" rules
    console.log ("cssText\n", cssText)

    console.log ("rules2", rules2)
