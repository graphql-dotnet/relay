import graphql from 'babel-plugin-relay/macro';
import { commitMutation } from 'react-relay'

const mutation = graphql`
  mutation MarkAllTodosMutation($input: MarkAllTodosInput!) {
    markAllTodos(input: $input) {
      changedTodos {
        id
        complete
      }
      viewer {
        id
        completedCount
      }
    }
  }
`

function getOptimisticResponse(complete, todos, user) {
  const payload = { viewer: { id: user.id } }
  if (todos && todos.edges) {
    payload.changedTodos = todos.edges
      .filter(edge => edge.node.complete !== complete)
      .map(edge => ({
        complete: complete,
        id: edge.node.id,
      }))
  }
  if (user.totalCount != null) {
    payload.viewer.completedCount = complete ? user.totalCount : 0
  }
  return {
    markAllTodos: payload,
  }
}

export default function commit(environment, complete, todos, user) {
  return commitMutation(environment, {
    mutation,
    variables: {
      input: { complete },
    },
    optimisticResponse: getOptimisticResponse(complete, todos, user),
  })
}