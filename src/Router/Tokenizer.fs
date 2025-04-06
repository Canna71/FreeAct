module FreeAct.Tokenizer

open System
open System.Text.RegularExpressions

/// Represents different types of path segments
type TokenType =
    | Static of string // Static path segment like "users"
    | Parameter of string // Dynamic parameter like ":userId"
    | Wildcard of string // Wildcard/splat parameter like "*rest"
    | Optional of TokenType // Optional parameter like ":userId?"
    | Custom of string * string // Custom regex parameter like ":id(\d+)"

/// Represents a parsed token with its type and value
type Token =
    {
        Type: TokenType
        Value: string
        Pattern: option<string> // Regex pattern for custom parameters
    }

/// Represents a parsed URL with path, query parameters, and fragment
type ParsedUrl =
    {
        Path: string
        QueryParams: Map<string, string list>
        Fragment: string option
    }

/// Parses a full URL into its components (path, query params, fragment)
let parseUrl (url: string) : ParsedUrl =
    // Split the URL into path, query string, and fragment
    let urlParts = url.Split([| '#' |], 2)
    let pathAndQuery = urlParts.[0]

    let fragment =
        if urlParts.Length > 1 then
            Some urlParts.[1]
        else
            None

    let queryParts = pathAndQuery.Split([| '?' |], 2)
    let path = queryParts.[0]

    let queryString =
        if queryParts.Length > 1 then
            queryParts.[1]
        else
            ""

    // Parse query parameters
    let queryParams =
        if String.IsNullOrEmpty(queryString) then
            Map.empty
        else
            queryString.Split('&')
            |> Array.choose (fun param ->
                let parts = param.Split([| '=' |], 2)

                if parts.Length = 2 && not (String.IsNullOrEmpty(parts.[0])) then
                    let key = Uri.UnescapeDataString(parts.[0])
                    let value = Uri.UnescapeDataString(parts.[1])
                    Some(key, value)
                else if parts.Length = 1 && not (String.IsNullOrEmpty(parts.[0])) then
                    let key = Uri.UnescapeDataString(parts.[0])
                    Some(key, "")
                else
                    None
            )
            |> Array.groupBy fst
            |> Array.map (fun (key, values) -> (key, values |> Array.map snd |> Array.toList))
            |> Map.ofArray

    {
        Path = path
        QueryParams = queryParams
        Fragment = fragment
    }

/// Tokenizes a URL path pattern into a sequence of tokens
let tokenize (path: string) : Token list =
    // Helper function to check if a character is valid in a parameter name
    let isValidParamChar c = Char.IsLetterOrDigit c || c = '_'

    // Remove leading and trailing slashes
    let trimmedPath = path.Trim('/')

    // Split the path into segments
    if String.IsNullOrEmpty(trimmedPath) then
        [] // Empty path
    else
        let segments = trimmedPath.Split('/')

        segments
        |> Array.map (fun segment ->
            match segment with
            | "" ->
                {
                    Type = Static ""
                    Value = ""
                    Pattern = None
                }

            | s when s.StartsWith("*") ->
                // Wildcard parameter
                let name =
                    if s.Length > 1 then
                        s.Substring(1)
                    else
                        "splat"

                {
                    Type = Wildcard name
                    Value = name
                    Pattern = None
                }

            | s when s.StartsWith(":") ->
                // Parameter or custom parameter
                let paramWithoutColon = s.Substring(1)

                if paramWithoutColon.Contains("(") && paramWithoutColon.EndsWith(")") then
                    // Custom regex parameter
                    let openParenIndex = paramWithoutColon.IndexOf("(")
                    let name = paramWithoutColon.Substring(0, openParenIndex)

                    let pattern =
                        paramWithoutColon.Substring(
                            openParenIndex + 1,
                            paramWithoutColon.Length - openParenIndex - 2
                        )

                    {
                        Type = Custom(name, pattern)
                        Value = name
                        Pattern = Some pattern
                    }

                elif paramWithoutColon.EndsWith("?") then
                    // Optional parameter
                    let name = paramWithoutColon.Substring(0, paramWithoutColon.Length - 1)

                    {
                        Type = Optional(Parameter name)
                        Value = name
                        Pattern = None
                    }

                else
                    // Regular parameter
                    {
                        Type = Parameter paramWithoutColon
                        Value = paramWithoutColon
                        Pattern = None
                    }

            | s ->
                // Static segment
                {
                    Type = Static s
                    Value = s
                    Pattern = None
                }
        )
        |> Array.toList

