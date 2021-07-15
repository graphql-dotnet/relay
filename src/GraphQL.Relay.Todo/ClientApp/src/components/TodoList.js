import React from 'react';
import graphql from 'babel-plugin-relay/macro';
import { createFragmentContainer } from 'react-relay'

import MarkAllTodosMutation from '../mutations/MarkAllTodosMutation'
import Todo from './Todo'

class TodoList extends React.Component {
  _handleMarkAllChange = e => {
    const complete = e.target.checked
    MarkAllTodosMutation(
      this.props.relay.environment,
      complete,
      this.props.viewer.todos,
      this.props.viewer
    )
  }

  renderTodos() {
    return this.props.viewer.todos.edges.map(edge => (
      <Todo
        key={edge.node.id}
        todo={edge.node}
        viewer={this.props.viewer}
      />
    ))
  }

  render() {
    const numTodos = this.props.viewer.totalCount
    const numCompletedTodos = this.props.viewer.completedCount
    return (
      <section className="main">
        <input
          checked={true}
          className="toggle-all"
          onChange={this._handleMarkAllChange}
          type="checkbox"
        />
        <label htmlFor="toggle-all">Mark all as complete</label>
        <ul className="todo-list">{this.renderTodos()}</ul>
      </section>
    )
  }
}

export default createFragmentContainer(TodoList, {
  viewer: graphql`
     fragment TodoList_viewer on User {
       todos(
         first: 2147483647 # max GraphQLInt
       ) @connection(key: "TodoList_todos") {
         edges {
           node {
             id
             complete
             ...Todo_todo
           }
         }
       }
       id
       totalCount
       completedCount
       ...Todo_viewer
     }
   `,
});