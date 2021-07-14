/**
 * @flow
 */

/* eslint-disable */

'use strict';

/*::
import type { ReaderFragment } from 'relay-runtime';
import type { FragmentReference } from "relay-runtime";
declare export opaque type TodoListFooter_viewer$ref: FragmentReference;
declare export opaque type TodoListFooter_viewer$fragmentType: TodoListFooter_viewer$ref;
export type TodoListFooter_viewer = {|
  +id: string,
  +completedCount: ?number,
  +completedTodos: ?{|
    +edges: ?$ReadOnlyArray<?{|
      +node: ?{|
        +id: string,
        +complete: boolean,
      |}
    |}>
  |},
  +totalCount: ?number,
  +$refType: TodoListFooter_viewer$ref,
|};
export type TodoListFooter_viewer$data = TodoListFooter_viewer;
export type TodoListFooter_viewer$key = {
  +$data?: TodoListFooter_viewer$data,
  +$fragmentRefs: TodoListFooter_viewer$ref,
  ...
};
*/


const node/*: ReaderFragment*/ = (function(){
var v0 = {
  "alias": null,
  "args": null,
  "kind": "ScalarField",
  "name": "id",
  "storageKey": null
};
return {
  "argumentDefinitions": [],
  "kind": "Fragment",
  "metadata": null,
  "name": "TodoListFooter_viewer",
  "selections": [
    (v0/*: any*/),
    {
      "alias": null,
      "args": null,
      "kind": "ScalarField",
      "name": "completedCount",
      "storageKey": null
    },
    {
      "alias": "completedTodos",
      "args": [
        {
          "kind": "Literal",
          "name": "first",
          "value": 2147483647
        },
        {
          "kind": "Literal",
          "name": "status",
          "value": "completed"
        }
      ],
      "concreteType": "TodoConnection",
      "kind": "LinkedField",
      "name": "todos",
      "plural": false,
      "selections": [
        {
          "alias": null,
          "args": null,
          "concreteType": "TodoEdge",
          "kind": "LinkedField",
          "name": "edges",
          "plural": true,
          "selections": [
            {
              "alias": null,
              "args": null,
              "concreteType": "Todo",
              "kind": "LinkedField",
              "name": "node",
              "plural": false,
              "selections": [
                (v0/*: any*/),
                {
                  "alias": null,
                  "args": null,
                  "kind": "ScalarField",
                  "name": "complete",
                  "storageKey": null
                }
              ],
              "storageKey": null
            }
          ],
          "storageKey": null
        }
      ],
      "storageKey": "todos(first:2147483647,status:\"completed\")"
    },
    {
      "alias": null,
      "args": null,
      "kind": "ScalarField",
      "name": "totalCount",
      "storageKey": null
    }
  ],
  "type": "User",
  "abstractKey": null
};
})();
// prettier-ignore
(node/*: any*/).hash = '2490c58e1768d71f3824c1facd127033';

module.exports = node;
