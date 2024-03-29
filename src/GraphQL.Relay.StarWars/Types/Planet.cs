using GraphQL.Relay.StarWars.Api;
using GraphQL.Relay.Types;
using GraphQL.Relay.Utilities;

namespace GraphQL.Relay.StarWars.Types
{
    public class PlanetGraphType : NodeGraphType<Planets, Task<Planets>>
    {
        private readonly Swapi _api;

        public PlanetGraphType(Swapi api)
        {
            _api = api;

            Name = "Planet";

            Id(p => p.Id);
            Field(p => p.Name);
            Field(p => p.RotationPeriod);
            Field(p => p.OrbitalPeriod);
            Field(p => p.Diameter);
            Field(p => p.Climate);
            Field(p => p.Gravity);
            Field(p => p.Terrain);
            Field(p => p.SurfaceWater);
            Field(p => p.Population);

            Connection<PeopleGraphType>()
                .Name("residents")
                .ResolveAsync(async ctx => await api
                    .GetMany<People>(ctx.Source.Residents)
                    .ContinueWith(t => ctx.ToConnection(t.Result))
                );

            Connection<FilmGraphType>()
                .Name("films")
                .ResolveAsync(async ctx => await api
                    .GetMany<Films>(ctx.Source.Films)
                    .ContinueWith(t => ctx.ToConnection(t.Result))
                );
        }

        public override Task<Planets> GetById(IResolveFieldContext<object> context, string id) =>
            _api.GetEntityAsync<Planets>(id);
    }
}
