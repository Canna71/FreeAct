module testbuilder

type MyBuilder() =
    // member this.Bind(x, f) = f x
    member inline this.Return(x) = x
    member inline this.Zero() = ()

    [<CustomOperation("empty")>]
    member inline this.Empty(_) = ()

    member inline this.Yield(x) = x
    
let my = MyBuilder()

