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
type RouterEvent =
    | RouteChanged of string // Just pass the path here, we'll resolve it later
    | NavigateTo of string
    | NavigateBack
    | NavigateForward

/// Router mode (hash-based or history API)
type RouterMode =
    | HashBased
    | HistoryAPI

/// Create a default router state
let createDefaultRouterState<'Content> () : RouterState<'Content> =
    {
        CurrentRoute = ""
        CurrentPath = ""
        PathParams = Map.empty
        QueryParams = Map.empty
        Content = None
    }

/// Register event handlers for router events within a React component
let setupRouterState<'Content, 'State>
    (appDb: IAppDb<'State>)
    (router: Router<'Content>)
    (getRouterState: 'State -> RouterState<'Content>)
    (setRouterState: RouterState<'Content> -> 'State -> 'State)
    (mode: RouterMode)
    (defaultContent: 'Content)
    =

    // Handle a route change by processing the route and updating state
    let handleRouteChange path =
        let (matchResult, content) =
            match router.Match(path) with
            | Some(result, handler) ->
                // Execute the handler to get the content
                let content = handler result
                (result, content)
            | None ->
                // Create default result for not found
                let parsedUrl = Tokenizer.parseUrl path

                let result: Tokenizer.RouteMatchResult =
                    {
                        Pattern = ""
                        PathParams = Map.empty
                        QueryParams = parsedUrl.QueryParams
                        Fragment = parsedUrl.Fragment
                    }

                (result, defaultContent)

        // Create a route result to update state
        let routeResult =
            {
                MatchedRoute = matchResult.Pattern
                Path = path
                PathParams = matchResult.PathParams
                QueryParams = matchResult.QueryParams
                Content = content
            }

        // Update router state via reducer
        appDb.Dispatch(fun (state: 'State) ->
            let currentRouter = getRouterState state

            let newRouterState =
                { currentRouter with
                    CurrentRoute = routeResult.MatchedRoute
                    CurrentPath = routeResult.Path
                    PathParams = routeResult.PathParams
                    QueryParams = routeResult.QueryParams
                    Content = Some routeResult.Content
                }

            setRouterState newRouterState state
        )

    // Register handler for route changed events
    registerTypedEventHandler<RouterEvent, 'State> (fun event state ->
        match event with
        | RouteChanged path ->
            // Route changes are handled separately to avoid recursive updates
            // We just return the state unchanged here
            state
        | NavigateTo path ->
            // Update browser URL
            window.history.pushState (null, "", path)
            // We'll handle the actual route change via the popstate event
            state
        | NavigateBack ->
            window.history.back ()
            state
        | NavigateForward ->
            window.history.forward ()
            state
    )

    // Return the handler function so it can be used within useEffect
    handleRouteChange

/// Create a link component that uses the FreeFrame router
let FreeFrameLink<'State> =
    FunctionComponent.Of(fun
                             (props:
                                 {|
                                     appDb: IAppDb<'State>
                                     destination: string
                                     className: string option
                                     children: ReactElement list
                                 |}) ->

        let handleClick (e: Browser.Types.MouseEvent) =
            e.preventDefault ()
            dispatchTyped<RouterEvent, 'State> props.appDb (NavigateTo props.destination)

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
    )

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

        // Set up the router state and get the handler for route changes
        let handleRouteChange =
            setupRouterState
                props.AppDb
                props.Router
                props.GetRouterState
                props.SetRouterState
                props.Mode
                props.DefaultContent

        // Keep track of the current URL for rendering
        let initialUrl =
            match props.Mode with
            | HistoryAPI -> window.location.pathname + window.location.search
            | HashBased ->
                let hash = window.location.hash

                if hash.StartsWith("#") then
                    hash.Substring(1)
                else
                    "/"

        let urlState = Hooks.useState initialUrl

        // Handle browser navigation events
        Hooks.useEffectDisposable (
            (fun () ->
                // Function to handle popstate events
                let handlePopState _ =
                    let path =
                        match props.Mode with
                        | HistoryAPI -> window.location.pathname + window.location.search
                        | HashBased ->
                            let hash = window.location.hash

                            if hash.StartsWith("#") then
                                hash.Substring(1)
                            else
                                "/"

                    // Update our local state
                    urlState.update path
                    // Process the route change
                    handleRouteChange path

                // Function to handle hashchange events
                let handleHashChange _ =
                    if props.Mode = HashBased then
                        let hash = window.location.hash

                        let path =
                            if hash.StartsWith("#") then
                                hash.Substring(1)
                            else
                                "/"

                        // Update our local state
                        urlState.update path
                        // Process the route change
                        handleRouteChange path

                // Add event listeners
                window.addEventListener ("popstate", handlePopState)
                window.addEventListener ("hashchange", handleHashChange)

                // Process the initial route
                handleRouteChange urlState.current

                // Cleanup function

                { new IDisposable with
                    member _.Dispose() =
                        // Remove event listeners on cleanup
                        window.removeEventListener ("popstate", handlePopState)
                        window.removeEventListener ("hashchange", handleHashChange)
                }

            ),
            [| box props.Mode; box props.Router |]
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
    dispatchTyped<RouterEvent, 'State> appDb (NavigateTo path)

/// Utility function to navigate back
let navigateBack<'State> (appDb: IAppDb<'State>) () =
    dispatchTyped<RouterEvent, 'State> appDb NavigateBack

/// Utility function to navigate forward
let navigateForward<'State> (appDb: IAppDb<'State>) () =
    dispatchTyped<RouterEvent, 'State> appDb NavigateForward

/// Hook to access the current route information
let useRoute<'Content, 'State>
    (appDb: IAppDb<'State>)
    (getRouterState: 'State -> RouterState<'Content>)
    =
    let subscription = createSubscription appDb getRouterState
    useSubscription subscription
