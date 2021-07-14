import React, { Component } from "react";
import classnames from "classnames";
import TodoTextInput from "./TodoTextInput";

import { editTodo, deleteTodo, completeTodo } from "../stores/todo";

export default class TodoItem extends Component {
    state = {
        editing: false
    };

    handleDoubleClick = () => {
        this.setState({ editing: true });
    };

    handleSave = (id, text) => {
        if (text.length === 0) {
            deleteTodo(id);
        } else {
            editTodo(id, text);
        }
        this.setState({ editing: false });
    };

    render() {
        const { todo } = this.props;

        let element;
        if (this.state.editing) {
            element = (
                <TodoTextInput
                    text={todo.text}
                    editing={this.state.editing}
                    onSave={text => this.handleSave(todo.id, text)}
                />
            );
        } else {
            element = (
                <div className="view">
                    <input
                        className="toggle"
                        type="checkbox"
                        checked={todo.completed}
                        onChange={() => completeTodo(todo.id)}
                    />
                    <label onDoubleClick={this.handleDoubleClick}>{todo.text}</label>
                    <button className="destroy" onClick={() => deleteTodo(todo.id)} />
                </div>
            );
        }

        return (
            <li
                className={classnames({
                    completed: todo.completed,
                    editing: this.state.editing
                })}
            >
                {element}
            </li>
        );
    }
}