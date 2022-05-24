using GraphQL.Relay.StarWars.Api;
using GraphQL.Relay.Types;
using GraphQL.Relay.Utilities;

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
                    .ContinueWith(t => ctx.ToConnection(t.Result))
                );

            Connection<PlanetGraphType>()
                .Name("planets")
                .ResolveAsync(async ctx => await api
                    .GetMany<Planets>(ctx.Source.Planets)
                    .ContinueWith(t => ctx.ToConnection(t.Result))
                );

            Connection<SpeciesGraphType>()
                .Name("species")
                .ResolveAsync(async ctx => await api
                    .GetMany<Species>(ctx.Source.Species)
                    .ContinueWith(t => ctx.ToConnection(t.Result))
                );

            Connection<StarshipGraphType>()
                .Name("starships")
                .ResolveAsync(async ctx => await api
                    .GetMany<Starships>(ctx.Source.Starships)
                    .ContinueWith(t => ctx.ToConnection(t.Result))
                );

            Connection<VehicleGraphType>()
                .Name("vehicles")
                .ResolveAsync(async ctx => await api
                    .GetMany<Vehicles>(ctx.Source.Vehicles)
                    .ContinueWith(t => ctx.ToConnection(t.Result))
                );
        }

        public override Task<Films> GetById(IResolveFieldContext<object> context, string id) =>
            _api.GetEntityAsync<Films>(id);
    }
}
