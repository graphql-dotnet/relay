/**
 * @flow
 */

/* eslint-disable */

'use strict';

/*::
import type { ConcreteRequest } from 'relay-runtime';
export type RemoveCompletedTodosInput = {|
  complete?: ?boolean
|};
export type RemoveCompletedTodosMutationVariables = {|
  input: RemoveCompletedTodosInput
|};
export type RemoveCompletedTodosMutationResponse = {|
  +removeCompletedTodos: ?{|
    +deletedTodoIds: ?$ReadOnlyArray<?string>,
    +viewer: ?{|
      +completedCount: ?number,
      +totalCount: ?number,
    |},
  |}
|};
export type RemoveCompletedTodosMutation = {|
  variables: RemoveCompletedTodosMutationVariables,
  response: RemoveCompletedTodosMutationResponse,
|};
*/


/*
mutation RemoveCompletedTodosMutation(
  $input: RemoveCompletedTodosInput!
) {
  removeCompletedTodos(input: $input) {
    deletedTodoIds
    viewer {
      completedCount
      totalCount
      id
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
v1 = [
  {
    "kind": "Variable",
    "name": "input",
    "variableName": "input"
  }
],
v2 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "deletedTodoIds",
  "storageKey": null
},
v3 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "completedCount",
  "storageKey": null
},
v4 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "totalCount",
  "storageKey": null
};
return {
  "fragment": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Fragment",
    "metadata": null,
    "name": "RemoveCompletedTodosMutation",
    "selections": [
      {
        "alias": null,
        "args": (v1/*: any*/),
        "concreteType": "RemoveCompletedTodosPayload",
        "kind": "LinkedField",
        "name": "removeCompletedTodos",
        "plural": false,
        "selections": [
          (v2/*: any*/),
          {
            "alias": null,
            "args": null,
            "concreteType": "User",
            "kind": "LinkedField",
            "name": "viewer",
            "plural": false,
            "selections": [
              (v3/*: any*/),
              (v4/*: any*/)
            ],
            "storageKey": null
          }
        ],
        "storageKey": null
      }
    ],
    "type": "Mutation",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Operation",
    "name": "RemoveCompletedTodosMutation",
    "selections": [
      {
        "alias": null,
        "args": (v1/*: any*/),
        "concreteType": "RemoveCompletedTodosPayload",
        "kind": "LinkedField",
        "name": "removeCompletedTodos",
        "plural": false,
        "selections": [
          (v2/*: any*/),
          {
            "alias": null,
            "args": null,
            "concreteType": "User",
            "kind": "LinkedField",
            "name": "viewer",
            "plural": false,
            "selections": [
              (v3/*: any*/),
              (v4/*: any*/),
              {
                "alias": null,
                "args": null,
                "kind": "ScalarField",
                "name": "id",
                "storageKey": null
              }
            ],
            "storageKey": null
          }
        ],
        "storageKey": null
      }
    ]
  },
  "params": {
    "cacheID": "4e579d172ceaf5d902f3da90438a1cfe",
    "id": null,
    "metadata": {},
    "name": "RemoveCompletedTodosMutation",
    "operationKind": "mutation",
    "text": "mutation RemoveCompletedTodosMutation(\n  $input: RemoveCompletedTodosInput!\n) {\n  removeCompletedTodos(input: $input) {\n    deletedTodoIds\n    viewer {\n      completedCount\n      totalCount\n      id\n    }\n  }\n}\n"
  }
};
})();
// prettier-ignore
(node/*: any*/).hash = '303799d791e6e233861ee011ff3bdbb8';

module.exports = node;
