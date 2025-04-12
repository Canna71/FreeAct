# FreeSS

FreeSS is a type-safe CSS-in-F# solution.

## Basic Styling

```fsharp
let button = style {
    display Display.Flex
    alignItems AlignItems.Center
    padding (Spacing.All 12)
    backgroundColor "#007bff"
    color "white"
    borderRadius 4
}

// Use in components
div {
    className button
    text "Styled Button"
}
```

## Style Composition

```fsharp
let baseButton = style {
    padding (Spacing.All 12)
    borderRadius 4
}

let primaryButton = style {
    compose baseButton
    backgroundColor "#007bff"
    color "white"
}

let dangerButton = style {
    compose baseButton
    backgroundColor "#dc3545"
    color "white"
}
```

## Responsive Design

```fsharp
let responsive = style {
    display Display.Flex
    
    media "screen and (max-width: 768px)" {
        flexDirection FlexDirection.Column
    }
}
```

## Type Safety

FreeSS provides type-safe alternatives for all CSS properties:

- Layout: Display, Position, FlexDirection, etc.
- Spacing: Margin, Padding with typed units
- Colors: RGB, HSL, named colors
- Typography: FontWeight, TextAlign, etc.
