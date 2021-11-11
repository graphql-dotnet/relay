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
                .ResolveApiConnection<Films>(api);

            Connection<PeopleGraphType>()
                .Name("people")
                .ResolveApiConnection<People>(api);

            Connection<PlanetGraphType>()
                .Name("planets")
                .ResolveApiConnection<Planets>(api);

            Connection<SpeciesGraphType>()
                .Name("species")
                .ResolveApiConnection<Species>(api);

            Connection<StarshipGraphType>()
                .Name("starships")
                .ResolveApiConnection<Starships>(api);

            Connection<VehicleGraphType>()
                .Name("vehicles")
                .ResolveApiConnection<Vehicles>(api);
        }
    }
}