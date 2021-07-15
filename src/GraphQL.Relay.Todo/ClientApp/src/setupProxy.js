const createProxyMiddleware = require('http-proxy-middleware');

const context =  [
    "/graphql",
    "/ui/graphiql"
];

module.exports = function(app) {
  const appProxy = createProxyMiddleware(context, {
    target: 'http://localhost:5000',
    secure: false
  });

  app.use(appProxy);
};
