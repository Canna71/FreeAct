module FreeAct.Router.Tests

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

testTokenizer ()

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

testUrlParsing ()

// Test the router
let testRouter () =
    printfn "\n=== Router Tests ==="

    let router = Router<string>()

    router
        .Route("/", fun _ -> "Home Page")
        .Route("/about", fun _ -> "About Page")
        .Route(
            "/users",
            fun result ->
                let sort =
                    match result.QueryParams.TryFind "sort" with
                    | Some [ sort ] -> sprintf " (Sorted by: %s)" sort
                    | _ -> ""

                sprintf "User List%s" sort
        )
        .Route(
            "/users/:userId",
            fun result ->
                let userId = result.PathParams.["userId"]

                let tab =
                    match result.QueryParams.TryFind "tab" with
                    | Some [ tab ] -> tab
                    | _ -> "overview"

                let fragment =
                    match result.Fragment with
                    | Some section -> sprintf " (Section: #%s)" section
                    | None -> ""

                sprintf "User Detail: %s (Tab: %s)%s" userId tab fragment
        )
        .Route(
            "/users/:userId/posts/:postId",
            fun result ->
                sprintf
                    "Post %s by User %s"
                    result.PathParams.["postId"]
                    result.PathParams.["userId"]
        )
        .Route("/users/admin", fun _ -> "Admin Panel")
        .Route(
            "/search",
            fun result ->
                let query =
                    match result.QueryParams.TryFind "q" with
                    | Some [ q ] -> q
                    | _ -> ""

                let filters =
                    match result.QueryParams.TryFind "filter" with
                    | Some filters -> sprintf " (Filters: %s)" (String.concat ", " filters)
                    | None -> ""

                sprintf "Search Results for '%s'%s" query filters
        )
        .Route("/files/*path", fun result -> sprintf "File: %s" result.PathParams.["path"])
        .Route(
            "/items/:id(\\d+)",
            fun result -> sprintf "Item with numeric ID: %s" result.PathParams.["id"]
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
        | Some result -> printfn "URL: %s => %s" url result
        | None -> printfn "URL: %s => No match" url

testRouter ()
