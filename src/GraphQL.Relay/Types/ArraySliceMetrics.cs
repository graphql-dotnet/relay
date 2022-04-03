using GraphQL.Builders;
using GraphQL.Relay.Utilities;

namespace GraphQL.Relay.Types
{
    /// <summary>
    /// Factory methods for <see cref="ArraySliceMetrics{TSource}"/>
    /// </summary>
    public static class ArraySliceMetrics
    {
        public static ArraySliceMetrics<TSource> Create<TSource>(
            IList<TSource> slice,
            int? first = null,
            string after = null,
            int? last = null,
            string before = null,
            bool strictCheck = true
        )
        {
            return new ArraySliceMetrics<TSource>(slice, first, after, last, before, strictCheck);
        }

        public static ArraySliceMetrics<TSource> Create<TSource>(
            IList<TSource> slice,
            int sliceStartIndex,
            int totalCount,
            int? first = null,
            string after = null,
            int? last = null,
            string before = null,
            bool strictCheck = true
        )
        {
            return new ArraySliceMetrics<TSource>(slice, first, after, last, before, sliceStartIndex, totalCount,
                strictCheck);
        }

        public static ArraySliceMetrics<TSource> Create<TSource, TParent>(
            IList<TSource> slice,
            ResolveConnectionContext<TParent> context,
            bool strictCheck = true
        )
        {
            return new ArraySliceMetrics<TSource>(slice, context.First, context.After, context.Last, context.Before, strictCheck);
        }

        public static ArraySliceMetrics<TSource> Create<TSource, TParent>(
            IList<TSource> slice,
            IResolveConnectionContext<TParent> context,
            int sliceStartIndex,
            int totalCount,
            bool strictCheck = true
        )
        {
            return new ArraySliceMetrics<TSource>(slice, context.First, context.After, context.Last, context.Before,
                sliceStartIndex, totalCount, strictCheck);
        }
    }

    public class ArraySliceMetrics<TSource>
    {
        private readonly IList<TSource> _items;

        /// <summary>
        /// The Total number of items in outer list. May be >= the SliceSize
        /// </summary>
        public int TotalCount { get; }

        /// <summary>
        /// The local total of the list slice.
        /// </summary>
        public int SliceSize => _items.Count;

        /// <summary>
        /// The start index of the slice within the larger List
        /// </summary>
        /// <returns></returns>
        public int StartIndex { get; }

        /// <summary>
        /// The end index of the slice within the larger List
        /// </summary>
        public int EndIndex => StartIndex + SliceSize - 1;

        public int FirstValidOffset => 0;
        public int LastValidOffset => TotalCount - 1;
        public int StartOffset { get; }
        public int EndOffset { get; }
        public bool HasPrevious { get; }
        public bool HasNext { get; }

        public bool IsEmpty => EndOffset < StartOffset;

        public IEnumerable<TSource> Slice => _items.Slice(
            Math.Max(StartOffset - StartIndex, 0),
            SliceSize - (EndIndex - EndOffset)
        );

        public ArraySliceMetrics(
            IList<TSource> slice,
            int? first,
            string after,
            int? last,
            string before,
            bool strictCheck = true
        ) : this(slice, first, after, last, before, 0, slice.Count, strictCheck)
        {
        }

        public ArraySliceMetrics(
            IList<TSource> slice,
            int? first,
            string after,
            int? last,
            string before,
            int sliceStartIndex,
            int totalCount,
            bool strictCheck = true
        )
        {
            _items = slice;

            var range = RelayPagination.CalculateEdgeRange(totalCount, first, after, last, before);

            StartIndex = sliceStartIndex;
            TotalCount = totalCount;
            StartOffset = Math.Max(range.StartOffset, StartIndex);
            EndOffset = Math.Max(StartOffset - 1, Math.Min(range.EndOffset, EndIndex));

            // Determine hasPrevious/hasNext according to specs
            // https://facebook.github.io/relay/graphql/connections.htm#sec-undefined.PageInfo.Fields
            // Because we work with offsets as cursors, we can use
            // a rather intuitive way to determine hasPrevious/hasNext.
            // The only special case we deal with is an empty edge list.
            // As an empty edge list does not contain any cursors,
            // pagination cannot continue from such a situation.
            HasPrevious = !IsEmpty && StartOffset > FirstValidOffset;
            HasNext = !IsEmpty && EndOffset < LastValidOffset;

            if (strictCheck)
            {
                if (!SliceCoversRange(StartIndex, EndIndex, range))
                {
                    throw new IncompleteSliceException(
                        $"Provided slice data with index range [{StartIndex},{EndIndex}] does not " +
                        $"completely contain the expected data range [{range.StartOffset}, {range.EndOffset}]", nameof(slice));
                }
            }
        }

        private static bool SliceCoversRange(int sliceStartIndex, int sliceEndIndex, EdgeRange range)
        {
            return sliceStartIndex <= range.StartOffset && sliceEndIndex >= range.EndOffset;
        }
    }

    [Obsolete("Use ArraySliceMetrics.Create instead")]
    public class ArraySliceMetrics<TSource, TParent> : ArraySliceMetrics<TSource>
    {
        [Obsolete("Use ArraySliceMetrics.Create instead")]
        public ArraySliceMetrics(
            IList<TSource> slice,
            ResolveConnectionContext<TParent> context,
            bool strictCheck = true
        ) : base(slice, context.First, context.After, context.Last, context.Before, 0, slice.Count, strictCheck)
        {
        }

        [Obsolete("Use ArraySliceMetrics.Create instead")]
        public ArraySliceMetrics(
            IList<TSource> slice,
            ResolveConnectionContext<TParent> context,
            int sliceStartIndex,
            int totalCount,
            bool strictCheck = true
        ) : base(slice, context.First, context.After, context.Last, context.Before, sliceStartIndex, totalCount,
            strictCheck)
        {
        }
    }
}
