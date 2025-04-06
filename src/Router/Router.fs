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
        match matchRoute (routes |> List.map (fun r -> r.Route)) url with
        | Some(route, result) ->
            let registeredRoute = routes |> List.find (fun r -> r.Route.Pattern = route.Pattern)

            match registeredRoute.Children with
            | Some childRouter ->
                let parentPath =
                    if result.Pattern.EndsWith("/") then
                        result.Pattern
                    else
                        result.Pattern + "/"

                let remainingPath =
                    if url.StartsWith(parentPath) then
                        url.Substring(parentPath.Length - 1)
                    else
                        "/"

                // Return both the parent match and child match
                Some
                    {
                        Result = result
                        Handler = registeredRoute.Handler
                        Child = childRouter.Match(remainingPath)
                    }
            | None ->
                Some
                    {
                        Result = result
                        Handler = registeredRoute.Handler
                        Child = None
                    }
        | None -> None
