using GraphQL.Builders;
using GraphQL.Relay.Types;
using GraphQL.Types.Relay.DataObjects;

namespace GraphQL.Relay.Utilities
{
    /// <summary>
    /// Provide extension methods for <see cref="IResolveConnectionContext{TSource}"/>.
    /// </summary>
    public static class ResolveConnectionContextExtensions
    {
        /// <summary>
        /// Calculates an <see cref="EdgeRange"/> object based on the current
        /// <see cref="IResolveConnectionContext"/> and the provided edge items count
        /// </summary>
        /// <param name="context"></param>
        /// <param name="edgeCount"></param>
        /// <returns></returns>
        public static EdgeRange EdgesToReturn(
            this IResolveConnectionContext context,
            int edgeCount
        )
        {
            var first = (!context.First.HasValue && !context.Last.HasValue)
                ? (context.PageSize ?? edgeCount)
                : default(int?);

            return RelayPagination.CalculateEdgeRange(
                edgeCount,
                first ?? context.First,
                context.After,
                context.Last,
                context.Before
            );
        }

        /// <summary>
        /// From the connection context, <see cref="IResolveConnectionContext"/>,
        /// it creates a <see cref="Connection{TSource}"/> based on the given <see cref="IQueryable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="context"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public static Connection<TSource> ToConnection<TSource>(
            this IResolveConnectionContext context,
            IQueryable<TSource> items
        ) => SliceMetrics.Create(items, context).ToConnection();

        /// <summary>
        /// From the connection context, <see cref="IResolveConnectionContext"/>,
        /// it creates a <see cref="Connection{TSource}"/> based on the given <see cref="IEnumerable{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="context"></param>
        /// <param name="items"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static Connection<TSource> ToConnection<TSource>(
            this IResolveConnectionContext context,
            IEnumerable<TSource> items,
            int? totalCount = null
        ) => SliceMetrics.Create(items, context, totalCount).ToConnection();
    }
}
