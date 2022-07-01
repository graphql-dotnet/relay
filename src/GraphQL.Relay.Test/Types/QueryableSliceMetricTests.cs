using GraphQL.Builders;
using GraphQL.Relay.Test.Fixtures;
using GraphQL.Relay.Test.Fixtures.Models;
using GraphQL.Relay.Types;
using static GraphQL.Relay.Test.Fixtures.DatabaseFixture;
using static GraphQL.Relay.Test.Types.ResolveConnectionContextFactory;

namespace GraphQL.Relay.Test.Types
{
    public class QueryableSliceMetricTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture fixture;

        public QueryableSliceMetricTests(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }

        private BloggingContext DbContext => fixture.Context;

        public static ResolveConnectionContext<Blog> CreateContext(
            int? first = null,
            string after = null,
            int? last = null,
            string before = null,
            int? defaultPageSize = 25
        ) => CreateContext<Blog>(first, after, last, before, defaultPageSize);

        [Fact]
        public void Create_WhenCreateFromFullList_ReturnsListCount()
        {
            // Arrange
            var context = CreateContext<Blog>();

            // Act
            var slice = SliceMetrics.Create(
                DbContext.Blogs,
                context
            );

            // Assert
            Assert.Equal(fixture.TotalCount, slice.SliceSize);
            Assert.Equal(fixture.TotalCount, slice.TotalCount);
            Assert.Equal(0, slice.StartIndex);
            Assert.False(slice.HasNext);
            Assert.False(slice.HasPrevious);
        }

        [Fact]
        public void Create_WhenCreateFromFullListAndContext_ReturnsListCount()
        {
            // Arrange
            var context = CreateContext(
                defaultPageSize: fixture.TotalCount
            );

            // Act
            var slice = SliceMetrics.Create(
                DbContext.Blogs,
                context
            );

            // Assert
            Assert.Equal(fixture.TotalCount, slice.SliceSize);
            Assert.Equal(fixture.TotalCount, slice.TotalCount);
            Assert.Equal(0, slice.StartIndex);
            Assert.False(slice.HasNext);
            Assert.False(slice.HasPrevious);
        }

        [Fact]
        public void Create_WhenDataDoesNotCoverRequestedRange_AdjustsStartAndEndOffsets()
        {
            // Arrange
            var context = CreateContext(
                first: 10,
                after: ConnectionUtils.OffsetToCursor(995)
            );

            // Act
            var slice = SliceMetrics.Create(
                DbContext.Blogs,
                context
            );

            // Assert
            Assert.Equal(4, slice.SliceSize);
            Assert.Equal(fixture.TotalCount, slice.TotalCount);
            Assert.Equal(996, slice.StartIndex);
            Assert.False(slice.HasNext);
            Assert.True(slice.HasPrevious);
        }

        [Fact]
        public void Create_WhenDataSliceStartsAfterEnd_ReturnsEmptyResult()
        {
            // Arrange
            var context = CreateContext(
                first: 10,
                after: ConnectionUtils.OffsetToCursor(999)
            );

            // Act
            var slice = SliceMetrics.Create(
                DbContext.Blogs,
                context
            );

            // Assert
            Assert.Equal(0, slice.SliceSize);
            Assert.Equal(1000, slice.StartIndex);

            Assert.False(slice.HasNext);
            Assert.True(slice.HasPrevious);
        }

        [Fact]
        public void HasNext_WhenDataIsSliceAndEndOffsetIsLastIndex_ReturnsFalse()
        {
            // Arrange
            var context = CreateContext(
                after: ConnectionUtils.OffsetToCursor(999)
            );

            // Act
            var slice = SliceMetrics.Create(DbContext.Blogs, context);

            // Assert
            Assert.Equal(1000, slice.StartIndex);
            Assert.Equal(0, slice.SliceSize);
            Assert.False(slice.HasNext);
        }

        [Fact]
        public void HasNext_WhenDataIsSliceAndEndOffsetIsLessThanLastIndex_ReturnsTrue()
        {
            // Arrange
            var context = CreateContext(
                first: 10,
                after: ConnectionUtils.OffsetToCursor(988)
            );

            // Act
            var slice = SliceMetrics.Create(DbContext.Blogs, context);

            // Assert
            Assert.Equal(989, slice.StartIndex);
            Assert.Equal(10, slice.SliceSize);
            Assert.True(slice.HasNext);
        }

        [Fact]
        public void HasNext_WhenEndOffsetIsLastIndex_ReturnsFalse()
        {
            // Arrange
            var context = CreateContext(
                first: 10,
                after: ConnectionUtils.OffsetToCursor(989)
            );

            // Act
            var slice = SliceMetrics.Create(DbContext.Blogs, context);

            // Assert
            Assert.Equal(990, slice.StartIndex);
            Assert.Equal(10, slice.SliceSize);
            Assert.False(slice.HasNext);
        }

        [Fact]
        public void HasNext_WhenEndOffsetIsLessThanLastIndex_ReturnsTrue()
        {
            // Arrange
            var context = CreateContext(
                first: 10,
                after: ConnectionUtils.OffsetToCursor(0)
            );

            // Act
            var slice = SliceMetrics.Create(DbContext.Blogs, context);

            // Assert
            Assert.Equal(1, slice.StartIndex);
            Assert.Equal(10, slice.SliceSize);
            Assert.True(slice.HasNext);
        }

        [Fact]
        public void HasPrevious_WhenDataIsSliceAndStartOffsetIsGreater0_ReturnsTrue()
        {
            // Arrange
            var context = CreateContext(
                first: 2,
                after: ConnectionUtils.OffsetToCursor(9)
            );

            // Act
            var slice = SliceMetrics.Create(DbContext.Blogs, context);

            // Assert
            Assert.Equal(10, slice.StartIndex);
            Assert.Equal(2, slice.SliceSize);
            Assert.True(slice.HasPrevious);
        }

        [Fact]
        public void HasPrevious_WhenDataIsSliceAndStartOffsetIsZero_ReturnsFalse()
        {
            // Arrange
            var context = CreateContext(
                first: 2
            );

            // Act
            var slice = SliceMetrics.Create(DbContext.Blogs, context);

            // Assert
            Assert.Equal(0, slice.StartIndex);
            Assert.Equal(2, slice.SliceSize);
            Assert.False(slice.HasPrevious);
        }

        [Fact]
        public void HasPrevious_WhenStartOffsetIsGreater0_ReturnsTrue()
        {
            // Arrange
            var context = CreateContext(
                after: ConnectionUtils.OffsetToCursor(0)
            );

            // Act
            var slice = SliceMetrics.Create(DbContext.Blogs, context);

            // Assert
            Assert.Equal(1, slice.StartIndex);
            Assert.True(slice.HasPrevious);
        }

        [Fact]
        public void HasPrevious_WhenStartOffsetIsZero_ReturnsFalse()
        {
            // Arrange
            var context = CreateContext();

            // Act
            var slice = SliceMetrics.Create(DbContext.Blogs, context);

            // Assert
            Assert.Equal(0, slice.StartIndex);
            Assert.False(slice.HasPrevious);
        }
    }
}
