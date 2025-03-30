module Demo.CompositionDemo

open Fable.React
open Browser
open Fable.Core.JS
open FreeAct.FreeFrame
open Fable.React.ReactBindings
open FreeAct
open Demo.FreeFrameDemo

// Subscription composition example
let SubscriptionCompositionExample () =
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

// Combined effects example
let EffectCompositionExample () =
    // Use combined effects to fetch todos and analyze them in parallel
    let isLoading, resultOpt = 
      useCombinedEffects
        fetchTodosEffect
        ()
        analyzeTodosEffect
        [{ id = 999; text = "Example task"; completed = false }] // Default analysis if fetch fails
        (fun todos analysis -> {| todos = todos; analysis = analysis |})
    
    div {
        className "effect-composition-example"
        h3 { "Effect Composition Example" }
        
        match isLoading, resultOpt with
        | true, _ -> 
            div { 
                className "loading"
                str "Loading and analyzing data simultaneously..." 
            }
        | false, Some (Ok combined) ->
            div {
                className "success"
                h4 { "Fetched Todos" }
                ul {
                    combined.todos |> List.map (fun todo ->
                        li {
                            key (string todo.id)
                            str todo.text
                        }
                    )
                }
                
                h4 { "Analysis Results" }
                p { str (sprintf "Total todos: %d" combined.analysis.totalCount) }
                p { str (sprintf "Completed: %d" combined.analysis.completedCount) }
                p { str (sprintf "Active: %d" combined.analysis.activeCount) }
                p { str (sprintf "Average text length: %.1f characters" combined.analysis.averageTextLength) }
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

// Chained effects example
let ChainedEffectsExample () =
    // Use chained effects: fetch todos, then prioritize them
    let isLoading, resultOpt = 
      useChainedEffect
        fetchTodosEffect
        ()
        id  // Pass the result directly
        prioritizeTodosEffect
    
    div {
        className "chained-effects-example"
        h3 { "Chained Effects Example" }
        
        match isLoading, resultOpt with
        | true, _ -> 
            div { 
                className "loading"
                str "Loading todos, then prioritizing them..." 
            }
        | false, Some (Ok prioritizedTodos) ->
              div {
                  className "success"
                  h4 { "Prioritized Todos" }
                  ol {
                      prioritizedTodos |> List.map (fun todo ->
                          li {
                              key (string todo.id)
                              className (if todo.completed then "completed-todo" else "active-todo")
                              str todo.text
                          }
                      )
                  }
                  p { 
                      str "Todos are sorted with active ones first, then alphabetically." 
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

// Main Composition demo component
let CompositionDemo () =
    // Initialize data when component mounts
    Hooks.useEffect((fun () ->
        initializeApp()
    ), [| |])
    
    div {
        className "composition-demo"
        h1 { "Composition Demo" }
        p { "This demo shows how to compose subscriptions and effects in FreeFrame." }
        
        div {
            className "composition-examples"
            SubscriptionCompositionExample()
            EffectCompositionExample()
            ChainedEffectsExample()
        }
    }
