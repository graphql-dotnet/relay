
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
                .ResolveAsync(async ctx => await api
                    .GetConnection<Films>(ctx.GetConnectionArguments())
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
                );

            Connection<PeopleGraphType>()
                .Name("people")
                .ResolveAsync(async ctx => await api
                    .GetConnection<People>(ctx.GetConnectionArguments())
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
                );

            Connection<PlanetGraphType>()
                .Name("planets")
                .ResolveAsync(async ctx => await api
                    .GetConnection<Planets>(ctx.GetConnectionArguments())
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
                );

            Connection<SpeciesGraphType>()
                .Name("species")
                .ResolveAsync(async ctx => await api
                    .GetConnection<Species>(ctx.GetConnectionArguments())
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
                );

            Connection<StarshipGraphType>()
                .Name("starships")
                .ResolveAsync(async ctx => await api
                    .GetConnection<Starships>(ctx.GetConnectionArguments())
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
                );

            Connection<VehicleGraphType>()
                .Name("vehicles")
                .ResolveAsync(async ctx => await api
                    .GetConnection<Vehicles>(ctx.GetConnectionArguments())
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
                );
        }
    }
}
