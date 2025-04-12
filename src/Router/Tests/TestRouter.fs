module FreeActRouterTests

open FreeActRouter
open FreeAct.Tokenizer

let testTokenizer () =
    printfn "Testing URL Tokenizer:"

    let testPaths =
        [
            "/users"
            "/users/:userId"
            "/users/:userId/posts/:postId"
            "/files/*path"
            "/items/:id(\\d+)"
            "/profile/:username?"
            "/"
        ]

    for path in testPaths do
        let tokens = tokenize path
        printfn "\nPath: %s" path
        printfn "Tokens:"

        for token in tokens do
            printfn "  %A: %s" token.Type token.Value

        let regex = tokensToRegex tokens
        printfn "Regex: %s" regex

        let paramNames = extractParamNames tokens
        printfn "Param Names: %A" paramNames

        let score = scoreRoute tokens
        printfn "Score: %d" score

// testTokenizer ()

// Test URL parsing
let testUrlParsing () =
    printfn "\nTesting URL Parsing:"

    let testUrls =
        [
            "/"
            "/users"
            "/users/42"
            "/users/42?tab=profile&sort=desc"
            "/search?q=test&filter=active&filter=recent"
            "/docs/intro#getting-started"
            "/profile?view=public#contact"
        ]

    for url in testUrls do
        let parsed = parseUrl url
        printfn "\nURL: %s" url
        printfn "  Path: %s" parsed.Path
        printfn "  Query Parameters:"

        for KeyValue(key, values) in parsed.QueryParams do
            printfn "    %s = %A" key values

        printfn "  Fragment: %A" parsed.Fragment

// testUrlParsing ()

// Test the router
let testRouter () =
    printfn "\n=== Router Tests ==="

    let router = Router<string>()

    router
        .Route("/", fun ctx -> "Home Page")
        .Route("/about", fun ctx -> "About Page")
        .Route(
            "/users",
            fun ctx ->
                let sort =
                    match ctx.Result.QueryParams.TryFind "sort" with
                    | Some [ sort ] -> sprintf " (Sorted by: %s)" sort
                    | _ -> ""

                sprintf "User List%s" sort
        )
        .Route(
            "/users/:userId",
            fun ctx ->
                let userId = ctx.Result.PathParams.["userId"]

                let tab =
                    match ctx.Result.QueryParams.TryFind "tab" with
                    | Some [ tab ] -> tab
                    | _ -> "overview"

                let fragment =
                    match ctx.Result.Fragment with
                    | Some section -> sprintf " (Section: #%s)" section
                    | None -> ""

                sprintf "User Detail: %s (Tab: %s)%s" userId tab fragment
        )
        .Route(
            "/users/:userId/posts/:postId",
            fun ctx ->
                sprintf
                    "Post %s by User %s"
                    ctx.Result.PathParams.["postId"]
                    ctx.Result.PathParams.["userId"]
        )
        .Route("/users/admin", fun _ -> "Admin Panel")
        .Route(
            "/search",
            fun ctx ->
                let query =
                    match ctx.Result.QueryParams.TryFind "q" with
                    | Some [ q ] -> q
                    | _ -> ""

                let filters =
                    match ctx.Result.QueryParams.TryFind "filter" with
                    | Some filters -> sprintf " (Filters: %s)" (String.concat ", " filters)
                    | None -> ""

                sprintf "Search Results for '%s'%s" query filters
        )
        .Route("/files/*path", fun ctx -> sprintf "File: %s" ctx.Result.PathParams.["path"])
        .Route(
            "/items/:id(\\d+)",
            fun ctx -> sprintf "Item with numeric ID: %s" ctx.Result.PathParams.["id"]
        )
    |> ignore

    let testUrls =
        [
            "/"
            "/about"
            "/users"
            "/users?sort=name"
            "/users/42"
            "/users/42?tab=profile"
            "/users/42?tab=settings#security"
            "/users/admin"
            "/users/42/posts/123"
            "/search?q=fsharp"
            "/search?q=router&filter=recent&filter=popular"
            "/files/documents/report.pdf"
            "/items/42"
            "/items/abc" // Should not match the numeric ID pattern
            "/unknown"
        ]

    for url in testUrls do
        match router.Match(url) with
        | Some matched ->
            matched.Handler { Result = matched.Result; ChildContent = None }
            |> printfn "URL: %s => %s" url
        | None -> printfn "URL: %s => No match" url

// testRouter ()

// Test nested routes
let testNestedRoutes () =
    printfn "\n=== Nested Router Tests ==="

    let childRouter = Router<string>()
    childRouter.Route("/profile", fun ctx -> "Profile") |> ignore
    childRouter.Route("/settings", fun ctx -> "Settings") |> ignore
    childRouter.Route("/settings/privacy", fun ctx -> "Privacy Settings") |> ignore

    let router = Router<string>()

    router
        .Route("/", fun ctx -> "Home")
        .RouteWithChildren(
            "/users"
            , fun ctx ->
                match ctx.ChildContent with
                | Some child -> sprintf "Users List + [%s]" child
                | None -> "Users List"
            , childRouter
        )
    |> ignore

    let testUrls =
        [
            "/" // Should match home
            "/users" // Should match users
            "/users/" // Should match users
            "/users/profile" // Should match users + profile
            "/users/settings" // Should match users + settings
            "/users/settings/" // Should match users + settings
            "/users/settings/privacy" // Should match users + settings + privacy
            "/unknown" // Should not match
        ]

    for url in testUrls do
        printfn "\nTesting URL: %s" url

        match router.Match(url) with
        | Some matched ->
            let rec renderMatch (m: MatchedRoute<string>) =
                match m.Child with
                | Some child ->
                    let childContent = renderMatch child
                    m.Handler { Result = m.Result; ChildContent = Some childContent }
                | None -> m.Handler { Result = m.Result; ChildContent = None }

            let result = renderMatch matched
            printfn "Match result: %s" result
            printfn "Path params: %A" matched.Result.PathParams
        | None -> printfn "No match found"

// testNestedRoutes ()
