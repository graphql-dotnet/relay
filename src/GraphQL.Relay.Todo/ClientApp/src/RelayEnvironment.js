import { Environment, Network, RecordSource, Store } from 'relay-runtime';
import fetchQuery from './fetchQuery';

async function fetchRelay(params, variables) {
    console.log(`fetching query ${params.name} with ${JSON.stringify(variables)}`);
    return fetchQuery(params.text, variables);
}

export default new Environment({
    network: Network.create(fetchRelay),
    store: new Store(new RecordSource()),
});