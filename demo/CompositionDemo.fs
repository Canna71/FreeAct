module Demo.CompositionDemo

open Fable.React
open Browser
open Fable.Core.JS
open FreeAct.FreeFrame
open Fable.React.ReactBindings
open FreeAct
open Demo.FreeFrameDemo

// Subscription composition example
let _SubscriptionCompositionExample () =
    // Create two more specialized subscriptions from our existing ones
    let completedTodosSubscription = mapSubscription todosSubscription (fun todos -> 
        todos |> List.filter (fun t -> t.completed))
    
    let activeTodosSubscription = mapSubscription todosSubscription (fun todos -> 
        todos |> List.filter (fun t -> not t.completed))
    
    // Combine them to get a summary subscription
    let summarySubscription = 
      combineSubscriptions 
        completedTodosSubscription 
        activeTodosSubscription
        (fun completed active -> 
            {| completed = completed.Length; active = active.Length |})
    
    // Use the combined subscription
    let summary = useSubscription summarySubscription
    
    div {
        className "subscription-composition-example"
        h3 { "Subscription Composition Example" }
        
        div {
            className "todos-summary"
            p {
                str (sprintf "Active tasks: %d" summary.active)
            }
            p {
                str (sprintf "Completed tasks: %d" summary.completed)
            }
            p {
                str (sprintf "Total: %d" (summary.active + summary.completed))
            }
        }
    }

let SubscriptionCompositionExample = FunctionComponent.Of _SubscriptionCompositionExample

let chainedEffectId = 
  registerNamedEffectHandler
    "ChainedEffect" 
    (fun () -> 
        printfn "Chained effect started"
        let combined = chainEffect fetchTodosEffect () id analyzeTodosEffect
        async {
            let! analysis = combined
            match analysis with
            | Ok analysis ->
              dispatch appDb setTodoAnalysisEvent (Loaded analysis)
            | _ ->
              dispatch appDb setTodoAnalysisEvent  (Failed "Failed to analyze todos")
        }      
    )
// Combined effects example
let _EffectChainingExample () =

    // Use combined effects to fetch todos and analyze them in parallel
    let analysys = useSubscription todosAnalysisSubscription
    
    console.log("EffectCompositionExample: ", analysys)

    let onStart (_) = 
      printfn "Starting effect chaining"
      runEffectAsync chainedEffectId () |> ignore
     
    
    div {
        className "effect-chaining-example"
        h3 { "Effect chaining Example" }
        button {
            className "start-button"
            onClick onStart
            str "Start Loading and Analyzing"
        }
        match analysys with
        | Loading -> 
            div { 
                className "loading"
                str "Loading and analyzing data simultaneously..." 
            }
        | Loaded analysis ->
                
                h4 { "Analysis Results" }
                p { str (sprintf "Total todos: %d" analysis.totalCount) }
                p { str (sprintf "Completed: %d" analysis.completedCount) }
                p { str (sprintf "Active: %d" analysis.activeCount) }
                p { str (sprintf "Average text length: %.1f characters" analysis.averageTextLength) }
            
        | Failed err ->
            div {
                className "error"
                str (sprintf "Error: %s" err)
            }
        | NotStarted ->
            div {
                className "none"
                str "No data loaded yet."
            }
    }

let EffectChainingExample = FunctionComponent.Of _EffectChainingExample

// Chained effects example
// let ChainedEffectsExample () =
//     // Use chained effects: fetch todos, then prioritize them
//     let isLoading, resultOpt = 
//       useChainedEffect
//         fetchTodosEffect
//         ()
//         id  // Pass the result directly
//         prioritizeTodosEffect
    
//     div {
//         className "chained-effects-example"
//         h3 { "Chained Effects Example" }
        
//         match isLoading, resultOpt with
//         | true, _ -> 
//             div { 
//                 className "loading"
//                 str "Loading todos, then prioritizing them..." 
//             }
//         | false, Some (Ok prioritizedTodos) ->
//               div {
//                   className "success"
//                   h4 { "Prioritized Todos" }
//                   ol {
//                       prioritizedTodos |> List.map (fun todo ->
//                           li {
//                               key (string todo.id)
//                               className (if todo.completed then "completed-todo" else "active-todo")
//                               str todo.text
//                           }
//                       )
//                   }
//                   p { 
//                       str "Todos are sorted with active ones first, then alphabetically." 
//                   }
//               }
//         | false, Some (Error err) ->
//             div {
//                 className "error"
//                 str (sprintf "Error: %s" err.Message)
//             }
//         | false, None ->
//             div {
//                 className "none"
//                 str "No data loaded yet."
//             }
//     }

// Main Composition demo component
let _CompositionDemo () =
    // Initialize data when component mounts
    Hooks.useEffect((fun () ->
        initializeApp()
    ), [| |])
    console.log("CompositionDemo: ", appDb)
    div {
        className "composition-demo"
        h1 { "Composition Demo" }
        p { "This demo shows how to compose subscriptions and effects in FreeFrame." }
        
        div {
            className "composition-examples"
            SubscriptionCompositionExample()
            EffectChainingExample()
            // ChainedEffectsExample()
        }
    }

let CompositionDemo = FunctionComponent.Of _CompositionDemo