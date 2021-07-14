import React from "react";
import TodoItem from "./TodoItem";
import { useStore } from "laco-react";
import { TodoStore, getFilteredTodos } from "../stores/todo";

const TodoList = () => {
  const { visibilityFilter } = useStore(TodoStore);
  return (
    <ul className="todo-list">
      {getFilteredTodos(visibilityFilter).map(todo => (
        <TodoItem key={todo.id} todo={todo} />
      ))}
    </ul>
  );
};

export default TodoList;
