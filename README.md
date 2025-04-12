# FreeAct

A DSL for creating React components in F#.

<!-- Could you please keep the link below so people can find the original template 🙏 -->

[![](https://img.shields.io/badge/Project_made_using_Glutinum.Template-7679db?style=for-the-badge)](https://github.com/glutinum-org/Glutinum.Template)

[![NuGet](https://img.shields.io/nuget/v/FreeAct.svg)](https://www.nuget.org/packages/FreeAct)

<!-- To learn how to use the template please refer to MANUAL.md -->

<!-- You can put the documentation for your binding below -->

## Usage

FreeAct is a DSL (Domain Specific Language) for building React applications in F# that provides:

- Component creation with a clean, F#-friendly syntax
- Built-in routing system
- State management with FreeFrame
- Type-safe styling with FreeSS
- Full integration with React ecosystem

## Overview

FreeAct is an opinionated F# library for building React applications that aims to provide a simpler alternative to existing solutions while maintaining type safety and separation of concerns. It consists of three main modules:

### FreeAct Core
A clean alternative to JSX or Feliz for creating React elements with a straightforward, F#-native syntax. Eliminates common pain points and provides a more intuitive API for F# developers.

### FreeFrame
A state management solution inspired by Re-Frame, offering a cleaner separation between rendering logic and application state. Unlike Elmish, FreeFrame promotes:
- Clear separation of concerns between state and view logic
- Simplified state updates through events and reducers
- Reactive views with automatic updates
- Composable state subscriptions

### FreeSS
A strongly-typed CSS-in-F# solution that provides:
- Full type safety for CSS properties and values
- F#-native syntax for defining styles
- Composition and reuse of style definitions
- No runtime overhead

## Installation

```bash
dotnet add package FreeAct
```

## Basic Usage

Here's a simple example showing the three main components working together:

```fsharp
open FreeAct
open FreeFrame
open FreeSS

// Define styles
let styles = style {
    display Display.Flex
    flexDirection FlexDirection.Column
    padding (Spacing.All 10)
}

// Define state management
type CounterEvent = 
    | Increment 
    | Decrement

// Create a view component
let Counter() =
    div {
        className styles
        
        button {
            onClick (fun _ -> dispatch CounterEvent.Increment)
            text "+"
        }
        
        span {
            text (string count)
        }
        
        button {
            onClick (fun _ -> dispatch CounterEvent.Decrement)
            text "-"
        }
    }
```

## Documentation

For more detailed documentation on each module:

- [FreeAct Core](./docs/freeact.md) - React element creation
- [FreeFrame](./docs/freeframe.md) - State management
- [FreeSS](./docs/freess.md) - Styling system

## Features

### Component Creation
- Clean, F#-native syntax for React components
- Strong type inference
- No JSX required
- Full access to React hooks and lifecycle methods
- Component composition with type-safe props

### Routing
- First-class routing support
- Type-safe route parameters
- Nested routes
- Query parameter handling
- Route-based code splitting

### State Management (FreeFrame)
- Event-driven architecture
- Pure reducers for state updates
- Automatic view updates
- Side-effect management
- State composition
- Time-travel debugging ready

### Styling (FreeSS)
- Type-safe CSS properties
- Style composition
- Media queries support
- Pseudo-selectors
- Dynamic styles
- Zero runtime overhead
- Scoped class names

## Examples

Our Todo application demonstrates the core features working together:

```fsharp
// Define state
type Todo = { id: int; text: string; completed: bool }
type TodoEvent = 
    | Add of string
    | Toggle of int
    | Delete of int

// Define styles
let todoItem = style {
    display Display.Flex
    alignItems AlignItems.Center
    padding (Spacing.All 8)
    borderBottom "1px solid #eee"
}

// Create components
let TodoList() =
    let todos = useView todosSubscription
    
    div {
        ul {
            className "todo-list"
            children (
                todos |> List.map (fun todo ->
                    li {
                        className todoItem
                        
                        input {
                            type' InputType.Checkbox
                            checked todo.completed
                            onChange (fun _ -> dispatch (Toggle todo.id))
                        }
                        
                        span { text todo.text }
                        
                        button {
                            onClick (fun _ -> dispatch (Delete todo.id))
                            text "×"
                        }
                    }
                )
            )
        }
    }
```

For more examples, check out our [demo application](demo/).

## Contributing

We welcome contributions! Here's how you can help:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes
4. Run tests (`dotnet test`)
5. Commit your changes (`git commit -m 'Add amazing feature'`)
6. Push to the branch (`git push origin feature/amazing-feature`)
7. Open a Pull Request

Please make sure to update tests as appropriate and follow our coding standards.

## License

Distributed under the MIT License. See `LICENSE` file for more information.
