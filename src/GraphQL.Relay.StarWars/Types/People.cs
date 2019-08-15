using GraphQL.Relay.StarWars.Api;
using GraphQL.Relay.Types;
using System.Threading.Tasks;

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
        Field(
          name: "homeworld",
          type: typeof(PlanetGraphType),
          resolve: ctx => _api.GetEntity<Planets>(ctx.Source.Homeworld)
        );

        Connection<FilmGraphType>()
            .Name("films")
            .Unidirectional()
            .Resolve(ctx => api
                .GetMany<Films>(ctx.Source.Films)
                .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
            );


        Connection<SpeciesGraphType>()
            .Name("species")
            .Unidirectional()
            .Resolve(ctx => api
                .GetMany<Species>(ctx.Source.Species)
                .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
            );

        Connection<StarshipGraphType>()
            .Name("starships")
            .Unidirectional()
            .Resolve(ctx => api
                .GetMany<Starships>(ctx.Source.Starships)
                .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
            );

        Connection<VehicleGraphType>()
            .Name("vehicles")
            .Unidirectional()
            .Resolve(ctx => api
                .GetMany<Vehicles>(ctx.Source.Vehicles)
                .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
            );
    }

    public override Task<People> GetById(string id) =>
        _api.GetEntity<People>(id);

  }
}