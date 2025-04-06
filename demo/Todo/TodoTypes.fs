module Demo.Todo.Types
// Define application state type
type TodoItem = {
    id: int
    text: string
    completed: bool
}

// Add a union type for loading state
type LoadingState<'T> =
    | NotStarted
    | Loading
    | Loaded of 'T
    | Failed of string

type TodoAnalysis = {
    totalCount: int
    completedCount: int
    activeCount: int
    averageTextLength: float
}

type TodoAppState = {
    todos: TodoItem list
    filter: string  // "all" | "active" | "completed"
    nextId: int
    isLoading: bool // Track loading state in the app state
    todoAnalysis: LoadingState<TodoAnalysis> // Track analysis state
}

let initialTodoState = {
    todos = []
    filter = "all"
    nextId = 1
    isLoading = false
    todoAnalysis = NotStarted // Initialize analysis state
}