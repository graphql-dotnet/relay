using System.Collections.Generic;
using System.Linq;
using GraphQL.Builders;
using GraphQL.Relay.Types;
using GraphQL.Types.Relay.DataObjects;

namespace GraphQL.Relay.Extensions
{
    /// <summary>
    /// Provide extension methods for <see cref="IResolveConnectionContext{TSource}"/>.
    /// </summary>
    public static class ResolveConnectionContextExtensions
    {
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
        )
        {
            var metrics = EnumerableSliceMetrics.Create(
                items,
                context,
                totalCount
            );

            var edges = metrics.Slice
                .Select((item, i) => new Edge<TSource>
                {
                    Node = item,
                    Cursor = ConnectionUtils.OffsetToCursor(metrics.StartIndex + i)
                })
                .ToList();

            var firstEdge = edges.FirstOrDefault();
            var lastEdge = edges.LastOrDefault();

            return new Connection<TSource>
            {
                Edges = edges,
                TotalCount = metrics.TotalCount,
                PageInfo = new PageInfo
                {
                    StartCursor = firstEdge?.Cursor,
                    EndCursor = lastEdge?.Cursor,
                    HasPreviousPage = metrics.HasPrevious,
                    HasNextPage = metrics.HasNext,
                }
            };
        }
    }
}