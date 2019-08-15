using GraphQL.Relay.StarWars.Api;
using GraphQL.Relay.Types;
using System.Threading.Tasks;

namespace GraphQL.Relay.StarWars.Types
{
  public class StarshipGraphType : NodeGraphType<Starships, Task<Starships>>
  {
    private readonly Swapi _api;
    public StarshipGraphType(Swapi api)
    {
        _api = api;

        Name = "Starship";

        Id(p => p.Id);
        Field(p => p.Name);
        Field(p => p.Model);
        Field(p => p.Manufacturer);
        Field(p => p.CostInCredits);
        Field(p => p.Length);
        Field(p => p.MaxAtmospheringSpeed);
        Field(p => p.Crew);
        Field(p => p.Passengers);
        Field(p => p.CargoCapacity);
        Field(p => p.Consumables);
        Field(p => p.HyperdriveRating);
        Field(p => p.MGLT);
        Field(p => p.StarshipClass);

        Connection<PeopleGraphType>()
            .Name("pilots")
            .Unidirectional()
            .Resolve(ctx => api
                .GetMany<People>(ctx.Source.Pilots)
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

    public override Task<Starships> GetById(string id) =>
        _api.GetEntity<Starships>(id);
  }
}