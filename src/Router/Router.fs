module FreeAct.Router

open Tokenizer
type RouteMatchResult = Tokenizer.RouteMatchResult

/// Represents a route handler
type RouteHandler<'T> = RouteMatchResult -> 'T

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

    member this.Match(url: string) =
        let rec matchNested
            (remainingPath: string)
            (parentResult: RouteMatchResult)
            (childRouter: Router<'T>)
            =
            match childRouter.Match(remainingPath) with
            | Some(childResult, childHandler) ->
                let mergedResult =
                    { childResult with
                        PathParams = mergeMaps parentResult.PathParams childResult.PathParams
                        Pattern = parentResult.Pattern + childResult.Pattern
                        QueryParams = mergeMaps parentResult.QueryParams childResult.QueryParams
                    }

                Some(mergedResult, childHandler)
            | None -> None

        match matchRoute (routes |> List.map (fun r -> r.Route)) url with
        | Some(route, result) ->
            let registeredRoute = routes |> List.find (fun r -> r.Route.Pattern = route.Pattern)

            match registeredRoute.Children with
            | Some childRouter ->
                // Extract the remaining path after the parent route
                let parentPath =
                    if result.Pattern.EndsWith("/") then
                        result.Pattern
                    else
                        result.Pattern + "/"

                let remainingPath =
                    if url.StartsWith(parentPath) then
                        url.Substring(parentPath.Length - 1) // Keep the leading slash
                    else
                        "/"

                // Try to match child routes first
                match matchNested remainingPath result childRouter with
                | Some _ as childMatch -> childMatch // Use child match if found
                | None -> Some(result, registeredRoute.Handler) // Fall back to parent handler
            | None ->
                // No children, return current match
                Some(result, registeredRoute.Handler)
        | None -> None
