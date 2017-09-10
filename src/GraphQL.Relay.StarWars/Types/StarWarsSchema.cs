using System;
using GraphQL.Types;

namespace GraphQL.Relay.StarWars.Types
{
    public class StarWarsSchema: Schema
    {
        public StarWarsSchema(Func<Type, object> resolveType)
            : base(type => (GraphType)resolveType(type))
        {
            var obj = resolveType(typeof(StarWarsQuery));
            Query = obj as StarWarsQuery;

            RegisterType<FilmGraphType>();
            RegisterType<PeopleGraphType>();
            RegisterType<PlanetGraphType>();
            RegisterType<SpeciesGraphType>();
            RegisterType<StarshipGraphType>();
            RegisterType<VehicleGraphType>();
        }
    }
}