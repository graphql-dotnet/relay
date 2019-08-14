using Xunit;
using static GraphQL.Relay.Types.ConnectionUtils;
using static GraphQL.Relay.Test.Types.ResolveConnectionContextFactory;

namespace GraphQL.Relay.Test.Types
{
    public class ConnectionUtilsTests
    {
        [Fact]
        public void ToConnection_WhenProvidingMatchingSlice_ReturnsConnectionForAllEdges()
        {
            var list = new[] {0, 1, 2, 3, 4};
            var con = ToConnection(list, CreateContext(5, OffsetToCursor(2)), 3, 10);
            Assert.Equal(10, con.TotalCount);
            Assert.Equal(OffsetToCursor(3), con.PageInfo.StartCursor);
            Assert.Equal(OffsetToCursor(7), con.PageInfo.EndCursor);
            Assert.True(con.PageInfo.HasNextPage);
            Assert.True(con.PageInfo.HasPreviousPage);
            Assert.Equal(5, con.Edges.Count);
        }

        [Fact]
        public void ToConnection_WhenProvidingOverProvisionedSlice_ReturnsRequestedSliceOnly()
        {
            var list = new[] {3, 4, 5, 6, 7, 8, 9};
            var con = ToConnection(list, CreateContext(4, OffsetToCursor(4)), 3, 15);
            Assert.Equal(15, con.TotalCount);
            Assert.Equal(OffsetToCursor(5), con.PageInfo.StartCursor);
            Assert.Equal(OffsetToCursor(8), con.PageInfo.EndCursor);
            Assert.True(con.PageInfo.HasNextPage);
            Assert.True(con.PageInfo.HasPreviousPage);
            Assert.Equal(4, con.Edges.Count);
            Assert.Equal(new[] {5, 6, 7, 8}, con.Items);
        }

        [Fact]
        public void ToConnection_WhenProvidingWholeDataset_ReturnsConnectionForAllEdges()
        {
            var list = new[] {0, 1, 2, 3, 4};
            var con = ToConnection(list, CreateContext());
            Assert.Equal(5, con.TotalCount);
            Assert.Equal(OffsetToCursor(0), con.PageInfo.StartCursor);
            Assert.Equal(OffsetToCursor(4), con.PageInfo.EndCursor);
            Assert.False(con.PageInfo.HasNextPage);
            Assert.False(con.PageInfo.HasPreviousPage);
            Assert.Equal(5, con.Edges.Count);
        }

        [Fact]
        public void ToConnection_WhenRequestingAllUpToBeforeCursor_ReturnsRequestedSliceOnly()
        {
            var list = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            var con = ToConnection(list, CreateContext(before: OffsetToCursor(7)), 0, 14);
            Assert.Equal(14, con.TotalCount);
            Assert.Equal(OffsetToCursor(0), con.PageInfo.StartCursor);
            Assert.Equal(OffsetToCursor(6), con.PageInfo.EndCursor);
            Assert.True(con.PageInfo.HasNextPage);
            Assert.False(con.PageInfo.HasPreviousPage);
            Assert.Equal(7, con.Edges.Count);
            Assert.Equal(new[] {0, 1, 2, 3, 4, 5, 6}, con.Items);
        }

        [Fact]
        public void ToConnection_WhenRequestingBackwardSlice_ReturnsRequestedSliceOnly()
        {
            var list = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
            var con = ToConnection(list, CreateContext(last: 3, before: OffsetToCursor(7)), 0, 14);
            Assert.Equal(14, con.TotalCount);
            Assert.Equal(OffsetToCursor(4), con.PageInfo.StartCursor);
            Assert.Equal(OffsetToCursor(6), con.PageInfo.EndCursor);
            Assert.True(con.PageInfo.HasNextPage);
            Assert.True(con.PageInfo.HasPreviousPage);
            Assert.Equal(3, con.Edges.Count);
            Assert.Equal(new[] {4, 5, 6}, con.Items);
        }

        [Fact]
        public void ToConnection_WhenRequestingEndSlice_ReturnsRequestedSliceOnly()
        {
            var list = new[] {3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13};
            var con = ToConnection(list, CreateContext(last: 4), 3, 14);
            Assert.Equal(14, con.TotalCount);
            Assert.Equal(OffsetToCursor(10), con.PageInfo.StartCursor);
            Assert.Equal(OffsetToCursor(13), con.PageInfo.EndCursor);
            Assert.False(con.PageInfo.HasNextPage);
            Assert.True(con.PageInfo.HasPreviousPage);
            Assert.Equal(4, con.Edges.Count);
            Assert.Equal(new[] {10, 11, 12, 13}, con.Items);
        }


