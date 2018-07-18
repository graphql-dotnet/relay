using System;
using GraphQL.Relay.Utilities;
using Xunit;
using static GraphQL.Relay.Types.ConnectionUtils;

namespace GraphQL.Relay.Test.Utilities
{
    public class RelayPaginationTests
    {
        [Fact]
        public void CalculateEdgeRange_IfNoPaginationArgumentsAreProvided_ReturnsFullRange()
        {
            var range = RelayPagination.CalculateEdgeRange(10);
            Assert.Equal(0, range.StartOffset);
            Assert.Equal(9, range.EndOffset);
        }

        [Fact]
        public void CalculateEdgeRange_IfAfterIsProvided_ReturnsAllEdgesAfterArgument()
        {
            var range = RelayPagination.CalculateEdgeRange(10, after: OffsetToCursor(5));
            Assert.Equal(6, range.StartOffset);
            Assert.Equal(9, range.EndOffset);
        }

        [Fact]
        public void CalculateEdgeRange_IfBeforeIsProvided_ReturnsAllEdgesBeforeArgument()
        {
            var range = RelayPagination.CalculateEdgeRange(10, before: OffsetToCursor(5));
            Assert.Equal(0, range.StartOffset);
            Assert.Equal(4, range.EndOffset);
        }

        [Theory]
        [InlineData(20, -3)]
        [InlineData(20, -2)]
        [InlineData(20, -1)]
        [InlineData(20, 20)]
        [InlineData(20, 21)]
        [InlineData(20, 22)]
        public void CalculateEdgeRange_IfAfterIsOutOfRange_SilentlyUses0AsBoundary(int edgeCount, int afterOffset)
        {
            var range = RelayPagination.CalculateEdgeRange(edgeCount, after: OffsetToCursor(afterOffset));
            Assert.Equal(0, range.StartOffset);
        }

        [Theory]
        [InlineData(20, -3)]
        [InlineData(20, -2)]
        [InlineData(20, -1)]
        [InlineData(20, 20)]
        [InlineData(20, 21)]
        [InlineData(20, 22)]
        public void CalculateEdgeRange_IfBeforeIsOutOfRange_SilentlyUsesTotalCountAsBoundary(int edgeCount,
            int afterOffset)
        {
            var range = RelayPagination.CalculateEdgeRange(edgeCount, before: OffsetToCursor(afterOffset));
            Assert.Equal(edgeCount - 1, range.EndOffset);
        }

        [Fact]
        public void CalculateEdgeRange_IfCountIsEmpty_ReturnsEmptyRange()
        {
            var range = RelayPagination.CalculateEdgeRange(0);
            Assert.Equal(-1, range.EndOffset);
        }

        [Fact]
        public void CalculateEdgeRange_IfAfterIsAtBeginning_ReturnsAllButTheFirstEdge()
        {
            var range = RelayPagination.CalculateEdgeRange(10, after: OffsetToCursor(0));
            Assert.Equal(1, range.StartOffset);
            Assert.Equal(9, range.EndOffset);
            Assert.Equal(9, range.Count);
        }

        [Fact]
        public void CalculateEdgeRange_IfSelectedRangeIsEmpty_ReturnsEmptyRange()
        {
            var range = RelayPagination.CalculateEdgeRange(10, after: OffsetToCursor(4), before: OffsetToCursor(5));
            Assert.Equal(5, range.StartOffset);
            Assert.Equal(4, range.EndOffset);
            Assert.Equal(0, range.Count);
            Assert.True(range.IsEmpty);
        }

        [Fact]
        public void CalculateEdgeRange_IfSelectedRangeHasOneElement_ReturnsRangeOfSingleEdge()
        {
            var range = RelayPagination.CalculateEdgeRange(10, after: OffsetToCursor(4), before: OffsetToCursor(6));
            Assert.Equal(5, range.StartOffset);
            Assert.Equal(5, range.EndOffset);
            Assert.Equal(1, range.Count);
            Assert.False(range.IsEmpty);
        }

