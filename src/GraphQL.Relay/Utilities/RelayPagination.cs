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

        private static EdgeRange ApplyCursorToEdges(int edgeCount, string after, string before)
        {
            const int outOfRange = -2;

            // only use "after" cursor if it represents an edge in [0, edgeCount-1]
            var afterOffset = CheckRange(OffsetOrDefault(after, outOfRange), edgeCount, -1);

            // only use "before" cursor if it represents an edge in [0, edgeCount-1]
            var beforeOffset = CheckRange(OffsetOrDefault(before, outOfRange), edgeCount, edgeCount);

            int startOffset = afterOffset + 1;
            int endOffset = beforeOffset - 1;

            return new EdgeRange(startOffset, endOffset);
        }

        private static int CheckRange(int value, int edgeCount, int defaultIfOutOfRange)
        {
            return value < 0 || value >= edgeCount ? defaultIfOutOfRange : value;
        }
    }
}
