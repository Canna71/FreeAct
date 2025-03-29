module FreeAct.ReactRouter

open Fable.React
open Fable.React.Helpers
open Browser
open Browser.Types
open Fable.Core
open Fable.Core.JsInterop
open FreeAct
open FreeAct.Router

type RouterMode =
    | HistoryAPI
    | HashBased

/// Context provider for the router
type RouterContext<'T> =
    {
        Router: Router<'T>
        CurrentUrl: string
        Navigate: string -> unit
    }

/// Create a React context for the router
let private createRouterContext<'T> () =
    ReactBindings.React.createContext<RouterContext<'T> option> (None)

/// Global router context instance
let RouterContext<'T> = createRouterContext<'T> ()

/// Router provider component that sets up routing and navigation
type RouterProviderProps<'T> =
    {
        Router: Router<'T>
        Mode: RouterMode
        DefaultContent: 'T
        Children: ReactElement list
    }

/// Create a router provider component
let RouterProvider<'T> (props: RouterProviderProps<'T>) =
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
                | HistoryAPI -> state.update (window.location.pathname + window.location.search)
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
        }

    // Return the context provider with children
    ofImport "Provider" "react" {| value = Some routerContext; children = props.Children |} []

/// Routes component that renders the matched route
let Routes<'T> (props: {| DefaultContent: 'T |}) =
    let routerContext = Hooks.useContext (RouterContext<'T>)

    match routerContext with
    | None ->
        printfn "Router context not found. Make sure to wrap your app with RouterProvider."
        null
    | Some context ->
        match context.Router.Match(context.CurrentUrl) with
        | Some(result, handler) -> handler result |> ofType
        | None -> props.DefaultContent |> ofType

/// Link component that works with the router
let Link
    (props:
        {|
            destination: string
            className: string option
            children: ReactElement list
        |})
    =
    let routerContext = Hooks.useContext (RouterContext<ReactElement>)

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

/// Hook to get the router navigation function
let useNavigate<'T> () =
    let routerContext = Hooks.useContext (RouterContext<'T>)

    match routerContext with
    | Some context -> context.Navigate
    | None ->
        printfn "Router context not found. Make sure to wrap your app with RouterProvider."
        fun _ -> ()

/// Hook to get the current URL
let useLocation<'T> () =
    let routerContext = Hooks.useContext (RouterContext<'T>)

    match routerContext with
    | Some context -> context.CurrentUrl
    | None ->
        printfn "Router context not found. Make sure to wrap your app with RouterProvider."
        ""
