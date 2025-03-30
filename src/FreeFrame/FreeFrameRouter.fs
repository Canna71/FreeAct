module FreeAct.FreeFrameRouter

open Fable.Core
open Browser
open Browser.Types
open Fable.React
open FreeAct
open FreeAct.Router
open FreeAct.FreeFrame

open System

// ==================================================
//           FreeFrame Router Integration
// ==================================================

/// Represents the router state stored in the application state
type RouterState<'Content> =
    {
        CurrentRoute: string
        CurrentPath: string
        PathParams: Map<string, string>
        QueryParams: Map<string, string list>
        Content: 'Content option
    }

/// Result of a route match with content
type RouteResult<'Content> =
    {
        MatchedRoute: string // The matched route pattern
        Path: string // The actual path that was matched
        PathParams: Map<string, string>
        QueryParams: Map<string, string list>
        Content: 'Content // The rendered content
    }

/// Events for router actions
type RouterEvent<'Content> =
    | RouteChanged of RouteResult<'Content>
    | NavigateTo of string
    | NavigateBack
    | NavigateForward

/// Router mode (hash-based or history API)
type RouterMode =
    | HashBased
    | HistoryAPI

/// Event IDs for router events
module RouterEvents =
    let routeChangedEvent<'Content> = EventId.auto<'Content> ()
    let navigateToEvent = EventId.auto ()
    let navigateBackEvent = EventId.auto ()
    let navigateForwardEvent = EventId.auto ()

/// Create a default router state
let createDefaultRouterState<'Content> () : RouterState<'Content> =
    {
        CurrentRoute = ""
        CurrentPath = ""
        PathParams = Map.empty
        QueryParams = Map.empty
        Content = None
    }

