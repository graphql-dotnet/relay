import React from "react";
import Footer from "./TodoListFooter";
import TodoList from "../components/TodoList";
import { useStore } from "laco-react";
import {
    TodoStore,
    completeAllTodos,
    clearCompletedTodos,
    getCompletedCount
} from "../stores/todo";

const MainSection = () => {
    const { todos } = useStore(TodoStore);
    const todosCount = todos.length;
    const completedCount = getCompletedCount(todos);
    return (
        <section className="main">
            {!!todosCount && (
                <span>
                    <input
                        className="toggle-all"
                        type="checkbox"
                        defaultChecked={completedCount === todosCount}
                    />
                    <label onClick={completeAllTodos} />
                </span>
            )}
            <TodoList />
            {!!todosCount && (
                <Footer
                    completedCount={completedCount}
                    activeCount={todosCount - completedCount}
                    onClearCompleted={clearCompletedTodos}
                />
            )}
        </section>
    );
};

export default MainSection;
