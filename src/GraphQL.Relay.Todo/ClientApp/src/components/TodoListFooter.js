import React from 'react';
import graphql from 'babel-plugin-relay/macro';
import { createFragmentContainer } from 'react-relay'

import RemoveCompletedTodosMutation from '../mutations/RemoveCompletedTodosMutation';

class TodoListFooter extends React.Component {
    _handleRemoveCompletedTodosClick = () => {
        RemoveCompletedTodosMutation(
            this.props.relay.environment,
            this.props.viewer.completedTodos,
            this.props.viewer,
        );
    };

    render() {
        const numCompletedTodos = this.props.viewer.completedCount;
        const numRemainingTodos = this.props.viewer.totalCount - numCompletedTodos;
        return (
            <footer className="footer">
                <span className="todo-count">
                    <strong>{numRemainingTodos}</strong> item{numRemainingTodos === 1 ? '' : 's'} left
                </span>
                {numCompletedTodos > 0 &&
                    <button
                        className="clear-completed"
                        onClick={this._handleRemoveCompletedTodosClick}>
                        Clear completed
                    </button>
                }
            </footer>
        );
    }
}

export default createFragmentContainer(
    TodoListFooter,
    {
        viewer: graphql`
            fragment TodoListFooter_viewer on User {
              id,
              completedCount,
              completedTodos: todos(
                status: "completed",
                first: 2147483647  # max GraphQLInt
              ) {
                edges {
                  node {
                    id
                    complete
                  }
                }
              },
              totalCount,
            }
        `
    }
);