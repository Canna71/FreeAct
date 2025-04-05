namespace FreeAct.Documentation

/// <summary>
/// FreeAct DSL Documentation and Examples
/// </summary>
module Documentation =
    /// <summary>
    /// Basic element usage examples
    /// </summary>
    module Basic =
        /// <summary>
        /// Shows how to create basic elements with text and attributes
        /// </summary>
        /// <example>
        /// <code>
        /// // Simple div with text
        /// div { text "Hello World" }
        ///
        /// // Element with multiple attributes
        /// div {
        ///     className "container"
        ///     id "main"
        ///     text "Content"
        /// }
        ///
        /// // Nested elements
        /// div {
        ///     className "parent"
        ///     div {
        ///         className "child"
        ///         text "Child content"
        ///     }
        /// }
        /// </code>
        /// </example>
        let basicUsage = ()

    /// <summary>
    /// Form handling examples
    /// </summary>
    module Forms =
        /// <summary>
        /// Shows how to create forms with validation and event handling
        /// </summary>
        /// <example>
        /// <code>
        /// form {
        ///     onSubmit (fun ev ->
        ///         ev.preventDefault()
        ///         // Handle form submission
        ///     )
        ///
        ///     input {
        ///         type' InputType.Text
        ///         required true
        ///         placeholder "Enter name"
        ///         onInput (fun ev ->
        ///             // Handle input change
        ///         )
        ///     }
        ///
        ///     select {
        ///         multiple true
        ///         option {
        ///             value "1"
        ///             text "Option 1"
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        let formExample = ()

    /// <summary>
    /// SVG drawing examples
    /// </summary>
    module Svg =
        /// <summary>
        /// Shows how to create SVG graphics with the DSL
        /// </summary>
        /// <example>
        /// <code>
        /// svg {
        ///     viewBox "0 0 100 100"
        ///     className "chart"
        ///
        ///     // Groups with transforms
        ///     g {
        ///         transform "translate(10,10)"
        ///
        ///         // Basic shapes
        ///         circle {
        ///             cx 50.0
        ///             cy 50.0
        ///             r 40.0
        ///             fill "red"
        ///         }
        ///
        ///         // Paths
        ///         path {
        ///             d "M10 10 H 90 V 90 L 10 10"
        ///             fill "none"
        ///             stroke "black"
        ///         }
        ///     }
        ///
        ///     // Gradients
        ///     defs {
        ///         linearGradient {
        ///             id "grad1"
        ///             x1 0.0
        ///             y1 0.0
        ///             x2 1.0
        ///             y2 1.0
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        let svgExample = ()

    /// <summary>
    /// Layout elements and their common use patterns
    /// </summary>
    module Layout =
        /// <example>
        /// <code>
        /// // Semantic layout example
        /// main {
        ///     header {
        ///         h1 { text "Page Title" }
        ///         nav {
        ///             ul {
        ///                 li { a { href "#home"; text "Home" } }
        ///                 li { a { href "#about"; text "About" } }
        ///             }
        ///         }
        ///     }
        ///
        ///     article {
        ///         section {
        ///             h2 { text "Section Title" }
        ///             p { text "Content here" }
        ///         }
        ///         aside {
        ///             text "Related content"
        ///         }
        ///     }
        ///
        ///     footer {
        ///         text "Page footer"
        ///     }
        /// }
        /// </code>
        /// </example>
        let ``Semantic Layout`` = ()

    /// <summary>
    /// Form elements with validation and accessibility
    /// </summary>
    module FormExamples =
        /// <example>
        /// <code>
        /// form {
        ///     onSubmit (fun ev -> ev.preventDefault())
        ///
        ///     fieldset {
        ///         legend { text "Personal Information" }
        ///
        ///         div {
        ///             label {
        ///                 htmlFor "name"
        ///                 text "Full Name"
        ///             }
        ///             input {
        ///                 id "name"
        ///                 type' InputType.Text
        ///                 required true
        ///                 aria "required" "true"
        ///                 placeholder "Enter your name"
        ///             }
        ///         }
        ///
        ///         div {
        ///             label {
        ///                 htmlFor "email"
        ///                 text "Email"
        ///             }
        ///             input {
        ///                 id "email"
        ///                 type' InputType.Email
        ///                 pattern "[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,}$"
        ///                 required true
        ///             }
        ///         }
        ///     }
        /// }
        /// </code>
        /// </example>
        let ``Accessible Forms`` = ()

    /// <summary>
    /// Interactive components with ARIA attributes
    /// </summary>
    module Interactive =
        /// <example>
        /// <code>
        /// // Modal dialog example
        /// dialog {
        ///     open' true
        ///     aria "labelledby" "dialog-title"
        ///     aria "describedby" "dialog-content"
        ///
        ///     h2 {
        ///         id "dialog-title"
        ///         text "Dialog Title"
        ///     }
        ///
        ///     div {
        ///         id "dialog-content"
        ///         text "Dialog content here"
        ///     }
        ///
        ///     button {
        ///         aria "label" "Close dialog"
        ///         onClick (fun _ -> ())
        ///         text "Close"
        ///     }
        /// }
        ///
        /// // Expandable section
        /// details {
        ///     summary { text "Click to expand" }
        ///     div { text "Expanded content" }
        /// }
        /// </code>
        /// </example>
        let ``Interactive Elements`` = ()

    /// <summary>
    /// Media elements with fallbacks
    /// </summary>
    module Media =
        /// <example>
        /// <code>
        /// // Responsive image with picture element
        /// picture {
        ///     source {
        ///         media "(min-width: 800px)"
        ///         srcset "large.jpg"
        ///     }
        ///     source {
        ///         media "(min-width: 400px)"
        ///         srcset "medium.jpg"
        ///     }
        ///     img {
        ///         src "small.jpg"
        ///         alt "Descriptive text"
        ///     }
        /// }
        ///
        /// // Video with multiple sources and tracks
        /// video {
        ///     controls true
        ///     width 640
        ///     height 360
        ///
        ///     source {
        ///         src "video.mp4"
        ///         type' "video/mp4"
        ///     }
        ///     source {
        ///         src "video.webm"
        ///         type' "video/webm"
        ///     }
        ///
        ///     track {
        ///         kind TrackKind.Subtitles
        ///         src "captions.vtt"
        ///         srclang "en"
        ///         label "English"
        ///     }
        /// }
        /// </code>
        /// </example>
        let ``Media Elements`` = ()
