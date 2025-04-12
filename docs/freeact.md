# FreeAct Core

FreeAct provides a clean DSL for creating React components in F#.

## Component Creation

```fsharp
let MyComponent() =
    div {
        className "container"
        
        h1 { 
            text "Hello World" 
        }
        
        p {
            className "description"
            text "Welcome to FreeAct"
        }
    }
```

## Props and Events

```fsharp
type MyProps = {
    title: string
    onSave: string -> unit
}

let MyComponent(props: MyProps) =
    div {
        h2 { text props.title }
        
        button {
            onClick (fun _ -> props.onSave "Clicked!")
            text "Save"
        }
    }
```

## Children and Composition

```fsharp
let Container(children: ReactElement list) =
    div {
        className "container"
        children children
    }

let App() =
    Container [
        h1 { text "Title" }
        p { text "Content" }
    ]
```
