using System;
using GraphQL.Types;
using GraphQL.Utilities;

namespace GraphQL.Relay.StarWars.Types
{
    public class StarWarsSchema: Schema
    {
        public StarWarsSchema(IServiceProvider provider)
            : base(provider)
        {
            Query = provider.GetRequiredService<StarWarsQuery>();

            RegisterType<FilmGraphType>();
            RegisterType<PeopleGraphType>();
            RegisterType<PlanetGraphType>();
            RegisterType<SpeciesGraphType>();
            RegisterType<StarshipGraphType>();
            RegisterType<VehicleGraphType>();
        }
    }
}