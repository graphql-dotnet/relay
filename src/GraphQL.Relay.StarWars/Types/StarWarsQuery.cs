
using GraphQL.Relay.StarWars.Api;

namespace GraphQL.Relay.StarWars.Types
{
    public class StarWarsQuery: GraphQL.Relay.Types.QueryGraphType
    {
        public StarWarsQuery(Swapi api) {
            Name = "StarWarsQuery";
        }
    }
}