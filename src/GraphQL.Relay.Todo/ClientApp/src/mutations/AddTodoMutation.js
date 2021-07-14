import {
    commitMutation,
    graphql,
} from 'react-relay';
import { ConnectionHandler } from 'relay-runtime';

const mutation = graphql`
  mutation AddTodoMutation($input: AddTodoInput!) {
    addTodo(input:$input) {
      todoEdge {
        __typename
        cursor
        node {
          id
          complete
          text
        }
      }
      viewer {
        id
        totalCount
      }
    }
  }
`;