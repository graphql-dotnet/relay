using GraphQL.Relay.Extensions;
using GraphQL.Relay.StarWars.Api;
using GraphQL.Relay.Types;

namespace GraphQL.Relay.StarWars.Types
{
    public class PeopleGraphType : NodeGraphType<People, Task<People>>
    {
        private readonly Swapi _api;

        public PeopleGraphType(Swapi api)
        {
            _api = api;

            Name = "People";

            Id(p => p.Id);
            Field(p => p.Name);
            Field(p => p.Height);
            Field(p => p.Mass);
            Field(p => p.HairColor);
            Field(p => p.SkinColor);
            Field(p => p.EyeColor);
            Field(p => p.BirthYear);
            Field(p => p.Gender);
            FieldAsync(
              name: "homeworld",
              type: typeof(PlanetGraphType),
              resolve: async ctx => await _api.GetEntity<Planets>(ctx.Source.Homeworld)
            );

            Connection<FilmGraphType>()
                .Name("films")
                .ResolveAsync(async ctx => await api
                    .GetMany<Films>(ctx.Source.Films)
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

        public override Task<People> GetById(IResolveFieldContext<object> context, string id) =>
            _api.GetEntityAsync<People>(id);
    }
}
