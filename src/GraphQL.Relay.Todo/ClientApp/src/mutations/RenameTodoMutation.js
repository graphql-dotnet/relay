import graphql from 'babel-plugin-relay/macro';
import { commitMutation } from 'react-relay'

const mutation = graphql`
  mutation RenameTodoMutation($input: RenameTodoInput!) {
    renameTodo(input: $input) {
      todo {
        id
        text
      }
    }
  }
`

function getOptimisticResponse(text, todo) {
  return {
    renameTodo: {
      todo: {
        id: todo.id,
        text: text,
      },
    },
  }
}

export default function commit(environment, text, todo) {
  return commitMutation(environment, {
    mutation,
    variables: {
      input: { text, id: todo.id },
    },
    optimisticResponse: getOptimisticResponse(text, todo),
  })
}