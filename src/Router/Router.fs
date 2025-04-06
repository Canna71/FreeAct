module FreeAct.Router

open Tokenizer

// Make RouteMatchResult type visible
type RouteMatchResult = Tokenizer.RouteMatchResult

/// Extended route context that includes child content
type RouteContext<'T> = { Result: RouteMatchResult; ChildContent: option<'T> }

/// Handler for a route - receives RouteContext with both match result and optional child content
type RouteHandler<'T> = RouteContext<'T> -> 'T

/// Result of a route match, containing the match info and handler
type MatchedRoute<'T> =
    {
        Result: RouteMatchResult
        Handler: RouteHandler<'T>
        Child: option<MatchedRoute<'T>>
    }

/// Represents a registered route with its pattern, handler and children
type RegisteredRoute<'T> =
    {
        Route: Route
        Handler: RouteHandler<'T>
        Children: Router<'T> option
    }

and Router<'T>() =
    let mutable routes: RegisteredRoute<'T> list = []

    // Helper function to merge maps
    let mergeMaps map1 map2 =
        Map(Seq.concat [ (Map.toSeq map1); (Map.toSeq map2) ])

    /// Registers a new route
    member this.Route(pattern: string, handler: RouteHandler<'T>) =
        let route = createRoute pattern

        routes <-
            {
                Route = route
                Handler = handler
                Children = None
            }
            :: routes

        this

    /// Registers a nested route
    member this.RouteWithChildren
        (pattern: string, handler: RouteHandler<'T>, children: Router<'T>)
        =
        let route = createRoute pattern

        routes <-
            {
                Route = route
                Handler = handler
                Children = Some children
            }
            :: routes

        this

    member this.Match(url: string) : option<MatchedRoute<'T>> =
        let normalizedUrl =
            if url.EndsWith("/") then
                url.TrimEnd('/')
            else
                url

        match matchRoute (routes |> List.map (fun r -> r.Route)) normalizedUrl with
        | Some(route, result) ->
            let registeredRoute = routes |> List.find (fun r -> r.Route.Pattern = route.Pattern)

            match registeredRoute.Children with
            | Some childRouter ->
                // Get the base path without trailing slash
                let basePath = result.Pattern.TrimEnd('/')

                // Only proceed with child matching if we have more path to match
                let childPath =
                    if normalizedUrl = basePath then
                        "/"
                    elif normalizedUrl.StartsWith(basePath + "/") then
                        normalizedUrl.Substring(basePath.Length)
                    else
                        "/"

                printfn
                    "Child router matching %s (from %s, base: %s)"
                    childPath
                    normalizedUrl
                    basePath

                Some
                    {
                        Result = result
                        Handler = registeredRoute.Handler
                        Child = childRouter.Match(childPath)
                    }
            | None ->
                Some
                    {
                        Result = result
                        Handler = registeredRoute.Handler
                        Child = None
                    }
        | None -> None
