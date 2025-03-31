module FreeAct.Interop

open Fable.Core
open Fable.React
open Fable.Core.JsInterop

type IReactProperty = interface end

// [<Import("ReactElement", "react")>]
// [<AllowNullLiteral>]
// type ReactElement = interface end
// Represents a generic CSSUnit
type ICSSUnit = interface end

[<Import("createElement", "react")>]
[<Emit "$0.apply(null, [$1, $2, ...$3])">]
let inline reactElementApply (name: obj) (props: 'a) (nested: #seq<ReactElement>) : ReactElement =
    jsNative

let inline createElement
    (name: obj)
    (properties: IReactProperty list)
    (children: ReactElement list)
    : ReactElement
    =
    let props: obj = createObj !!properties
    // let nested = emitJsExpr (props) "$0.nested || []"
    emitJsStatement (props) "delete $0.nested"
    reactElementApply name props children
