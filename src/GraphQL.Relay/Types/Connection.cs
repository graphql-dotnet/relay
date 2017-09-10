using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Builders;
using GraphQL.Relay.Utilities;
using GraphQL.Types.Relay.DataObjects;
using Panic.StringUtils;

namespace GraphQL.Relay.Types
{
    public class ArraySliceMetrics<TSource, TParent> {
        private IList<TSource> _items;

        /// <summary>
        /// The Total number of items in outer list. May be >= the SliceSize
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// The local total of the list slice.
        /// </summary>
        public int SliceSize { get; set; }
        /// <summary>
        /// The start index of the slice within the larger List
        /// </summary>
        /// <returns></returns>
        public int StartIndex { get; set; } = 0;
        /// <summary>
        /// The end index of the slice within the larger List
        /// </summary>
        public int EndIndex => StartIndex + SliceSize;

        public int StartOffset { get; set; }
        public int EndOffset { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }

        public ArraySliceMetrics(
            IList<TSource> slice,
            ResolveConnectionContext<TParent> context
        ): this(slice, context, 0, slice.Count) {}

        public IEnumerable<TSource> Slice => _items.Slice(
            Math.Max(StartOffset - StartIndex, 0),
            SliceSize - (EndIndex - EndOffset)
        );

        public ArraySliceMetrics(
            IList<TSource> slice,
            ResolveConnectionContext<TParent> context,
            int sliceStartIndex,
            int totalCount
        ) {
            _items = slice;

            SliceSize = slice.Count;
            StartIndex = sliceStartIndex;

            var beforeOffset = Connection.OffsetOrDefault(context.Before, totalCount);
            var afterOffset = Connection.OffsetOrDefault(context.After, defaultOffset: -1);

            StartOffset = new[] {sliceStartIndex - 1, afterOffset, -1}.Max() + 1;
            EndOffset = new[] {EndIndex - 1, beforeOffset, totalCount}.Max();

            if (context.First.HasValue)
                EndOffset = Math.Min(EndOffset, StartOffset + context.First.Value);

            if (context.Last.HasValue)
                StartOffset = Math.Min(StartOffset, EndOffset - context.Last.Value);

            var lowerBound = context.After != null ? afterOffset + 1 : 0;
            var upperBound = context.Before != null ? beforeOffset : totalCount;

            HasPrevious = context.Last.HasValue && StartOffset > lowerBound;
            HasNext = context.First.HasValue && EndOffset < upperBound;
        }
    }
    public static class Connection
    {
        private const string Prefix = "arrayconnection";

        public static Connection<TSource> ToConnection<TSource, TParent>(
            IEnumerable<TSource> items,
            ResolveConnectionContext<TParent> context
        ) {
            var list = items.ToList();
            return ToConnection(list, context, sliceStartIndex: 0, totalCount: list.Count);
        }

        public static Connection<TSource> ToConnection<TSource, TParent>(
            IEnumerable<TSource> slice,
            ResolveConnectionContext<TParent> context,
            int sliceStartIndex,
            int totalCount
        ) {
            var sliceList = slice as IList<TSource> ?? slice.ToList();

            var metrics = new ArraySliceMetrics<TSource, TParent>(
                sliceList,
                context,
                sliceStartIndex,
                totalCount
            );

            var edges = metrics.Slice.Select((item, i) => new Edge<TSource> {
                    Node = item,
                    Cursor = OffsetToCursor(metrics.StartOffset + i)
                })
                .ToList();

            var firstEdge = edges.FirstOrDefault();
            var lastEdge = edges.LastOrDefault();

            return new Connection<TSource>
            {
                Edges = edges,
                TotalCount = totalCount,
                PageInfo = new PageInfo {
                    StartCursor = firstEdge?.Cursor,
                    EndCursor = lastEdge?.Cursor,
                    HasPreviousPage = metrics.HasPrevious,
                    HasNextPage = metrics.HasNext,
                }
            };
        }

        public static int CursorToOffset(string cursor)
        {
            return int.Parse(
                StringUtils.Base64Decode(cursor).Substring(Prefix.Length + 1)
            );
        }

        public static string OffsetToCursor(int offset)
        {
            return StringUtils.Base64Encode($"{Prefix}:{offset}");
        }

        public static int OffsetOrDefault(string cursor, int defaultOffset)
        {
            if (cursor == null)
                return defaultOffset;

            try
            {
                return CursorToOffset(cursor);
            }
            catch
            {
                return defaultOffset;
            }
        }
    }
}