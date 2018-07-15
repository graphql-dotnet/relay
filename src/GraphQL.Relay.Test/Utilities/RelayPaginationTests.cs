using System;
using GraphQL.Relay.Types;
using GraphQL.Relay.Utilities;
using Xunit;

namespace GraphQL.Relay.Test.Utilities
{
    public class RelayPaginationTests
    {
        [Fact]
        public void Apply_IfNoPaginationArgumentsAreProvided_ReturnsFullRange()
        {
            var range = RelayPagination.CalculateEdgeRange(10);
            Assert.Equal(0, range.StartOffset);
            Assert.Equal(9, range.EndOffset);
        }


        [Fact]
        public void Apply_IfAfterIsProvided_ReturnsAllEdgesAfterArgument()
        {
            var range = RelayPagination.CalculateEdgeRange(10, after: ConnectionUtils.OffsetToCursor(5));
            Assert.Equal(6, range.StartOffset);
            Assert.Equal(9, range.EndOffset);
        }


        [Fact]
        public void Apply_IfBeforeIsProvided_ReturnsAllEdgesBeforeArgument()
        {
            var range = RelayPagination.CalculateEdgeRange(10, before: ConnectionUtils.OffsetToCursor(5));
            Assert.Equal(0, range.StartOffset);
            Assert.Equal(4, range.EndOffset);
        }

        [Theory]
        [InlineData(20, -3)]
        [InlineData(20, -2)]
        [InlineData(20, 21)]
        [InlineData(20, 22)]
        public void Apply_IfAfterIsOutOfRange_SilentlyUses0AsBoundary(int edgeCount, int afterOffset)
        {
            var range = RelayPagination.CalculateEdgeRange(edgeCount, after: ConnectionUtils.OffsetToCursor(afterOffset));
            Assert.Equal(0, range.StartOffset);
        }

        [Theory]
        [InlineData(20, -3)]
        [InlineData(20, -2)]
        [InlineData(20, 21)]
        [InlineData(20, 22)]
        public void Apply_IfBeforeIsOutOfRange_SilentlyUsesTotalCountAsBoundary(int edgeCount, int afterOffset)
        {
            var range = RelayPagination.CalculateEdgeRange(edgeCount, before: ConnectionUtils.OffsetToCursor(afterOffset));
            Assert.Equal(edgeCount - 1, range.EndOffset);
        }

        [Fact]
        public void Apply_IfFirstIsNegative_ThrowsException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => RelayPagination.CalculateEdgeRange(10, first: -1));
            Assert.Equal("first", ex.ParamName);
        }

        [Fact]
        public void Apply_IfLastIsNegative_ThrowsException()
        {
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => RelayPagination.CalculateEdgeRange(10, last: -1));
            Assert.Equal("last", ex.ParamName);
        }


        [Fact]
        public void Apply_IfFirstIsProvided_LimitsRangeLength()
        {
            var range = RelayPagination.CalculateEdgeRange(20, first: 10);
            Assert.Equal(10, range.Count);
            Assert.Equal(0, range.StartOffset);
            Assert.Equal(9, range.EndOffset);
        }


        [Fact]
        public void Apply_IfLastIsProvided_LimitsRangeLength()
        {
            var range = RelayPagination.CalculateEdgeRange(20, last: 10);
            Assert.Equal(10, range.Count);
            Assert.Equal(10, range.StartOffset);
            Assert.Equal(19, range.EndOffset);
        }

        [Fact]
        public void Apply_IfFirstAndLastAreProvide_AppliesFirstThenLast()
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
        public void Apply_IfAllParametersAreProvided_CalculatesCorrectPagination(
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
                after: afterOffset == null ? null : ConnectionUtils.OffsetToCursor(afterOffset.Value),
                before: beforeOffset == null ? null : ConnectionUtils.OffsetToCursor(beforeOffset.Value));
            Assert.Equal(expectedStart, range.StartOffset);
            Assert.Equal(expectedEnd, range.EndOffset);
        }
    }
}