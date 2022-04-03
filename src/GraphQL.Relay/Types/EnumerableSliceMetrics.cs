using System.Collections.Generic;
using System.Linq;
using GraphQL.Builders;
using GraphQL.Relay.Extensions;
using GraphQL.Relay.Utilities;

namespace GraphQL.Relay.Types
{
    /// <summary>
    /// Factory methods for <see cref="EnumerableSliceMetrics{TSource}"/>
    /// </summary>
    public static class EnumerableSliceMetrics
    {
        /// <summary>
        /// When a page size is not specified, the default page size is 25.
        /// </summary>
        public static int DefaultPageSize { get; set; } = 25;

        /// <summary>
        /// Factory method to create an <see cref="EnumerableSliceMetrics{TSource}"/>
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="items"></param>
        /// <param name="context"></param>
        /// <param name="totalCount"></param>
        /// <param name="strictCheck"></param>
        /// <returns></returns>
        public static EnumerableSliceMetrics<TSource> Create<TSource>(
            IEnumerable<TSource> items,
            IResolveConnectionContext context,
            int? totalCount = null,
            bool strictCheck = true
        ) => new(items, context, totalCount, strictCheck);
    }

    /// <summary>
    /// Relay connection slice metrics
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public class EnumerableSliceMetrics<TSource>
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
        /// Ending index for a slice of an IEnumerable item source
        /// </summary>
        public int EndIndex { get; }

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
        /// <param name="itemSource"></param>
        /// <param name="connectContext"></param>
        /// <param name="totalCount"></param>
        /// <param name="strictCheck"></param>
        ///
        public EnumerableSliceMetrics(
            IEnumerable<TSource> itemSource,
            IResolveConnectionContext connectContext,
            int? totalCount = null,
            bool strictCheck = true
        )
        {
            TotalCount = totalCount ?? itemSource.Count();

            var edges = connectContext.EdgesToReturn(TotalCount);
            var slice = itemSource.Skip(edges.StartOffset).Take(edges.Count).ToList();

            Slice = slice;
            SliceSize = slice.Count;

            HasNext = (edges.StartOffset + SliceSize) < TotalCount;
            HasPrevious = edges.StartOffset > 0;

            StartIndex = edges.StartOffset;
            EndIndex = StartIndex + SliceSize - 1;

            if (strictCheck)
            {
                RelayPagination.EnsureSliceCoversRange(
                    nameof(itemSource),
                    edges,
                    StartIndex,
                    EndIndex
                );
            }
        }
    }
}