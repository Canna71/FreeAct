namespace FreeAct

module Router =
    // let's say I have the following route defined:
    // type Route =
    //     | Home
    //     | About
    //     | Contact
    //     | User of int
    //     | Post of int
    //     | NotFound

    // I can define a matching between routes and their corresponding URL patterns,
    // so for example "/home" would match the Home route, "/about" would match the About route,
    // "/contact" would match the Contact route, "/user/123" would match the User route with id 123,
    // "/post/456" would match the Post route with id 456, and any other URL would match the NotFound route.
    // I can use a discriminated union to define the routes and their corresponding URL patterns.
    // I can also define a function that takes a URL and returns the corresponding route.
    // I can use a pattern matching to match the URL against the defined patterns.
    // I can also define a function that takes a route and returns the corresponding URL.

    // structure to hold the route and its corresponding URL pattern
    type RoutePattern<'T> = { Route: 'T; Pattern: string }

    // function to create a route pattern
    let createRoutePattern route pattern = { Route = route; Pattern = pattern }
    // function to match a URL against the defined patterns
    let matchRoute (url: string) (patterns: RoutePattern<'T> list) =
        patterns
        |> List.tryFind (fun pattern -> url.StartsWith(pattern.Pattern))
        |> Option.map (fun pattern -> pattern.Route)
