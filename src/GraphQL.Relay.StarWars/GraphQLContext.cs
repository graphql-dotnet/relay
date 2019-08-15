using GraphQL.Relay.StarWars.Api;
using GraphQL.Types;
using System.Collections.Generic;

namespace GraphQL.Relay.StarWars
{
    public class GraphQLContext : Dictionary<string, object>
    {
        public Swapi Api { get; }
        public GraphQLContext(Swapi api)
        {
            Api = api;
        }
    }

    public static class GraphQLUserContextExtensions
    {
        public static Swapi Api<TSource>(this ResolveFieldContext<TSource> context)
        {
            return ((GraphQLContext)context.UserContext).Api;
        }

        // public static IDataLoader<int, TReturn> GetDataLoader<TSource, TReturn>(
        //     this ResolveFieldContext<TSource> context,
        //     Func<IEnumerable<int>, Task<ILookup<int, TReturn>>> fetchDelegate
        // ) {
        //     return ((GraphQLContext)context.UserContext).LoadContext.GetOrCreateLoader(context.FieldDefinition, fetchDelegate);
        // }
    }
}