import graphql from 'babel-plugin-relay/macro';
import { commitMutation } from 'react-relay'

const mutation = graphql`
  mutation ChangeTodoStatusMutation($input: ChangeTodoStatusInput!) {
    changeTodoStatus(input: $input) {
      todo {
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

function getOptimisticResponse(complete, todo, user) {
  const viewerPayload = { id: user.id }
  if (user.completedCount != null) {
    viewerPayload.completedCount = complete
      ? user.completedCount + 1
      : user.completedCount - 1
  }
  return {
    changeTodoStatus: {
      todo: {
        complete: complete,
        id: todo.id,
      },
      viewer: viewerPayload,
    },
  }
}

export default function commit(environment, complete, todo, user) {
  return commitMutation(environment, {
    mutation,
    variables: { input: { complete: complete, id: todo.id } },
    optimisticResponse: getOptimisticResponse(complete, todo, user),
  })
}