
function graphQLFetcher(graphQLParams) {
  return fetch(window.location.origin + '/api/graphql', {
    method: 'post',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(graphQLParams),
  }).then(response => response.json());
}

ReactDOM.render(
  React.createElement(GraphiQL, { fetcher: graphQLFetcher }, null),
  document.getElementById('root')
);
