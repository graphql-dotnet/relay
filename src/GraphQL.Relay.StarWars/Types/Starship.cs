using GraphQL.Relay.StarWars.Api;
using GraphQL.Relay.Types;
using GraphQL.Relay.Utilities;

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
                .ResolveAsync(async ctx => await api
                    .GetMany<People>(ctx.Source.Pilots)
                    .ContinueWith(t => ctx.ToConnection(t.Result))
                );

            Connection<FilmGraphType>()
                .Name("films")
                .ResolveAsync(async ctx => await api
                    .GetMany<Films>(ctx.Source.Films)
                    .ContinueWith(t => ctx.ToConnection(t.Result))
                );
        }

        public override Task<Starships> GetById(IResolveFieldContext<object> context, string id) =>
            _api.GetEntityAsync<Starships>(id);
    }
}