/// Register event handlers for router events
let registerRouterEvents<'Content, 'State>
    (getRouterState: 'State -> RouterState<'Content>)
    (setRouterState: RouterState<'Content> -> 'State -> 'State)
    =

    // Handle route changed events
    registerNamedEventHandler
        RouterEvents.routeChangedEvent
        (fun (RouteChanged result) state ->
            let currentRouter = getRouterState state

            let newRouterState =
                { currentRouter with
                    CurrentRoute = result.MatchedRoute
                    CurrentPath = result.Path
                    PathParams = result.PathParams
                    QueryParams = result.QueryParams
                    Content = Some result.Content
                }

            setRouterState newRouterState state
        )

    // Handle navigate to events
    registerNamedEventHandler
        RouterEvents.navigateToEvent
        (fun (path) state ->
            // Update browser URL but don't trigger another event - the popstate event will handle that
            window.history.pushState (null, "", path)
            state
        )

    // Handle navigate back events
    registerNamedEventHandler
        RouterEvents.navigateBackEvent
        (fun _ state ->
            window.history.back ()
            state
        )

    // Handle navigate forward events
    registerNamedEventHandler
        RouterEvents.navigateForwardEvent
        (fun _ state ->
            window.history.forward ()
            state
        )

/// Initialize the router with the FreeFrame state management
let initializeRouter<'Content, 'State>
    (appDb: IAppDb<'State>)
    (router: Router<'Content>)
    (getRouterState: 'State -> RouterState<'Content>)
    (setRouterState: RouterState<'Content> -> 'State -> 'State)
    (mode: RouterMode)
    (defaultContent: 'Content)
    =

    // Register event handlers
    registerRouterEvents getRouterState setRouterState

    // Handle browser navigation events (popstate)
    let handleLocationChange _ =
        let path =
            match mode with
            | RouterMode.HashBased ->
                let hash = window.location.hash

                if hash.StartsWith("#") then
                    hash.Substring(1)
                else
                    hash
            | RouterMode.HistoryAPI -> window.location.pathname + window.location.search

        // Use the router's Match method to find a matching route
        match router.Match(path) with
        | Some(matchResult, handler) ->
            // Get query parameters from the URL
            let parsedUrl = FreeAct.Tokenizer.parseUrl path

            // // Create a route match result
            // let matchResult = {
            //     PathParams = Map.empty  // Will be populated by the handler
            //     QueryParams = parsedUrl.QueryParams
            //     Fragment = parsedUrl.Fragment
            // }

            // Execute the handler to get the content
            let content = handler matchResult

            // // Create a result to dispatch
            let routeResult =
                {
                    MatchedRoute = matchResult.Pattern // We don't have direct access to the matched pattern
                    Path = path
                    PathParams = matchResult.PathParams
                    QueryParams = matchResult.QueryParams
                    Content = content
                }

            // Dispatch route changed event
            dispatch appDb RouterEvents.routeChangedEvent routeResult
        | None ->
            // Create a default route result for not found
            let notFoundResult =
                {
                    MatchedRoute = ""
                    Path = path
                    PathParams = Map.empty
                    QueryParams = Map.empty
                    Content = defaultContent
                }

            dispatch appDb RouterEvents.routeChangedEvent notFoundResult

    // Set up event listener for popstate
    window.addEventListener ("popstate", handleLocationChange)

    // Handle initial route
    handleLocationChange null

    // Return a function to clean up the event listener
    (fun () -> window.removeEventListener ("popstate", handleLocationChange))

/// Create a link component that uses the FreeFrame router
let FreeFrameLink<'State>
    (props:
        {|
            appDb: IAppDb<'State>
            destination: string
            className: string option
            children: ReactElement list
        |})
    =

    let handleClick (e: Browser.Types.MouseEvent) =
        e.preventDefault ()
        dispatch props.appDb RouterEvents.navigateToEvent props.destination

    a {
        href props.destination
        onClick handleClick

        className
            [
                match props.className with
                | Some className' -> yield className'
                | _ -> ()
            ]

        children props.children
    }

/// Create a router provider component that connects the FreeFrame router to React
let FreeFrameRouterProvider<'Content, 'State> =
    FunctionComponent.Of(fun
                             (props:
                                 {|
                                     AppDb: IAppDb<'State>
                                     Router: Router<'Content>
                                     GetRouterState: 'State -> RouterState<'Content>
                                     SetRouterState: RouterState<'Content> -> 'State -> 'State
                                     Mode: RouterMode
                                     DefaultContent: 'Content
                                     Children: ReactElement list
                                 |}) ->

        // Initialize router on component mount
        Hooks.useEffectDisposable (
            (fun () ->
                let cleanup =
                    initializeRouter
                        props.AppDb
                        props.Router
                        props.GetRouterState
                        props.SetRouterState
                        props.Mode
                        props.DefaultContent

                { new IDisposable with
                    member _.Dispose() = cleanup ()
                }
            ),
            [| box props.Router; box props.Mode |]
        )

        // Render children
        fragment { props.Children }
    )

/// Create a routes component that renders the current route content
let FreeFrameRoutes<'State> =
    FunctionComponent.Of(fun
                             (props:
                                 {|
                                     GetRouterState: 'State -> RouterState<ReactElement>
                                     AppDb: IAppDb<'State>
                                     DefaultContent: ReactElement
                                 |}) ->

        let subscription = createSubscription props.AppDb props.GetRouterState
        let routerState = useSubscription subscription

        // Render the current route content or default content
        match routerState.Content with
        | Some content -> content
        | None -> props.DefaultContent
    )

/// Utility function to navigate programmatically
let navigateTo<'State> (appDb: IAppDb<'State>) (path: string) =
    dispatch appDb RouterEvents.navigateToEvent path

/// Utility function to navigate back
let navigateBack<'State> (appDb: IAppDb<'State>) () =
    dispatch appDb RouterEvents.navigateBackEvent ()

/// Utility function to navigate forward
let navigateForward<'State> (appDb: IAppDb<'State>) () =
    dispatch appDb RouterEvents.navigateForwardEvent ()

/// Hook to access the current route information
let useRoute<'Content, 'State>
    (appDb: IAppDb<'State>)
    (getRouterState: 'State -> RouterState<'Content>)
    =
    let subscription = createSubscription appDb getRouterState
    useSubscription subscription
