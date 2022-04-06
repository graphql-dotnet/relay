using System.Collections.Generic;
using System.Linq;
using GraphQL.Builders;
using GraphQL.Relay.Extensions;
using GraphQL.Relay.Utilities;

namespace GraphQL.Relay.Types
{
    /// <summary>
    /// Factory methods for <see cref="SliceMetrics{TSource}"/>
    /// </summary>
    public static class SliceMetrics
    {
        /// <summary>
        /// Factory method to create an <see cref="SliceMetrics{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="context"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static SliceMetrics<TSource> Create<TSource>(
            IEnumerable<TSource> source,
            IResolveConnectionContext context,
            int? totalCount = null
        )
        {
            var totalSourceRowCount = totalCount ?? source.Count();
            var edges = context.EdgesToReturn(totalSourceRowCount);
            var nodes = source.Skip(edges.StartOffset).Take(edges.Count);

            return new(
                source: nodes.ToList(),
                edges,
                totalSourceRowCount
            );
        }

        /// <summary>
        /// Factory method to create an <see cref="SliceMetrics{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static SliceMetrics<TSource> Create<TSource>(
            IQueryable<TSource> source,
            IResolveConnectionContext context
        )
        {
            var totalCount = source.Count();
            var edges = context.EdgesToReturn(totalCount);
            var nodes = source.Skip(edges.StartOffset).Take(edges.Count);

            return new(
                source: nodes.ToList(),
                edges,
                totalCount
            );
        }
    }

    /// <summary>
    /// Relay connection slice metrics
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public class SliceMetrics<TSource>
    {
        /// <summary>
        /// The Total number of items in outer list. May be >= the SliceSize
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// The local total of the list slice.
        /// </summary>
        public int SliceSize { get; }

        /// <summary>
        /// Starting index for a slice of an IEnumerable item source
        /// </summary>
        public int StartIndex { get; }

        /// <summary>
        /// When a slice of a larger IEnumerable source has any records before it, this will be true
        /// </summary>
        public bool HasPrevious { get; }

        /// <summary>
        /// When a slice of a larger IEnumerable source has any records after it, this will be true
        /// </summary>
        public bool HasNext { get; }

        /// <summary>
        /// A particular section of a larger IEnumerable source
        /// </summary>
        public IEnumerable<TSource> Slice { get; }

        /// <summary>
        /// Constructor for relay connection slice metrics
        /// </summary>
        /// <param name="source"></param>
        /// <param name="edges"></param>
        /// <param name="totalCount"></param>
        ///
        public SliceMetrics(
            IList<TSource> source,
            EdgeRange edges,
            int totalCount
        )
        {
            TotalCount = totalCount;

            Slice = source;
            SliceSize = source.Count;

            HasNext = (edges.StartOffset + SliceSize) < TotalCount;
            HasPrevious = edges.StartOffset > 0;

            StartIndex = edges.StartOffset;
        }
    }
}