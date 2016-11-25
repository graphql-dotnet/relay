using GraphQL.Builders;
using GraphQL.Relay.Utilities;
using GraphQL.Types.Relay.DataObjects;
using Panic.StringUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Relay.Types
{
    public static class Connection
    {   
        private static string PREFIX = "arrayconnection";

        public static Connection<TSource> ToConnection<TSource, TParent>(
            IEnumerable<TSource> items,
            ResolveConnectionContext<TParent> context)
        {
            return ToConnection(items, context, 0, items.Count());
        }

        public static Connection<TSource> ToConnection<TSource, TParent>(
            IEnumerable<TSource> slice,
            ResolveConnectionContext<TParent> context,
            int sliceStartIndex,
            int totalCount
        ) {
            int sliceCount = slice.Count();
            int sliceEnd = sliceStartIndex + sliceCount;

            int beforeOffset = OffsetOrDefault(context.Before, totalCount);
            int afterOffset = OffsetOrDefault(context.After, -1);

            int startOffset = new[] { sliceStartIndex - 1, afterOffset, -1 }.Max() + 1;
            int endOffset = new[] { sliceEnd - 1, beforeOffset, totalCount }.Max();

            if (context.First.HasValue)
            {
                endOffset = Math.Min(endOffset, startOffset + context.First.Value);
            }

            if (context.Last.HasValue)
            {
                startOffset = Math.Min(startOffset, endOffset - context.Last.Value);
            }

            slice = slice.Slice(
                Math.Max(startOffset - sliceStartIndex, 0),
                sliceCount - (sliceEnd - endOffset)
            );

            var edges = slice.Select((item, i) => new Edge<TSource>
            {
                Node = item,
                Cursor = OffsetToCursor(startOffset + i),
            })
            .ToList();

            var firstEdge = edges.FirstOrDefault();
            var lastEdge = edges.LastOrDefault();
            var lowerBound = context.After != null ? (afterOffset + 1) : 0;
            var upperBound = context.Before != null ? beforeOffset : totalCount;

            return new Connection<TSource>()
            {
                Edges = edges,
                TotalCount = totalCount,
                PageInfo = new PageInfo
                {
                    StartCursor = firstEdge?.Cursor,
                    EndCursor = lastEdge?.Cursor,
                    HasPreviousPage = context.Last.HasValue ? startOffset > lowerBound : false,
                    HasNextPage = context.First.HasValue ? endOffset > upperBound : false,
                }
            };
        }

        public static int CursorToOffset(string cursor)
        {
            return int.Parse(
                StringUtils.Base64Decode(cursor).Substring(PREFIX.Length + 1)
            );
        }

        public static string OffsetToCursor(int offset) {
            return StringUtils.Base64Encode($"{PREFIX}:{offset}");
        }

        public static int OffsetOrDefault(string cursor, int defaultOffset)
        {
            if (cursor == null)
            {
                return defaultOffset;
            }

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
