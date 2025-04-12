module FreeAct.ReactRouter

open Fable.React
open Fable.React.Helpers
open Browser
open Browser.Types
open Fable.Core
open Fable.Core.JsInterop
open FreeAct
open FreeActRouter

type RouterMode =
    | HistoryAPI
    | HashBased

/// Context provider for the router
type RouterContextInfo =
    {
        Router: Router<ReactElement>
        CurrentUrl: string
        Navigate: string -> unit
        ChildRenderer: (unit -> ReactElement) option
    }

/// Create a React context for the router
let private createRouterContext<'T> () = createContext (None)

/// Global router context instance
let RouterContext = createRouterContext ()

/// Router provider component that sets up routing and navigation
type RouterProviderProps =
    {
        Router: Router<ReactElement>
        Mode: RouterMode
        DefaultContent: ReactElement
        Children: ReactElement list
    }

/// Create a router provider component
let RouterProvider =
    FunctionComponent.Of(fun (props: RouterProviderProps) ->

        let initialUrl =
            match props.Mode with
            | HistoryAPI -> window.location.pathname + window.location.search
            | HashBased ->
                let hash = window.location.hash

                if hash.StartsWith("#") then
                    hash.Substring(1)
                else
                    "/"

        let state = Hooks.useState (initialUrl)

        let navigate (url: string) =
            match props.Mode with
            | HistoryAPI ->
                history.pushState (null, "", url)
                state.update (url)
            | HashBased ->
                window.location.hash <-
                    if url.StartsWith("/") then
                        "#" + url
                    else
                        "#/" + url

        // Set up event listeners
        Hooks.useEffect (
            fun () ->
                let handlePopState _ =
                    match props.Mode with
                    | HistoryAPI ->
                        state.update (window.location.pathname + window.location.search)
                    | HashBased ->
                        let hash = window.location.hash

                        state.update (
                            if hash.StartsWith("#") then
                                hash.Substring(1)
                            else
                                "/"
                        )

                let handleHashChange _ =
                    if props.Mode = HashBased then
                        let hash = window.location.hash

                        state.update (
                            if hash.StartsWith("#") then
                                hash.Substring(1)
                            else
                                "/"
                        )

                // Add event listeners
                window.addEventListener ("popstate", handlePopState)
                window.addEventListener ("hashchange", handleHashChange)

                // Cleanup function
                let cleanup =
                    window.removeEventListener ("popstate", handlePopState)
                    window.removeEventListener ("hashchange", handleHashChange)

                unbox cleanup

            , [| box props.Mode |]
        )

        // Router context value
        let routerContext =
            {
                Router = props.Router
                CurrentUrl = state.current
                Navigate = navigate
                ChildRenderer = None
            }

        // console.log props.Children
        // Return the context provider with children
        // ofImport "Provider" "react" {| value = Some routerContext; children = props.Children |> List.toArray |} []
        contextProvider RouterContext (Some routerContext) (props.Children |> List.toArray)
    )

/// Routes component that renders the matched route
let Routes =
    FunctionComponent.Of(fun (props: {| DefaultContent: ReactElement |}) ->
        console.log "Routes component rendering..."
        let routerContext = Hooks.useContext (RouterContext)

        match routerContext with
        | None ->
            printfn "Router context not found. Make sure to wrap your app with RouterProvider."

            div {
                className "router-error"
                str "Router context not found. Make sure to wrap your app with RouterProvider."
            }
        | Some context ->
            let rec renderMatch (m: MatchedRoute<ReactElement>) : ReactElement =
                match m.Child with
                | Some child ->
                    let childContent = renderMatch child
                    m.Handler { Result = m.Result; ChildContent = Some childContent }
                | None -> m.Handler { Result = m.Result; ChildContent = None }

            match context.Router.Match(context.CurrentUrl) with
            | Some matched -> renderMatch matched
            | None -> props.DefaultContent
    )

/// Link component that works with the router
let Link =
    FunctionComponent.Of(fun
                             (props:
                                 {|
                                     destination: string
                                     className: string option
                                     children: ReactElement list
                                 |}) ->
        let routerContext = Hooks.useContext (RouterContext)

        match routerContext with
        | None ->
            a {
                href props.destination

                className (
                    match props.className with
                    | Some c -> c
                    | None -> ""
                )

                children props.children
            }
        | Some context ->
            a {
                href props.destination

                className (
                    match props.className with
                    | Some c -> c
                    | None -> ""
                )

                onClick (fun e ->
                    e.preventDefault ()
                    context.Navigate props.destination
                )

                children props.children
            }
    )

/// Hook to get the router navigation function
let useNavigate<'T> () =
    let routerContext = Hooks.useContext (RouterContext)

    match routerContext with
    | Some context -> context.Navigate
    | None ->
        printfn "Router context not found. Make sure to wrap your app with RouterProvider."
        fun _ -> ()

/// Hook to get the current URL
let useLocation<'T> () =
    let routerContext = Hooks.useContext (RouterContext)

    match routerContext with
    | Some context -> context.CurrentUrl
    | None ->
        printfn "Router context not found. Make sure to wrap your app with RouterProvider."
        ""
