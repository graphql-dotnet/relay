using GraphQL.Builders;
using GraphQL.Relay.Extensions;
using GraphQL.Relay.StarWars.Api;

namespace GraphQL.Relay.StarWars.Utilities
{
    public static class ConnectionBuilderExtensions
    {
        public static void ResolveApiConnection<TEntity>(
            this ConnectionBuilder<object> builder,
            Swapi api
        ) where TEntity : Entity
        {
            builder
                .Bidirectional()
                .Resolve(ctx => api
                    .GetConnectionAsync<TEntity>(ctx.GetConnectionArguments())
                    .ContinueWith(t => ctx.ToConnection(t.Result.Entities, t.Result.TotalCount))
            );
        }
    }
}
