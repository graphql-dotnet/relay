using System.Threading.Tasks;
using GraphQL.Relay.StarWars.Api;
using GraphQL.Relay.Types;
using GraphQL.Types;

using ConnectionUtils = GraphQL.Relay.Types.Connection;

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
        Field(p => p.Homeworld);
        Field(p => p.Language);

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
        _api.Get<Species>(id);

  }
}