        [Fact]
        public void CalculateEdgeRange_IfAfterIsAtTheEnd_ReturnsEmptyResult()
        {
            var range = RelayPagination.CalculateEdgeRange(10, after: OffsetToCursor(9));
            Assert.Equal(10, range.StartOffset);
            Assert.Equal(9, range.EndOffset);
            Assert.Equal(0, range.Count);
            Assert.True(range.IsEmpty);
        }

        [Fact]
        public void CalculateEdgeRange_IfBeforeIsAtBeginning_ReturnsEmptyRange()
        {
            var range = RelayPagination.CalculateEdgeRange(10, before: OffsetToCursor(0));
            Assert.Equal(0, range.StartOffset);
            Assert.Equal(-1, range.EndOffset);
            Assert.Equal(0, range.Count);
            Assert.True(range.IsEmpty);
        }

        [Fact]
        public void CalculateEdgeRange_IfBeforeIsAtTheEnd_ReturnsAllButTheLastEdge()
        {
            var range = RelayPagination.CalculateEdgeRange(10, before: OffsetToCursor(9));
            Assert.Equal(0, range.StartOffset);
            Assert.Equal(8, range.EndOffset);
            Assert.Equal(9, range.Count);
        }

        [Fact]
        public void CalculateEdgeRange_IfFirstIsNegative_ThrowsException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() =>
                RelayPagination.CalculateEdgeRange(10, first: -1));
            Assert.Equal("first", ex.ParamName);
        }

        [Fact]
        public void CalculateEdgeRange_IfLastIsNegative_ThrowsException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => RelayPagination.CalculateEdgeRange(10, last: -1));
            Assert.Equal("last", ex.ParamName);
        }


        [Fact]
        public void CalculateEdgeRange_IfFirstIsProvided_LimitsRangeLength()
        {
            var range = RelayPagination.CalculateEdgeRange(20, first: 10);
            Assert.Equal(10, range.Count);
            Assert.Equal(0, range.StartOffset);
            Assert.Equal(9, range.EndOffset);
        }

        [Fact]
        public void CalculateEdgeRange_IfLastIsProvided_LimitsRangeLength()
        {
            var range = RelayPagination.CalculateEdgeRange(20, last: 10);
            Assert.Equal(10, range.Count);
            Assert.Equal(10, range.StartOffset);
            Assert.Equal(19, range.EndOffset);
        }

        [Fact]
        public void CalculateEdgeRange_IfFirstAndLastAreProvide_AppliesFirstThenLast()
        {
            var range = RelayPagination.CalculateEdgeRange(20, first: 10, last: 5);
            Assert.Equal(5, range.Count);
            Assert.Equal(5, range.StartOffset);
            Assert.Equal(9, range.EndOffset);
        }

        [Theory]
        [InlineData(15, null, null, null, null, 0, 14)]
        [InlineData(15, 0, null, null, null, 1, 14)]
        [InlineData(15, 5, null, null, null, 6, 14)]
        [InlineData(15, null, 10, null, null, 0, 9)]
        [InlineData(15, null, 14, null, null, 0, 13)]
        [InlineData(100, 20, 80, 30, null, 21, 50)]
        [InlineData(100, 20, 80, 30, 10, 41, 50)]
        public void CalculateEdgeRange_IfAllParametersAreProvided_CalculatesCorrectPagination(
            int count,
            int? afterOffset,
            int? beforeOffset,
            int? first,
            int? last,
            int expectedStart,
            int expectedEnd)
        {
            var range = RelayPagination.CalculateEdgeRange(count,
                first: first,
                last: last,
                after: afterOffset == null ? null : OffsetToCursor(afterOffset.Value),
                before: beforeOffset == null ? null : OffsetToCursor(beforeOffset.Value));
            Assert.Equal(expectedStart, range.StartOffset);
            Assert.Equal(expectedEnd, range.EndOffset);
        }
    }
}
