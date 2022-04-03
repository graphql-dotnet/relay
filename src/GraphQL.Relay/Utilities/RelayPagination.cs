using System;
using GraphQL.Relay.Types;
using static GraphQL.Relay.Types.ConnectionUtils;

namespace GraphQL.Relay.Utilities
{
    public static class RelayPagination
    {
        /// <summary>
        /// Apply Facebook Relay pagination as described at <a href="https://facebook.github.io/relay/graphql/connections.htm#sec-Pagination-algorithm">
        /// https://facebook.github.io/relay/graphql/connections.htm#sec-Pagination-algorithm</a> 
        /// </summary>
        /// <param name="edgeCount">Total number of edges.</param>
        /// <param name="first">Maximum number of edges to return after <paramref name="after"/> (if provided) or from
        /// the start of the edge list. 
        /// </param> 
        /// <param name="after">Only return edges coming after the edge represented by this cursor.</param>
        /// <param name="last">Maximum number of edges to return before <paramref name="before"/> (if provided)
        /// or from the end of the edge list.</param>
        /// <param name="before">Only return edges coming before the edge represented by this cursor.</param>
        /// <returns>An <see cref="EdgeRange"/> that defines the range of edges to return.</returns>
        public static EdgeRange CalculateEdgeRange(int edgeCount, int? first = null, string after = null, int? last = null,
            string before = null)
        {
            var range = ApplyCursorToEdges(edgeCount, after, before);
            if (first != null)
            {
                if (first.Value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(first));
                }
                range.LimitCountFromStart(first.Value);
            }
            
            if (last != null)
            {
                if (last.Value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(last));
                }
                
                range.LimitCountToEnd(last.Value);
            }
            return range;
        }

        /// <summary>
        /// Ensures that the <see cref="EdgeRange"/> is within the bounds of starting and ending index of the set.
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="edges"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <exception cref="IncompleteSliceException"></exception>
        public static void EnsureSliceCoversRange(string paramName, EdgeRange edges, int startIndex, int endIndex)
        {
            if (SliceCoversRange(startIndex, endIndex, edges))
            {
                return;
            }

            throw new IncompleteSliceException(
                $"Provided slice data with index range [{startIndex},{endIndex}] does not " +
                $"completely contain the expected data range [{edges.StartOffset}, {edges.EndOffset}]", paramName);
        }

        private static bool SliceCoversRange(int sliceStartIndex, int sliceEndIndex, EdgeRange range)
        {
            return sliceStartIndex <= range.StartOffset && sliceEndIndex >= range.EndOffset;
        }

        private static EdgeRange ApplyCursorToEdges(int edgeCount, string after, string before)
        {
            const int outOfRange = -2;
            // only use "after" cursor if it represents an edge in [0, edgeCount-1]
            var afterOffset = CheckRange(OffsetOrDefault(after, outOfRange), edgeCount, -1);
            // only use "before" cursor if it represents an edge in [0, edgeCount-1]
            var beforeOffset = CheckRange(OffsetOrDefault(before, outOfRange), edgeCount, edgeCount);

            int startOffset = afterOffset + 1;
            int endOffset = beforeOffset - 1;


            var edges = new EdgeRange(startOffset, endOffset);
            return edges;
        }

        private static int CheckRange(int value, int edgeCount, int defaultIfOutOfRange)
        {
            return value < 0 || value >= edgeCount ? defaultIfOutOfRange : value;
        }
    }
}
