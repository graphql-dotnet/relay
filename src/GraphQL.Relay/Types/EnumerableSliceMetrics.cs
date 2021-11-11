using System.Collections.Generic;
using System.Linq;
using GraphQL.Builders;

namespace GraphQL.Relay.Types
{
    /// <summary>
    /// Factory methods for <see cref="EnumerableSliceMetrics{TSource}"/>
    /// </summary>
    public static class EnumerableSliceMetrics
    {
        public static int DefaultPageSize { get; set; } = 25;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="items"></param>
        /// <param name="context"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public static EnumerableSliceMetrics<TSource> Create<TSource>(
            IEnumerable<TSource> items,
            IResolveConnectionContext context,
            int? totalCount = null
        ) => new(items, context, totalCount);
    }

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

        public int StartIndex { get; }

        public bool HasPrevious { get; }

        public bool HasNext { get; }

        public IEnumerable<TSource> Slice { get; }

        public EnumerableSliceMetrics(
            IEnumerable<TSource> itemSource,
            IResolveConnectionContext connectContext,
            int? totalCount = null
        )
        {
            TotalCount = totalCount ?? itemSource.Count();

            var (startingIndex, pageSize) = GetStartingIndexAndPageSize(TotalCount, connectContext);

            var slice = itemSource.Skip(startingIndex).Take(pageSize).ToList();

            Slice = slice;
            SliceSize = slice.Count;

            HasNext = (startingIndex + SliceSize) < TotalCount;
            HasPrevious = startingIndex > 0;

            StartIndex = startingIndex;
        }

        private static (int startingIndex, int pageSize) GetStartingIndexAndPageSize(int totalCount, IResolveConnectionContext context)
        {
            var (first, last) = GetFirstLast(context);
            var pageSize = last ?? first ?? context.PageSize ?? EnumerableSliceMetrics.DefaultPageSize;

            if (!string.IsNullOrEmpty(context.After))
            {
                var afterIndex = ConnectionUtils.CursorToOffset(context.After);

                if (last.HasValue)
                {
                    var startingIndex = totalCount - last.Value;

                    return (
                        startingIndex: startingIndex < afterIndex ? afterIndex : startingIndex,
                        pageSize
                    );
                }

                return (
                    startingIndex: afterIndex + 1,
                    pageSize
                );
            }

            if (!string.IsNullOrEmpty(context.Before))
            {
                var beforeIndex = ConnectionUtils.CursorToOffset(context.Before);
                var startingIndex = !last.HasValue || pageSize > beforeIndex ? 0 : beforeIndex - pageSize;
                var lastIndex = startingIndex + pageSize - 1;

                return (
                    startingIndex,
                    pageSize: lastIndex >= beforeIndex ? (pageSize - (lastIndex - beforeIndex) - 1) : pageSize);
            }

            return (
                startingIndex: !last.HasValue || totalCount < pageSize ? 0 : totalCount - pageSize,
                pageSize);
        }

        private static (int? first, int? last) GetFirstLast(
            IResolveConnectionContext context
        ) => (
            first: context.First,
            last: context.First.HasValue ? null : context.Last
        );
    }
}