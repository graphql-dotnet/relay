/**
 * @flow
 */

/* eslint-disable */

'use strict';

/*::
import type { ReaderFragment } from 'relay-runtime';
import type { FragmentReference } from "relay-runtime";
declare export opaque type Todo_viewer$ref: FragmentReference;
declare export opaque type Todo_viewer$fragmentType: Todo_viewer$ref;
export type Todo_viewer = {|
  +id: string,
  +totalCount: ?number,
  +completedCount: ?number,
  +$refType: Todo_viewer$ref,
|};
export type Todo_viewer$data = Todo_viewer;
export type Todo_viewer$key = {
  +$data?: Todo_viewer$data,
  +$fragmentRefs: Todo_viewer$ref,
  ...
};
*/


const node/*: ReaderFragment*/ = {
  "argumentDefinitions": [],
  "kind": "Fragment",
  "metadata": null,
  "name": "Todo_viewer",
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
      "alias": null,
      "args": null,
      "kind": "ScalarField",
      "name": "completedCount",
      "storageKey": null
    }
  ],
  "type": "User",
  "abstractKey": null
};
// prettier-ignore
(node/*: any*/).hash = '1e2b17bb7b92d4521c4e72309d996339';

module.exports = node;
