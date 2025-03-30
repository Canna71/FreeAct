module Demo.EffectsDemo

open Fable.React
open Browser
open Fable.Core.JS
open FreeAct.FreeFrame
open Fable.React.ReactBindings
open FreeAct
open Demo.FreeFrameDemo

// Example component with Effects
let EffectSimpleExample () =
    // Use the F# idiomatic hook for effects
    let isLoading, result = useEffect fetchTodosEffect ()
    
    div {
        className "effect-example"
        h3 { "Effect Example (Simple)" }
        
        match isLoading, result with
        | true, _ -> 
            div { 
                className "loading"
                str "Loading data..." 
            }
        | false, Some (Ok todos) ->
            div {
                className "success"
                str (sprintf "Successfully loaded %d items" todos.Length)
                ul {
                    todos |> List.map (fun todo ->
                        li {
                            key (string todo.id)
                            str todo.text
                        }
                    )
                }
            }
        | false, Some (Error err) ->
            div {
                className "error"
                str (sprintf "Error: %s" err.Message)
            }
        | false, None ->
            div {
                className "none"
                str "No data loaded yet."
            }
    }

// Example using the union-based loading state
let EffectWithUnionStateExample () =
    // Define how to map the result to our union state
    let mapResultToState (result: Result<TodoItem list, exn>) =
        match result with
        | Ok todos -> Loaded todos
        | Error err -> Failed err.Message
    
    // Use the new hook with our union state
    let loadingState, resultOpt = 
        useEffectWithUnionState 
            fetchTodosEffect
            ()
            NotStarted  // Initial state
            mapResultToState  // Result mapping function
    
    div {
        className "effect-example-union"
        h3 { "Effect Example (Union Loading State)" }
        
        match loadingState with
        | NotStarted -> 
            div { 
                className "not-started"
                str "Effect not started yet" 
            }
        | Loading -> 
            div { 
                className "loading"
                str "Loading data..." 
            }
        | Loaded todos ->
            div {
                className "success"
                str (sprintf "Successfully loaded %d items" todos.Length)
                ul {
                    todos |> List.map (fun todo ->
                        li {
                            key (string todo.id)
                            str todo.text
                        }
                    )
                }
            }
        | Failed errorMsg ->
            div {
                className "error"
                str (sprintf "Error: %s" errorMsg)
            }
    }

// Main Effects demo component
let EffectsDemo () =    
    div {
        className "effects-demo"
        h1 { "Effects Demo" }
        p { "This demo demonstrates different ways to handle effects in FreeFrame." }
        
        div {
            className "effects-examples"
            EffectSimpleExample()
            EffectWithUnionStateExample()
        }
    }
