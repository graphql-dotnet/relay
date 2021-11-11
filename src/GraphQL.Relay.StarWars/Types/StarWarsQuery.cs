using GraphQL.Relay.StarWars.Api;
using GraphQL.Relay.StarWars.Utilities;
using GraphQL.Relay.Types;

namespace GraphQL.Relay.StarWars.Types
{
    public class StarWarsQuery : QueryGraphType
    {
        public StarWarsQuery(Swapi api)
        {
            Name = "StarWarsQuery";

            Connection<FilmGraphType>()
                .Name("films")
                .Bidirectional()
                .ResolveApiConnection<Films>(api);

            Connection<PeopleGraphType>()
                .Name("people")
                .Bidirectional()
                .ResolveApiConnection<People>(api);

            Connection<PlanetGraphType>()
                .Name("planets")
                .Bidirectional()
                .ResolveApiConnection<Planets>(api);

            Connection<SpeciesGraphType>()
                .Name("species")
                .Bidirectional()
                .ResolveApiConnection<Species>(api);

            Connection<StarshipGraphType>()
                .Name("starships")
                .Bidirectional()
                .ResolveApiConnection<Starships>(api);

            Connection<VehicleGraphType>()
                .Name("vehicles")
                .Bidirectional()
                .ResolveApiConnection<Vehicles>(api);
        }
    }
}