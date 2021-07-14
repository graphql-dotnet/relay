import { Store } from "laco";

export const TodoStore = new Store(
    {
        todos: [
            {
                text: "Use Laco",
                completed: false,
                id: 0
            }
        ],
        visibilityFilter: "All"
    },
    "TodoStore"
);

export const addTodo = text =>
    TodoStore.set(
        ({ todos }) => ({
            todos: [
                ...todos,
                {
                    id: todos.reduce((maxId, todo) => Math.max(todo.id, maxId), -1) + 1,
                    completed: false,
                    text
                }
            ]
        }),
        "Add todo"
    );

export const deleteTodo = id =>
    TodoStore.set(
        ({ todos }) => ({
            todos: todos.filter(item => item.id !== id)
        }),
        "Delete todo"
    );

export const editTodo = (id, text) =>
    TodoStore.set(
        ({ todos }) => ({
            todos: todos.map(todo => (todo.id === id ? { ...todo, text } : todo))
        }),
        "Edit todo"
    );

export const completeTodo = id =>
    TodoStore.set(
        ({ todos }) => ({
            todos: todos.map(todo =>
                todo.id === id ? { ...todo, completed: !todo.completed } : todo
            )
        }),
        "Complete todo"
    );

export const completeAllTodos = () => {
    const todos = TodoStore.get().todos;
    const areAllMarked = todos.every(todo => todo.completed);
    TodoStore.set(
        () => ({
            todos: todos.map(todo => ({
                ...todo,
                completed: !areAllMarked
            }))
        }),
        "Complete all todos"
    );
};

export const clearCompletedTodos = () =>
    TodoStore.set(
        ({ todos }) => ({
            todos: todos.filter(t => t.completed === false)
        }),
        "Clear completed todos"
    );

export const getTodosCount = () => TodoStore.get().todos.length;

export const getCompletedCount = todos =>
    todos.reduce((count, todo) => (todo.completed ? count + 1 : count), 0);

// Visibility filter stuff
export const setVisibilityFilter = type =>
    TodoStore.set(
        () => ({
            visibilityFilter: type
        }),
        "Set visibility"
    );

export const getFilteredTodos = visibilityFilter => {
    const todos = TodoStore.get().todos;
    switch (visibilityFilter) {
        case "All":
            return todos;
        case "Completed":
            return todos.filter(t => t.completed);
        case "Active":
            return todos.filter(t => !t.completed);
        default:
            throw new Error("Unknown filter: " + visibilityFilter);
    }
};
