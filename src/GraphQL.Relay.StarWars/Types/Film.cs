using GraphQL.Relay.StarWars.Api;
using GraphQL.Relay.Types;

namespace GraphQL.Relay.StarWars.Types
{
    public class FilmGraphType : AsyncNodeGraphType<Films>
    {
        private readonly Swapi _api;
        public FilmGraphType(Swapi api)
        {
            _api = api;

            Name = "Film";

            Id(p => p.Id);
            Field(p => p.Title);
            Field(p => p.EpisodeId);
            Field(p => p.OpeningCrawl);
            Field(p => p.Director);
            Field(p => p.Producer);
            Field(p => p.ReleaseDate);

            Connection<PeopleGraphType>()
                .Name("characters")
                .ResolveAsync(async ctx => await api
                    .GetMany<People>(ctx.Source.Characters)
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
                );

            Connection<PlanetGraphType>()
                .Name("planets")
                .ResolveAsync(async ctx => await api
                    .GetMany<Planets>(ctx.Source.Planets)
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
                );

            Connection<SpeciesGraphType>()
                .Name("species")
                .ResolveAsync(async ctx => await api
                    .GetMany<Species>(ctx.Source.Species)
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
                );

            Connection<StarshipGraphType>()
                .Name("starships")
                .ResolveAsync(async ctx => await api
                    .GetMany<Starships>(ctx.Source.Starships)
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
                );

            Connection<VehicleGraphType>()
                .Name("vehicles")
                .ResolveAsync(async ctx => await api
                    .GetMany<Vehicles>(ctx.Source.Vehicles)
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
                );
        }

        public override Task<Films> GetById(IResolveFieldContext<object> context, string id) =>
            _api.GetEntityAsync<Films>(id);
    }
}
