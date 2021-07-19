/**
 * @flow
 */

/* eslint-disable */

'use strict';

/*::
import type { ReaderFragment } from 'relay-runtime';
type TodoListFooter_viewer$ref = any;
type TodoList_viewer$ref = any;
import type { FragmentReference } from "relay-runtime";
declare export opaque type TodoApp_viewer$ref: FragmentReference;
declare export opaque type TodoApp_viewer$fragmentType: TodoApp_viewer$ref;
export type TodoApp_viewer = {|
  +id: string,
  +totalCount: ?number,
  +$fragmentRefs: TodoListFooter_viewer$ref & TodoList_viewer$ref,
  +$refType: TodoApp_viewer$ref,
|};
export type TodoApp_viewer$data = TodoApp_viewer;
export type TodoApp_viewer$key = {
  +$data?: TodoApp_viewer$data,
  +$fragmentRefs: TodoApp_viewer$ref,
  ...
};
*/


const node/*: ReaderFragment*/ = {
  "argumentDefinitions": [],
  "kind": "Fragment",
  "metadata": null,
  "name": "TodoApp_viewer",
  "selections": [
    {
      "alias": null,
      "args": null,
      "kind": "ScalarField",
      "name": "id",
      "storageKey": null
    },
    {
      "alias": null,
      "args": null,
      "kind": "ScalarField",
      "name": "totalCount",
      "storageKey": null
    },
    {
      "args": null,
      "kind": "FragmentSpread",
      "name": "TodoListFooter_viewer"
    },
    {
      "args": null,
      "kind": "FragmentSpread",
      "name": "TodoList_viewer"
    }
  ],
  "type": "User",
  "abstractKey": null
};
// prettier-ignore
(node/*: any*/).hash = 'b9743417c7b5ef2bbda96cf675aa9eb4';

module.exports = node;
