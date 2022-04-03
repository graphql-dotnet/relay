using GraphQL.Types;

namespace GraphQL.Relay.StarWars.Types
{
    public class StarWarsSchema : Schema
    {
        public StarWarsSchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetService<StarWarsQuery>();

            RegisterType(typeof(FilmGraphType));
            RegisterType(typeof(PeopleGraphType));
            RegisterType(typeof(PlanetGraphType));
            RegisterType(typeof(SpeciesGraphType));
            RegisterType(typeof(StarshipGraphType));
            RegisterType(typeof(VehicleGraphType));
        }
    }
}
