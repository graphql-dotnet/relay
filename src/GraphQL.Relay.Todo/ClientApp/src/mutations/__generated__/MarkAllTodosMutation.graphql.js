/**
 * @flow
 */

/* eslint-disable */

'use strict';

/*::
import type { ConcreteRequest } from 'relay-runtime';
export type MarkAllTodosInput = {|
  complete?: ?boolean
|};
export type MarkAllTodosMutationVariables = {|
  input: MarkAllTodosInput
|};
export type MarkAllTodosMutationResponse = {|
  +markAllTodos: ?{|
    +changedTodos: ?$ReadOnlyArray<?{|
      +id: string,
      +complete: boolean,
    |}>,
    +viewer: ?{|
      +id: string,
      +completedCount: ?number,
    |},
  |}
|};
export type MarkAllTodosMutation = {|
  variables: MarkAllTodosMutationVariables,
  response: MarkAllTodosMutationResponse,
|};
*/


/*
mutation MarkAllTodosMutation(
  $input: MarkAllTodosInput!
) {
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
*/

const node/*: ConcreteRequest*/ = (function(){
var v0 = [
  {
    "defaultValue": null,
    "kind": "LocalArgument",
    "name": "input"
  }
],
v1 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "id",
  "storageKey": null
},
v2 = [
  {
    "alias": null,
    "args": [
      {
        "kind": "Variable",
        "name": "input",
        "variableName": "input"
      }
    ],
    "concreteType": "MarkAllTodosPayload",
    "kind": "LinkedField",
    "name": "markAllTodos",
    "plural": false,
    "selections": [
      {
        "alias": null,
        "args": null,
        "concreteType": "Todo",
        "kind": "LinkedField",
        "name": "changedTodos",
        "plural": true,
        "selections": [
          (v1/*: any*/),
          {
            "alias": null,
            "args": null,
            "kind": "ScalarField",
            "name": "complete",
            "storageKey": null
          }
        ],
        "storageKey": null
      },
      {
        "alias": null,
        "args": null,
        "concreteType": "User",
        "kind": "LinkedField",
        "name": "viewer",
        "plural": false,
        "selections": [
          (v1/*: any*/),
          {
            "alias": null,
            "args": null,
            "kind": "ScalarField",
            "name": "completedCount",
            "storageKey": null
          }
        ],
        "storageKey": null
      }
    ],
    "storageKey": null
  }
];
return {
  "fragment": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Fragment",
    "metadata": null,
    "name": "MarkAllTodosMutation",
    "selections": (v2/*: any*/),
    "type": "TodoMutation",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Operation",
    "name": "MarkAllTodosMutation",
    "selections": (v2/*: any*/)
  },
  "params": {
    "cacheID": "d46e7ec7a20692a47ac3fc3dfb222488",
    "id": null,
    "metadata": {},
    "name": "MarkAllTodosMutation",
    "operationKind": "mutation",
    "text": "mutation MarkAllTodosMutation(\n  $input: MarkAllTodosInput!\n) {\n  markAllTodos(input: $input) {\n    changedTodos {\n      id\n      complete\n    }\n    viewer {\n      id\n      completedCount\n    }\n  }\n}\n"
  }
};
})();
// prettier-ignore
(node/*: any*/).hash = '00fd81d60a24546c792660837e3fc6bd';

module.exports = node;
