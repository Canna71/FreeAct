module FreeFrame.Router

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
    | RouteChanged of path: string * result: MatchedRoute<ReactElement> option
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

let rec extractRouteState (matched: MatchedRoute<'T>) =
    let baseState =
        {
            CurrentRoute = matched.Result.Pattern
            CurrentPath = matched.Result.Pattern
            PathParams = matched.Result.PathParams
            QueryParams = matched.Result.QueryParams
            Fragment = matched.Result.Fragment
        }

    match matched.Child with
    | Some child ->
        let childState = extractRouteState child

        { baseState with
            CurrentPath = matched.Result.Pattern + "/" + childState.CurrentPath.TrimStart('/') // Build full path
            PathParams =
                Map.fold
                    (fun acc key value -> Map.add key value acc)
                    baseState.PathParams
                    childState.PathParams
            QueryParams =
                Map.fold
                    (fun acc key value -> Map.add key value acc)
                    baseState.QueryParams
                    childState.QueryParams
        }
    | None -> baseState

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
                    | RouteChanged(path, _) ->
                        match props.Router.Match(path) with
                        | Some matched ->
                            // Update router state from complete matched route (including nested)
                            let newRouterState = extractRouteState matched
                            props.SetRouterState newRouterState state
                        | None ->
                            props.SetRouterState
                                { createDefaultRouterState () with CurrentPath = path }
                                state

                    | NavigateTo path ->
                        window.history.pushState (null, "", path)

                        match props.Router.Match(path) with
                        | Some matched ->
                            let newState = props.SetRouterState (extractRouteState matched) state
                            props.AppDb.ForceRefresh()
                            newState
                        | None ->
                            let newState =
                                props.SetRouterState
                                    { createDefaultRouterState () with CurrentPath = path }
                                    state

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

                    // Match the route and extract route result from complete match
                    match props.Router.Match(path) with
                    | Some matched ->
                        dispatchTyped<RouterEvent, 'State>
                            props.AppDb
                            (RouteChanged(path, Some matched))
                    | None ->
                        dispatchTyped<RouterEvent, 'State> props.AppDb (RouteChanged(path, None))

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

                        // Dispatch route changed event
                        dispatchTyped<RouterEvent, 'State>
                            props.AppDb
                            (RouteChanged(path, matchResult))

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

                dispatchTyped<RouterEvent, 'State>
                    props.AppDb
                    (RouteChanged(initialPath, matchResult))

                // Cleanup function
                { new IDisposable with
                    member _.Dispose() =
                        window.removeEventListener ("popstate", handlePopState)
                        window.removeEventListener ("hashchange", handleHashChange)
                }
            ),
            [| box props.Mode |]
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
        let routerState = useNewView props.AppDb props.GetRouterState

        let rec renderMatch (matched: MatchedRoute<ReactElement>) : ReactElement =

            match matched.Child with
            | Some child ->
                let childContent = renderMatch child
                matched.Handler { Result = matched.Result; ChildContent = Some childContent }
            | None -> matched.Handler { Result = matched.Result; ChildContent = None }

        match props.Router.Match(routerState.CurrentPath) with
        | Some matched -> renderMatch matched
        | None ->
            console.log ("No match found for path:", routerState.CurrentPath)
            props.DefaultContent
    )

// Update event handler
let handleRouteChange (router: Router<ReactElement>) (path: string) = router.Match(path)

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
