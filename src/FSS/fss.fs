module Fss

open FreeAct
open Fable.Core.JS

type CssClassBuilder(name: string) =
    inherit StyleBuilder(name)

let css = CssClassBuilder

let rules =
    css "my" {
        flex
        backgroundColor "#f0f0f0"
    }

let testCssInFsharp () = console.log ("rules", rules)
