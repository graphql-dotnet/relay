import React from 'react';
import graphql from 'babel-plugin-relay/macro';
import { createFragmentContainer } from 'react-relay'

import AddTodoMutation from '../mutations/AddTodoMutation';
import TodoList from './TodoList';
import TodoListFooter from './TodoListFooter';
import TodoTextInput from './TodoTextInput';

class TodoApp extends React.Component {
    _handleTextInputSave = (text) => {
        AddTodoMutation(
            this.props.relay.environment,
            text,
            this.props.viewer,
        );
    };

    render() {
        const hasTodos = this.props.viewer.totalCount > 0;
        return (
            <div>
                <section className="todoapp">
                    <header className="header">
                        <h1>
                            todos
                        </h1>
                        <TodoTextInput
                            autoFocus={true}
                            className="new-todo"
                            onSave={this._handleTextInputSave}
                            placeholder="What needs to be done?"
                        />
                    </header>
                    <TodoList viewer={this.props.viewer} />
                    {hasTodos &&
                        <TodoListFooter
                            todos={this.props.viewer.todos}
                            viewer={this.props.viewer}
                        />
                    }
                </section>
                <footer className="info">
                    <p>
                        Double-click to edit a todo
                    </p>
                    <p>
                        Created by the <a href="https://facebook.github.io/relay/">
                            Relay team
                        </a>
                    </p>
                    <p>
                        Part of <a href="http://todomvc.com">TodoMVC</a>
                    </p>
                </footer>
            </div>
        );
    }
}

export default createFragmentContainer(TodoApp, {
    viewer: graphql`
    fragment TodoApp_viewer on User {
      id,
      totalCount,
      ...TodoListFooter_viewer,
      ...TodoList_viewer,
    }
  `,
});