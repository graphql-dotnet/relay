using GraphQL.Http;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Relay.Todo
{
    public class SchemaWriter
    {
        private readonly IDocumentWriter writer = new DocumentWriter();
        private readonly IDocumentExecuter executor;
        private readonly GraphQL.Types.Schema _schema;

        public SchemaWriter(GraphQL.Types.Schema schema)
        {
            executor = new DocumentExecuter();
            _schema = schema;
        }

        public async Task<string> Generate()
        {
            ExecutionResult result = await executor.ExecuteAsync(options =>
            {
                options.Schema = _schema;
                options.Query = introspectionQuery;
            });

            if (result.Errors?.Any() ?? false)
                throw result.Errors.First();

            return await writer.WriteToStringAsync(result);
        }

        private const string introspectionQuery = @"
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
