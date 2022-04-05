using GraphQL.SystemTextJson;

namespace GraphQL.Relay.Todo
{
    public class SchemaWriter
    {
        private readonly IGraphQLTextSerializer _serializer = new GraphQLSerializer();
        private readonly IDocumentExecuter _executor;
        private readonly GraphQL.Types.Schema _schema;

        public SchemaWriter(GraphQL.Types.Schema schema)
        {
            _executor = new DocumentExecuter();
            _schema = schema;
        }

        public async Task<string> GenerateAsync()
        {
            var result = await _executor.ExecuteAsync(
                new ExecutionOptions
                {
                    Schema = _schema,
                    Query = _introspectionQuery
                }
            );

            if (result.Errors?.Any() ?? false)
                throw result.Errors.First();

            return _serializer.Serialize(result);
        }

        private readonly string _introspectionQuery = @"
          query IntrospectionQuery {
            __schema {
              queryType { name }
              mutationType { name }
              subscriptionType { name }
              types {
                ...FullType
              }
              directives {
                name
                description
                locations
                args {
                  ...InputValue
                }
              }
            }
          }
          fragment FullType on __Type {
            kind
            name
            description
            fields(includeDeprecated: true) {
              name
              description
              args {
                ...InputValue
              }
              type {
                ...TypeRef
              }
              isDeprecated
              deprecationReason
            }
            inputFields {
              ...InputValue
            }
            interfaces {
              ...TypeRef
            }
            enumValues(includeDeprecated: true) {
              name
              description
              isDeprecated
              deprecationReason
            }
            possibleTypes {
              ...TypeRef
            }
          }
          fragment InputValue on __InputValue {
            name
            description
            type { ...TypeRef }
            defaultValue
          }
          fragment TypeRef on __Type {
            kind
            name
            ofType {
              kind
              name
              ofType {
                kind
                name
                ofType {
                  kind
                  name
                  ofType {
                    kind
                    name
                    ofType {
                      kind
                      name
                      ofType {
                        kind
                        name
                        ofType {
                          kind
                          name
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        ";
    }
}
