using GraphQL.Relay.StarWars.Api;
using GraphQL.Relay.Types;
using System.Threading.Tasks;

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
            .Unidirectional()
            .Resolve(ctx => api
                .GetMany<People>(ctx.Source.Characters)
                .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
            );

        Connection<PlanetGraphType>()
            .Name("planets")
            .Unidirectional()
            .Resolve(ctx => api
                .GetMany<Planets>(ctx.Source.Planets)
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

    public override Task<Films> GetById(string id) =>
        _api.GetEntity<Films>(id);
  }
}