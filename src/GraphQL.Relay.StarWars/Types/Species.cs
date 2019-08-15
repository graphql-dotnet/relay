using GraphQL.Relay.StarWars.Api;
using GraphQL.Relay.Types;
using System.Threading.Tasks;

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
        Field(
          name: "homeworld",
          type: typeof(PlanetGraphType),
          resolve: ctx => _api.GetEntity<Planets>(ctx.Source.Homeworld)
        );

        Connection<PeopleGraphType>()
            .Name("people")
            .Unidirectional()
            .Resolve(ctx => api
                .GetMany<People>(ctx.Source.People)
                .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
            );

        Connection<FilmGraphType>()
            .Name("films")
            .Unidirectional()
            .Resolve(ctx => api
                .GetMany<Films>(ctx.Source.Films)
                .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
            );
    }

    public override Task<Species> GetById(string id) =>
        _api.GetEntity<Species>(id);

  }
}