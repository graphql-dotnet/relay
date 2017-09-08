using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL.Builders;
using GraphQL.Relay.Utilities;
using GraphQL.Types.Relay.DataObjects;
using Panic.StringUtils;

namespace GraphQL.Relay.Types
{
    public static class Connection
    {
        private const string Prefix = "arrayconnection";

        public static Connection<TSource> ToConnection<TSource, TParent>(
            IEnumerable<TSource> items,
            ResolveConnectionContext<TParent> context)
        {
            var list = items.ToList();
            return ToConnection(list, context, sliceStartIndex: 0, totalCount: list.Count);
        }

        public static Connection<TSource> ToConnection<TSource, TParent>(
            IEnumerable<TSource> slice,
            ResolveConnectionContext<TParent> context,
            int sliceStartIndex,
            int totalCount
        )
        {
            var sliceList = slice as IList<TSource> ?? slice.ToList();

            var sliceCount = sliceList.Count;
            var sliceEnd = sliceStartIndex + sliceCount;

            var beforeOffset = OffsetOrDefault(context.Before, totalCount);
            var afterOffset = OffsetOrDefault(context.After, defaultOffset: -1);

            var startOffset = new[] {sliceStartIndex - 1, afterOffset, -1}.Max() + 1;
            var endOffset = new[] {sliceEnd - 1, beforeOffset, totalCount}.Max();

            if (context.First.HasValue)
                endOffset = Math.Min(endOffset, startOffset + context.First.Value);

            if (context.Last.HasValue)
                startOffset = Math.Min(startOffset, endOffset - context.Last.Value);

            slice = sliceList.Slice(
                Math.Max(startOffset - sliceStartIndex, val2: 0),
                sliceCount - (sliceEnd - endOffset)
            );

            var edges = slice.Select((item, i) => new Edge<TSource>
                {
                    Node = item,
                    Cursor = OffsetToCursor(startOffset + i)
                })
                .ToList();

            var firstEdge = edges.FirstOrDefault();
            var lastEdge = edges.LastOrDefault();
            var lowerBound = context.After != null ? afterOffset + 1 : 0;
            var upperBound = context.Before != null ? beforeOffset : totalCount;

            return new Connection<TSource>
            {
                Edges = edges,
                TotalCount = totalCount,
                PageInfo = new PageInfo
                {
                    StartCursor = firstEdge?.Cursor,
                    EndCursor = lastEdge?.Cursor,
                    HasPreviousPage = context.Last.HasValue && startOffset > lowerBound,
                    HasNextPage = context.First.HasValue && endOffset < upperBound
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