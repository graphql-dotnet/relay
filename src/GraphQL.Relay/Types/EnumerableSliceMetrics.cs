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
        private int SliceSize { get; }

        public int StartOffset { get; }

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

            StartOffset = startingIndex;
        }

        private static (int startingIndex, int pageSize) GetStartingIndexAndPageSize(int totalCount, IResolveConnectionContext context)
        {
            var (first, last) = GetFirstLast(context);
            var pageSize = last ?? first ?? context.PageSize ?? EnumerableSliceMetrics.DefaultPageSize;

            if (!string.IsNullOrEmpty(context.After))
            {
                var after = ConnectionUtils.CursorToOffset(context.After);

                if (last.HasValue)
                {
                    var startingIndex = totalCount - last.Value;

                    return (
                        startingIndex: startingIndex < after ? after : startingIndex,
                        pageSize);
                }

                return (
                    startingIndex: after + 1,
                    pageSize
                );
            }

            if (!string.IsNullOrEmpty(context.Before))
            {
                var before = ConnectionUtils.CursorToOffset(context.Before);

                if (first.HasValue)
                {
                    var lastIndex = pageSize - 1;

                    return (
                        startingIndex: 0,
                        pageSize: lastIndex > before ? before - 1 : pageSize);
                }

                if (last.HasValue)
                {
                    var startingIndex = before - pageSize - 1;
                    var lastIndex = startingIndex + pageSize - 1;

                    return (
                        startingIndex: startingIndex < 0 ? 0 : startingIndex,
                        pageSize: lastIndex > before ? lastIndex - startingIndex : pageSize);
                }
            }

            return (
                startingIndex: 0,
                pageSize);
        }

        private static (int? first, int? last) GetFirstLast(IResolveConnectionContext context)
        {
            if (context.Last == null)
            {
                return (
                    first: context.First,
                    last: context.First.HasValue ? null : context.Last);
            }

            return (
                first: context.First.HasValue ? context.First : null,
                last: context.First.HasValue ? null : context.Last);
        }
    }
}