/// Converts a token list to a regular expression pattern for matching
let tokensToRegex (tokens: Token list) : string =
    let patterns =
        tokens
        |> List.map (fun token ->
            match token.Type with
            | Static value -> sprintf "/%s" (Regex.Escape(value))

            | Parameter _ -> "/([^/]+)"

            | Wildcard _ -> "(/.*)"

            | Optional(Parameter _) -> "(?:/([^/]+))?"

            | Custom(_, pattern) -> sprintf "/(%s)" pattern

            | _ -> "/([^/]+)" // Default case
        )

    let pattern = String.concat "" patterns

    "^"
    + (if pattern = "" then
           "/"
       else
           pattern)
    + "(?:/.*)?$" // Allow any content after the match

/// Extracts parameter names from a token list
let extractParamNames (tokens: Token list) : string list =
    tokens
    |> List.choose (fun token ->
        match token.Type with
        | Parameter name -> Some name
        | Wildcard name -> Some name
        | Optional(Parameter name) -> Some name
        | Custom(name, _) -> Some name
        | _ -> None
    )

/// Represents the result of a route match
type RouteMatchResult =
    {
        Pattern: string // The matched route pattern
        PathParams: Map<string, string> // Parameters from the URL path
        QueryParams: Map<string, string list> // Query string parameters
        Fragment: string option // URL fragment (hash)
    }

/// Creates a route matcher function from a path pattern
let createMatcher (pathPattern: string) =
    let tokens = tokenize pathPattern
    let regex = tokensToRegex tokens
    let paramNames = extractParamNames tokens

    // printfn "Created matcher for %s with regex: %s" pathPattern regex // Debug output
    let regexObj = Regex(regex)

    fun (url: string) ->
        // Normalize URL by removing trailing slash except for root
        let normalizedUrl =
            if url = "/" then
                url
            else
                url.TrimEnd('/')

        let normalizedPath =
            if normalizedUrl.StartsWith("/") then
                normalizedUrl
            else
                "/" + normalizedUrl

        let m = regexObj.Match(normalizedPath)
        // printfn "Matching %s against %s: %b" normalizedPath regex m.Success // Debug output

        if m.Success then
            // Extract path parameters
            let pathParams =
                paramNames
                |> List.mapi (fun i name ->
                    let groupIndex = i + 1 // Group 0 is the entire match

                    if groupIndex < m.Groups.Count && m.Groups.[groupIndex].Success then
                        Some(name, m.Groups.[groupIndex].Value)
                    else
                        None
                )
                |> List.choose id
                |> Map.ofList

            // Return the complete match result
            Some
                {
                    Pattern = pathPattern
                    PathParams = pathParams
                    QueryParams = (parseUrl url).QueryParams
                    Fragment = (parseUrl url).Fragment
                }
        else
            None

/// Scores a route pattern for ranking (higher score = more specific)
let scoreRoute (tokens: Token list) : int =
    tokens
    |> List.sumBy (fun token ->
        match token.Type with
        | Static _ -> 2 // Static segments are most specific
        | Custom _ -> 2 // Custom regex segments are equally specific
        | Parameter _ -> 1 // Dynamic parameters are less specific
        | Optional _ -> 0 // Optional segments are even less specific
        | Wildcard _ -> 0 // Wildcard segments are least specific
    )

/// Represents a route with its pattern, tokens, and score
type Route =
    {
        Pattern: string
        Tokens: Token list
        Score: int
        Matcher: string -> RouteMatchResult option
    }

/// Creates a route from a path pattern
let createRoute (pathPattern: string) : Route =
    let tokens = tokenize pathPattern

    {
        Pattern = pathPattern
        Tokens = tokens
        Score = scoreRoute tokens
        Matcher = createMatcher pathPattern
    }

/// Finds the best matching route for a given path
let matchRoute (routes: Route list) (url: string) : (Route * RouteMatchResult) option =
    routes
    |> List.sortByDescending (fun route -> route.Score)
    |> List.choose (fun route ->
        match route.Matcher url with
        | Some result -> Some(route, result)
        | None -> None
    )
    |> List.tryHead
