using GraphQL.Relay.StarWars.Api;
using GraphQL.Relay.Types;
using GraphQL.Relay.Utilities;

namespace GraphQL.Relay.StarWars.Types
{
    public class VehicleGraphType : NodeGraphType<Vehicles, Task<Vehicles>>
    {
        private readonly Swapi _api;

        public VehicleGraphType(Swapi api)
        {
            _api = api;

            Name = "Vehicle";

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
            Field(p => p.VehicleClass);
            Field(p => p.Consumables);

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

        public override Task<Vehicles> GetById(IResolveFieldContext<object> context, string id) =>
            _api.GetEntityAsync<Vehicles>(id);
    }
}
