module testbuilder

type MyBuilder() =
    member this.Return(x) = x
    member this.Zero() = ()
    
let my = MyBuilder()

