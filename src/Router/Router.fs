module FreeAct.Router

open Tokenizer

/// Represents a route handler
type RouteHandler<'T> = RouteMatchResult -> 'T

/// Represents a registered route with its pattern and handler
type RegisteredRoute<'T> = { Route: Route; Handler: RouteHandler<'T> }

/// Simple router implementation
type Router<'T>() =
    let mutable routes: RegisteredRoute<'T> list = []

    /// Registers a new route
    member this.Route(pattern: string, handler: RouteHandler<'T>) =
        let route = createRoute pattern
        routes <- { Route = route; Handler = handler } :: routes
        this

    /// Matches a URL against registered routes
    member this.Match(url: string) : option<RouteMatchResult * RouteHandler<'T>> =
        match matchRoute (routes |> List.map (fun r -> r.Route)) url with
        | Some(route, result) ->
            let registeredRoute = routes |> List.find (fun r -> r.Route.Pattern = route.Pattern)
            Some(result, registeredRoute.Handler)
        | None -> None
