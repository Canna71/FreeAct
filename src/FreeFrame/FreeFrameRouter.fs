module FreeAct.FreeFrameRouter

open Fable.Core
open Browser
open Browser.Types
open Fable.React
open FreeAct
open FreeAct.Router
open FreeAct.FreeFrame
open System
open FreeAct.Tokenizer

// ==================================================
//           FreeFrame Router Integration
// ==================================================

/// Represents the router state stored in the application state
type RouterState =
    {
        CurrentRoute: string
        CurrentPath: string
        PathParams: Map<string, string>
        QueryParams: Map<string, string list>
        Fragment: string option
    }

/// Events for router actions
type RouterEvent =
    | RouteChanged of path: string * result: Tokenizer.RouteMatchResult option
    | NavigateTo of string
    | NavigateBack
    | NavigateForward

/// Router mode (hash-based or history API)
type RouterMode =
    | HashBased
    | HistoryAPI

/// Create a default router state
let createDefaultRouterState () : RouterState =
    {
        CurrentRoute = ""
        CurrentPath = ""
        PathParams = Map.empty
        QueryParams = Map.empty
        Fragment = None
    }

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
let FreeFrameRouterProvider<'State> =
    FunctionComponent.Of(fun
                             (props:
                                 {|
                                     AppDb: IAppDb<'State>
                                     Router: Router<ReactElement>
                                     GetRouterState: 'State -> RouterState
                                     SetRouterState: RouterState -> 'State -> 'State
                                     Mode: RouterMode
                                     Children: ReactElement list
                                 |}) ->

        // Register handler for route events if not already done
        Hooks.useEffect (
            (fun () ->
                // Register handler for router events
                registerTypedEventHandler<RouterEvent, 'State> (fun event state ->
                    match event with
                    | RouteChanged(path, routeMatchOpt) ->
                        let currentRouter = props.GetRouterState state

                        match routeMatchOpt with
                        | Some routeMatch ->
                            let newRouterState =
                                { currentRouter with
                                    CurrentRoute = routeMatch.Pattern
                                    CurrentPath = path
                                    PathParams = routeMatch.PathParams
                                    QueryParams = routeMatch.QueryParams
                                    Fragment = routeMatch.Fragment
                                }

                            props.SetRouterState newRouterState state
                        | None ->
                            // If no match, just update the path
                            let newRouterState =
                                { currentRouter with
                                    CurrentRoute = ""
                                    CurrentPath = path
                                    PathParams = Map.empty
                                    QueryParams = Map.empty
                                    Fragment = None
                                }

                            props.SetRouterState newRouterState state
                    | NavigateTo path ->
                        // Update browser URL
                        window.history.pushState (null, "", path)

                        // Also trigger a route change event
                        let matchResult = props.Router.Match(path)

                        let routeResult =
                            match matchResult with
                            | Some(result, _) -> Some result
                            | None -> None

                        dispatchTyped<RouterEvent, 'State>
                            props.AppDb
                            (RouteChanged(path, routeResult))

                        // Return unchanged state since we'll handle the change via the RouteChanged event
                        state
                    | NavigateBack ->
                        window.history.back ()
                        state
                    | NavigateForward ->
                        window.history.forward ()
                        state
                )
            ),
            [||] // Empty dependency array - only run once
        )

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

                    // Dispatch route changed event
                    let matchResult = props.Router.Match(path)

                    let routeMatch =
                        match matchResult with
                        | Some(result, _) -> Some result
                        | None -> None

                    dispatchTyped<RouterEvent, 'State>
                        props.AppDb
                        (RouteChanged(path, routeMatch))

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

                        // Dispatch route changed event
                        let matchResult = props.Router.Match(path)

                        let routeMatch =
                            match matchResult with
                            | Some(result, _) -> Some result
                            | None -> None

                        dispatchTyped<RouterEvent, 'State>
                            props.AppDb
                            (RouteChanged(path, routeMatch))

                // Add event listeners
                window.addEventListener ("popstate", handlePopState)
                window.addEventListener ("hashchange", handleHashChange)

                // Process the initial route
                let path = urlState.current
                let matchResult = props.Router.Match(path)

                let routeMatch =
                    match matchResult with
                    | Some(result, _) -> Some result
                    | None -> None

                dispatchTyped<RouterEvent, 'State> props.AppDb (RouteChanged(path, routeMatch))

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
        fragment { children props.Children }
    )

/// Create a routes component that renders the current route content
let FreeFrameRoutes<'State> =
    FunctionComponent.Of(fun
                             (props:
                                 {|
                                     Router: Router<ReactElement>
                                     GetRouterState: 'State -> RouterState
                                     AppDb: IAppDb<'State>
                                     DefaultContent: ReactElement
                                 |}) ->

        let subscription = createSubscription props.AppDb props.GetRouterState
        let routerState = useSubscription subscription

        // Match the current path and render the appropriate component
        match props.Router.Match(routerState.CurrentPath) with
        | Some(_, handler) ->
            // Create a fresh route match result to pass to the handler
            let result =
                {
                    Pattern = routerState.CurrentRoute
                    PathParams = routerState.PathParams
                    QueryParams = routerState.QueryParams
                    Fragment = routerState.Fragment
                }

            handler result
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
let useRoute<'State> (appDb: IAppDb<'State>) (getRouterState: 'State -> RouterState) =
    let subscription = createSubscription appDb getRouterState
    useSubscription subscription
