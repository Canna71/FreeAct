

BUG: After a navigation, the app is not responding to events (subscriptions disposed?)
TODO: study a better way to architecture the project and avoid circular dependencies
- TODO: add a way to define the initial model in the app
- TODO: simplify the way a part of an application treat its own part of the app state
- TODO: improve the way appDb is passed around
TODO: implement nested routes
TODO: add computation expressions for routes
TODO: possibly allow synchronous effects
TODO: Fix Named event handlers!!! => they should work similar to typed ones
TODO: syntactic sugar or other for FunctionComponent.Of
TODO: find an alternative to FunctionComponent.Of (Elmish?)
TODO: split project into multiple projects
TODO: complete the builders for elements
TODO: fix naming to distinguish between definitions of effects and actually executing them
TODO: improve demo, avoid errors for duplicate Keys
TODO: restore composition demo
TODO: add a demo for typed(Union) events

Differences between subscribers and effects:

Subscriptions are used to listen to events and update the model based on those events.
Effects are used to perform side effects based on the model or messages.
Subscriptions are typically used to listen to events that are external to the application, such as user input or network requests.
Effects are typically used to perform actions that are not directly related to the model, such as logging or making network requests.