        [Fact]
        public void ToConnection_WhenRequestingForwardAndBackwardSlice_ReturnsRequestedSliceOnly()
        {
            // NB: this usage is discouraged in the spec, but supported by this library
            // See https://facebook.github.io/relay/graphql/connections.htm#sec-Forward-pagination-arguments
            var list = new[] {4, 5, 6, 7, 8, 9, 10, 11, 12, 13};
            var con = ToConnection(list,
                CreateContext(6, OffsetToCursor(5), 3, OffsetToCursor(12)), 4, 20);
            Assert.Equal(20, con.TotalCount);
            Assert.Equal(OffsetToCursor(9), con.PageInfo.StartCursor);
            Assert.Equal(OffsetToCursor(11), con.PageInfo.EndCursor);
            Assert.True(con.PageInfo.HasNextPage);
            Assert.True(con.PageInfo.HasPreviousPage);
            Assert.Equal(3, con.Edges.Count);
            Assert.Equal(new[] {9, 10, 11}, con.Items);
        }

        [Fact]
        public void ToConnection_WhenRequestingForwardAndBackwardSliceWthLargeCounts_ReturnsRequestedSliceOnly()
        {
            // NB: this usage is discouraged in the spec, but supported by this library
            // See https://facebook.github.io/relay/graphql/connections.htm#sec-Forward-pagination-arguments
            var list = new[] {4, 5, 6, 7, 8, 9, 10, 11, 12, 13};
            var con = ToConnection(list,
                CreateContext(100, OffsetToCursor(5), 100, OffsetToCursor(12)), 4, 20);
            Assert.Equal(20, con.TotalCount);
            Assert.Equal(OffsetToCursor(6), con.PageInfo.StartCursor);
            Assert.Equal(OffsetToCursor(11), con.PageInfo.EndCursor);
            Assert.True(con.PageInfo.HasNextPage);
            Assert.True(con.PageInfo.HasPreviousPage);
            Assert.Equal(6, con.Edges.Count);
            Assert.Equal(new[] {6, 7, 8, 9, 10, 11}, con.Items);
        }

        [Fact]
        public void ToConnection_WhenRequestingForwardSlice_ReturnsRequestedSliceOnly()
        {
            var list = new[] {4, 5, 6, 7, 8, 9, 10};
            var con = ToConnection(list, CreateContext(2, OffsetToCursor(5)), 4, 11);
            Assert.Equal(11, con.TotalCount);
            Assert.Equal(OffsetToCursor(6), con.PageInfo.StartCursor);
            Assert.Equal(OffsetToCursor(7), con.PageInfo.EndCursor);
            Assert.True(con.PageInfo.HasNextPage);
            Assert.True(con.PageInfo.HasPreviousPage);
            Assert.Equal(2, con.Edges.Count);
            Assert.Equal(new[] {6, 7}, con.Items);
        }

        [Fact]
        public void ToConnection_WhenRequestingFromAfterCursor_ReturnsRequestedSliceOnly()
        {
            var list = new[] {4, 5, 6, 7, 8, 9, 10};
            var con = ToConnection(list, CreateContext(after: OffsetToCursor(5)), 4, 11);
            Assert.Equal(11, con.TotalCount);
            Assert.Equal(OffsetToCursor(6), con.PageInfo.StartCursor);
            Assert.Equal(OffsetToCursor(10), con.PageInfo.EndCursor);
            Assert.False(con.PageInfo.HasNextPage);
            Assert.True(con.PageInfo.HasPreviousPage);
            Assert.Equal(5, con.Edges.Count);
            Assert.Equal(new[] {6, 7, 8, 9, 10}, con.Items);
        }

        [Fact]
        public void ToConnection_WhenRequestingStartSlice_ReturnsRequestedSliceOnly()
        {
            var list = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
            var con = ToConnection(list, CreateContext(4), 0, 14);
            Assert.Equal(14, con.TotalCount);
            Assert.Equal(OffsetToCursor(0), con.PageInfo.StartCursor);
            Assert.Equal(OffsetToCursor(3), con.PageInfo.EndCursor);
            Assert.True(con.PageInfo.HasNextPage);
            Assert.False(con.PageInfo.HasPreviousPage);
            Assert.Equal(4, con.Edges.Count);
            Assert.Equal(new[] {0, 1, 2, 3}, con.Items);
        }

        [Fact]
        public void ToConnection_WhenBeforeCursorIsAheadOfAfterCursor_ReturnsEmptyList()
        {
            var list = new[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};

            // this should specifically NOT throw, even though the input is somewhat nonsensical
            var con = ToConnection(list, CreateContext(first: 4, after: OffsetToCursor(8), before: OffsetToCursor(2)),
                0, 14);
            Assert.Equal(14, con.TotalCount);
            Assert.Empty(con.Edges);
            Assert.Null(con.PageInfo.StartCursor);
            Assert.Null(con.PageInfo.EndCursor);
            Assert.False(con.PageInfo.HasNextPage);
            Assert.False(con.PageInfo.HasPreviousPage);
        }
    }
}
