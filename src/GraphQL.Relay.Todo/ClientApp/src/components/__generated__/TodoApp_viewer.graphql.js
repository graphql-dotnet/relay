/**
 * @flow
 */

/* eslint-disable */

'use strict';

/*::
<<<<<<< Updated upstream:src/GraphQL.Relay.Todo/ClientApp/components/__generated__/TodoApp_viewer.graphql.js
import type {ConcreteFragment} from 'relay-runtime';
export type TodoApp_viewer = {|
  +id: string;
  +totalCount: ?number;
=======
import type { ReaderFragment } from 'relay-runtime';
import type { FragmentReference } from "relay-runtime";
declare export opaque type TodoApp_viewer$ref: FragmentReference;
declare export opaque type TodoApp_viewer$fragmentType: TodoApp_viewer$ref;
export type TodoApp_viewer = {|
  +id: string,
  +totalCount: ?number,
  +$refType: TodoApp_viewer$ref,
>>>>>>> Stashed changes:src/GraphQL.Relay.Todo/ClientApp/src/components/__generated__/TodoApp_viewer.graphql.js
|};
*/


const fragment /*: ConcreteFragment*/ = {
  "argumentDefinitions": [],
  "kind": "Fragment",
  "metadata": null,
  "name": "TodoApp_viewer",
  "selections": [
    {
      "kind": "ScalarField",
      "alias": null,
      "args": null,
      "name": "id",
      "storageKey": null
    },
    {
      "kind": "ScalarField",
      "alias": null,
      "args": null,
      "name": "totalCount",
      "storageKey": null
<<<<<<< Updated upstream:src/GraphQL.Relay.Todo/ClientApp/components/__generated__/TodoApp_viewer.graphql.js
    },
    {
      "kind": "FragmentSpread",
      "name": "TodoListFooter_viewer",
      "args": null
    },
    {
      "kind": "FragmentSpread",
      "name": "TodoList_viewer",
      "args": null
=======
>>>>>>> Stashed changes:src/GraphQL.Relay.Todo/ClientApp/src/components/__generated__/TodoApp_viewer.graphql.js
    }
  ],
  "type": "User"
};
<<<<<<< Updated upstream:src/GraphQL.Relay.Todo/ClientApp/components/__generated__/TodoApp_viewer.graphql.js
=======
// prettier-ignore
(node/*: any*/).hash = '9a4e63b5dbc7ffde130bfc47a79868dd';
>>>>>>> Stashed changes:src/GraphQL.Relay.Todo/ClientApp/src/components/__generated__/TodoApp_viewer.graphql.js

module.exports = fragment;
