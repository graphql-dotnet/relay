using GraphQL.Relay.Extensions;
using GraphQL.Relay.StarWars.Api;
using GraphQL.Relay.Types;

namespace GraphQL.Relay.StarWars.Types
{
    public class SpeciesGraphType : NodeGraphType<Species, Task<Species>>
    {
        private readonly Swapi _api;

        public SpeciesGraphType(Swapi api)
        {
            _api = api;

            Name = "Species";

            Id(p => p.Id);
            Field(p => p.Name);

            Field(p => p.Classification);
            Field(p => p.Designation);
            Field(p => p.AverageHeight);
            Field(p => p.SkinColors);
            Field(p => p.HairColors);
            Field(p => p.EyeColors);
            Field(p => p.AverageLifespan);
            Field(p => p.Language);
            FieldAsync(
              name: "homeworld",
              type: typeof(PlanetGraphType),
              resolve: async ctx => await _api.GetEntity<Planets>(ctx.Source.Homeworld)
            );

            Connection<PeopleGraphType>()
                .Name("people")
                .ResolveAsync(async ctx => await api
                    .GetMany<People>(ctx.Source.People)
                    .ContinueWith(t => ctx.ToConnection(t.Result))
                );

            Connection<FilmGraphType>()
                .Name("films")
                .ResolveAsync(async ctx => await api
                    .GetMany<Films>(ctx.Source.Films)
                    .ContinueWith(t => ctx.ToConnection(t.Result))
                );
        }

        public override Task<Species> GetById(IResolveFieldContext<object> context, string id) =>
            _api.GetEntityAsync<Species>(id);
    }
}
