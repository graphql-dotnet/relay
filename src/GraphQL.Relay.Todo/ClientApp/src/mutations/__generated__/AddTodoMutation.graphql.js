/**
 * @flow
 */

/* eslint-disable */

'use strict';

/*::
import type { ConcreteRequest } from 'relay-runtime';
export type AddTodoInput = {|
  text?: ?string
|};
export type AddTodoMutationVariables = {|
  input: AddTodoInput
|};
export type AddTodoMutationResponse = {|
  +addTodo: ?{|
    +todoEdge: ?{|
      +__typename: string,
      +cursor: string,
      +node: ?{|
        +id: string,
        +complete: boolean,
        +text: string,
      |},
    |},
    +viewer: ?{|
      +id: string,
      +totalCount: ?number,
    |},
  |}
|};
export type AddTodoMutation = {|
  variables: AddTodoMutationVariables,
  response: AddTodoMutationResponse,
|};
*/


/*
mutation AddTodoMutation(
  $input: AddTodoInput!
) {
  addTodo(input: $input) {
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
    "concreteType": "AddTodoPayload",
    "kind": "LinkedField",
    "name": "addTodo",
    "plural": false,
    "selections": [
      {
        "alias": null,
        "args": null,
        "concreteType": "TodoEdge",
        "kind": "LinkedField",
        "name": "todoEdge",
        "plural": false,
        "selections": [
          {
            "alias": null,
            "args": null,
            "kind": "ScalarField",
            "name": "__typename",
            "storageKey": null
          },
          {
            "alias": null,
            "args": null,
            "kind": "ScalarField",
            "name": "cursor",
            "storageKey": null
          },
          {
            "alias": null,
            "args": null,
            "concreteType": "Todo",
            "kind": "LinkedField",
            "name": "node",
            "plural": false,
            "selections": [
              (v1/*: any*/),
              {
                "alias": null,
                "args": null,
                "kind": "ScalarField",
                "name": "complete",
                "storageKey": null
              },
              {
                "alias": null,
                "args": null,
                "kind": "ScalarField",
                "name": "text",
                "storageKey": null
              }
            ],
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
            "name": "totalCount",
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
    "name": "AddTodoMutation",
    "selections": (v2/*: any*/),
    "type": "Mutation",
    "abstractKey": null
  },
  "kind": "Request",
  "operation": {
    "argumentDefinitions": (v0/*: any*/),
    "kind": "Operation",
    "name": "AddTodoMutation",
    "selections": (v2/*: any*/)
  },
  "params": {
    "cacheID": "c5988a12447e37d0bb3233b9ba9276d0",
    "id": null,
    "metadata": {},
    "name": "AddTodoMutation",
    "operationKind": "mutation",
    "text": "mutation AddTodoMutation(\n  $input: AddTodoInput!\n) {\n  addTodo(input: $input) {\n    todoEdge {\n      __typename\n      cursor\n      node {\n        id\n        complete\n        text\n      }\n    }\n    viewer {\n      id\n      totalCount\n    }\n  }\n}\n"
  }
};
})();
// prettier-ignore
(node/*: any*/).hash = 'e7e7cfa52aac36299ff22a59935deb67';

module.exports = node;
