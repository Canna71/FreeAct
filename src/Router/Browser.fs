module FreeAct.Router.BrowserRouter

open FreeAct.Tokenizer
open System
open Browser.Dom
open Browser.Types
// open Browser.Navigation

// Re-export the core router types
type RouteMatchResult = FreeAct.Tokenizer.RouteMatchResult

/// Configuration options for the browser router
type BrowserRouterConfig<'T> =
    {
        /// The router instance
        Router: FreeAct.Router.Router<'T>

        /// Function to render the application with the matched route result
        Render: option<'T> -> unit

        /// Base path for the application (e.g., "/app" if hosted at example.com/app/)
        BasePath: string

        /// Whether to use hash-based routing (#/path) instead of history API
        UseHashRouting: bool

        /// Function to handle 404 (not found) routes
        NotFoundHandler: option<unit -> 'T>
    }

/// Browser router state
type BrowserRouter<'T> = { Config: BrowserRouterConfig<'T>; mutable CurrentLocation: string }

/// Creates a new browser router
let create (config: BrowserRouterConfig<'T>) =
    let router = { Config = config; CurrentLocation = "" }

    // Initialize with default values if not provided
    let config =
        { config with
            BasePath =
                if String.IsNullOrEmpty(config.BasePath) then
                    ""
                else
                    config.BasePath.TrimEnd('/')
            UseHashRouting = config.UseHashRouting
            NotFoundHandler = config.NotFoundHandler
        }

    router

/// Gets the current browser location path
let getCurrentPath (router: BrowserRouter<'T>) =
    if router.Config.UseHashRouting then
        // For hash-based routing, use the hash part of the URL
        let hash = window.location.hash

        if hash.StartsWith("#") then
            let path = hash.Substring(1)

            if path.StartsWith("/") then
                path
            else
                "/" + path
        else
            "/"
    else
        // For history API routing, use the pathname
        let path = window.location.pathname

        // Remove the base path if it exists
        if
            not (String.IsNullOrEmpty(router.Config.BasePath))
            && path.StartsWith(router.Config.BasePath)
        then
            let relativePath = path.Substring(router.Config.BasePath.Length)

            if relativePath.StartsWith("/") then
                relativePath
            else
                "/" + relativePath
        else
            path

/// Gets the full URL including query string and hash
let getFullUrl (router: BrowserRouter<'T>) =
    let path = getCurrentPath router
    let search = window.location.search

    let hash =
        if router.Config.UseHashRouting then
            "" // Hash is already part of the path in hash routing
        else
            window.location.hash

    path + search + hash

/// Handles a location change
let handleLocationChange (router: BrowserRouter<'T>) =
    let url = getFullUrl router

    // Skip if the location hasn't changed
    if url <> router.CurrentLocation then
        router.CurrentLocation <- url

        // Match the route
        let result = router.Config.Router.Match(url)

        // Handle 404 if needed
        let finalResult =
            match result, router.Config.NotFoundHandler with
            | None, Some handler -> Some(handler ())
            | _ -> result

        // Render the result
        router.Config.Render finalResult

/// Navigates to a new URL
let navigate (router: BrowserRouter<'T>) (url: string) =
    if router.Config.UseHashRouting then
        // For hash-based routing, update the hash
        let newHash =
            if url.StartsWith("/") then
                "#" + url
            else
                "#/" + url

        window.location.hash <- newHash
    else
        // For history API routing, use pushState
        let fullUrl =
            if url.StartsWith("/") then
                router.Config.BasePath + url
            else
                router.Config.BasePath + "/" + url

        history.pushState (null, "", fullUrl)

        // Manually trigger location change since pushState doesn't fire an event
        handleLocationChange router

/// Replaces the current URL without adding to history
let replace (router: BrowserRouter<'T>) (url: string) =
    if router.Config.UseHashRouting then
        // For hash-based routing, update the hash without triggering navigation
        let newHash =
            if url.StartsWith("/") then
                "#" + url
            else
                "#/" + url

        history.replaceState (null, "", window.location.pathname + window.location.search + newHash)
    else
        // For history API routing, use replaceState
        let fullUrl =
            if url.StartsWith("/") then
                router.Config.BasePath + url
            else
                router.Config.BasePath + "/" + url

        history.replaceState (null, "", fullUrl)

    // Manually trigger location change
    handleLocationChange router

/// Starts the router and listens for location changes
let start (router: BrowserRouter<'T>) =
    // Set up event listeners
    if router.Config.UseHashRouting then
        // For hash-based routing, listen to hashchange events
        window.addEventListener ("hashchange", fun _ -> handleLocationChange router)
    else
        // For history API routing, listen to popstate events
        window.addEventListener ("popstate", fun _ -> handleLocationChange router)

    // Handle the initial location
    handleLocationChange router

    router

/// Creates a link element that uses the router for navigation
let createLink (router: BrowserRouter<'T>) (url: string) (content: string) =
    let link = document.createElement ("a") :?> HTMLAnchorElement

    link.href <-
        if router.Config.UseHashRouting then
            "#"
            + (if url.StartsWith("/") then
                   url
               else
                   "/" + url)
        else
            router.Config.BasePath
            + (if url.StartsWith("/") then
                   url
               else
                   "/" + url)

    link.textContent <- content

    // Intercept clicks to use the router's navigation
    link.addEventListener (
        "click",
        fun e ->
            e.preventDefault ()
            navigate router url
    )

    link
