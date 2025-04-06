module FreeAct.FreeFrameRouter

open Fable.Core
open Browser
open Browser.Types
open Fable.React
open FreeAct
open FreeAct.Router
open FreeFrame
open System

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
    | RouteChanged of path: string * result: RouteMatchResult option
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
                    | Some className -> yield className
                    | None -> ()
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

        // Register router event handler
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

                        // Match the route
                        let matchResult = props.Router.Match(path)

                        let routeResult =
                            match matchResult with
                            | Some(result, _) -> Some result
                            | None -> None

                        // Create a new router state
                        let currentRouter = props.GetRouterState state

                        // Update state based on match result
                        let newState =
                            match routeResult with
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
                                let newRouterState =
                                    { currentRouter with
                                        CurrentRoute = ""
                                        CurrentPath = path
                                        PathParams = Map.empty
                                        QueryParams = Map.empty
                                        Fragment = None
                                    }

                                props.SetRouterState newRouterState state

                        // Ensure subscribers are notified
                        props.AppDb.ForceRefresh()

                        newState

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

                    // Match the route
                    let matchResult = props.Router.Match(path)

                    let routeMatch =
                        match matchResult with
                        | Some(result, _) -> Some result
                        | None -> None

                    // Dispatch route changed event
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

                        // Match the route
                        let matchResult = props.Router.Match(path)

                        let routeMatch =
                            match matchResult with
                            | Some(result, _) -> Some result
                            | None -> None

                        // Dispatch route changed event
                        dispatchTyped<RouterEvent, 'State>
                            props.AppDb
                            (RouteChanged(path, routeMatch))

                // Add event listeners
                window.addEventListener ("popstate", handlePopState)
                window.addEventListener ("hashchange", handleHashChange)

                // Process the initial route
                let initialPath =
                    match props.Mode with
                    | HistoryAPI -> window.location.pathname + window.location.search
                    | HashBased ->
                        let hash = window.location.hash

                        if hash.StartsWith("#") then
                            hash.Substring(1)
                        else
                            "/"

                let matchResult = props.Router.Match(initialPath)

                let routeMatch =
                    match matchResult with
                    | Some(result, _) -> Some result
                    | None -> None

                dispatchTyped<RouterEvent, 'State>
                    props.AppDb
                    (RouteChanged(initialPath, routeMatch))

                // Cleanup function
                { new IDisposable with
                    member _.Dispose() =
                        window.removeEventListener ("popstate", handlePopState)
                        window.removeEventListener ("hashchange", handleHashChange)
                }
            ),
            [| box props.Mode |]
        )

        printfn "FreeFrameRouterProvider initialized with mode: %A" props.Mode
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
        console.log ("FreeFrameRoutes initialized with appDB: %A", props.AppDb)
        // Create a subscription to watch router state changes
        let routerState = useNewView props.AppDb props.GetRouterState

        // For debugging - log when route changes are detected
        Hooks.useEffect (
            (fun () ->
                console.log ("FreeFrameRoutes detected route change:", routerState.CurrentPath)
            ),
            [| box routerState.CurrentPath |]
        )

        // let h t = h1 { sprintf "FreeFrameRoutes %s" t }

        // Match the current path to a route handler
        match props.Router.Match(routerState.CurrentPath) with
        | Some(result, handler) ->
            // Create a route match result to pass to the handler
            // Render the matched route
            // let res = handler result
            let res = handler result
            console.log ("FreeFrameRoutes matched route result: ", res)
            fragment { res }

        | None -> props.DefaultContent
    // For debugging - show current date and time
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
    useNewView appDb getRouterState
