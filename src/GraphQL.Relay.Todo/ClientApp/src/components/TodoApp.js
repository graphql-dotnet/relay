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
        return (
            <div>
                <section className="todoapp">
                    <header className="header">
                        <h1>todos</h1>
                        <TodoTextInput
                            autoFocus
                            className="new-todo"
                            onSave={this._handleTextInputSave}
                            placeholder="What needs to be done?"
                        />
                    </header>
                    <TodoList viewer={this.props.viewer} />
                        <TodoListFooter
                            todos={this.props.viewer.todos}
                            viewer={this.props.viewer}
                        />
                </section>
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