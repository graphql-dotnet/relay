using GraphQL.Relay.Types;
using Xunit;
using static GraphQL.Relay.Test.Types.ResolveConnectionContextFactory;

namespace GraphQL.Relay.Test.Types
{
    public class ArraySliceMetricesTests
    {
        [Fact]
        public void Create_WhenCreateFromFullList_ReturnsListCount()
        {
            var list = new[] {1, 2, 3, 4};
            var slice = ArraySliceMetrics.Create(list);

            Assert.Equal(4, slice.TotalCount);
            Assert.Equal(4, slice.SliceSize);
            Assert.Equal(0, slice.StartIndex);
            Assert.Equal(0, slice.StartOffset);
            Assert.Equal(3, slice.EndIndex);
            Assert.Equal(3, slice.EndOffset);
            Assert.False(slice.HasNext);
            Assert.False(slice.HasPrevious);
        }

        [Fact]
        public void Create_WhenCreateFromFullListAndContext_ReturnsListCount()
        {
            var list = new[] {1, 2, 3, 4};
            var slice = ArraySliceMetrics.Create(list, CreateContext());

            Assert.Equal(4, slice.TotalCount);
            Assert.Equal(4, slice.SliceSize);
            Assert.Equal(0, slice.StartIndex);
            Assert.Equal(0, slice.StartOffset);
            Assert.Equal(3, slice.EndIndex);
            Assert.Equal(3, slice.EndOffset);
            Assert.False(slice.HasNext);
            Assert.False(slice.HasPrevious);
        }


        [Fact]
        public void Create_WhenFirstIsGreaterThanEdgeRange_DoesNotExtendRange()
        {
            var list = new[] {0, 1, 2, 3, 4, 5, 6, 7};
            var slice = ArraySliceMetrics.Create(list, 0, list.Length, 30,
                ConnectionUtils.OffsetToCursor(3));

            Assert.Equal(8, slice.TotalCount);
            Assert.Equal(8, slice.SliceSize);
            Assert.Equal(0, slice.StartIndex);
            Assert.Equal(4, slice.StartOffset);
            Assert.Equal(list.Length - 1, slice.EndIndex);
            Assert.Equal(7, slice.EndOffset);
            Assert.False(slice.HasNext);
            Assert.True(slice.HasPrevious);
            Assert.Equal(new[] {4, 5, 6, 7}, slice.Slice);
        }


        [Fact]
        public void Create_WhenRangeArgumentsCoverSlice_ClampsRange()
        {
            var list = new[] {0, 1, 2, 3, 4, 5, 6, 7};
            var slice = ArraySliceMetrics.Create(list, 0, list.Length, 2,
                ConnectionUtils.OffsetToCursor(3));

            Assert.Equal(list.Length, slice.TotalCount);
            Assert.Equal(list.Length, slice.SliceSize);
            Assert.Equal(0, slice.StartIndex);
            Assert.Equal(4, slice.StartOffset);
            Assert.Equal(list.Length - 1, slice.EndIndex);
            Assert.Equal(5, slice.EndOffset);
            Assert.True(slice.HasNext);
            Assert.True(slice.HasPrevious);
            Assert.Equal(new[] {4, 5}, slice.Slice);
        }


        [Fact]
        public void Create_WhenStrictCheckEnabledAndDataEndsTooEarly_ThrowsException()
        {
            var list = new[] {0, 1, 2, 3, 4, 5, 6, 7};
            Assert.Throws<IncompleteSliceException>(() =>
                ArraySliceMetrics.Create(list, 0, 18, 9, strictCheck: true));
        }

        [Fact]
        public void Create_WhenStrictCheckEnabledAndDataStartsTooLate_ThrowsException()
        {
            var list = new[] {0, 1, 2, 3, 4, 5, 6, 7};
            Assert.Throws<IncompleteSliceException>(() =>
                ArraySliceMetrics.Create(list, 1, 18, 5, strictCheck: true));
        }


        [Fact]
        public void CreateInNonStrictMode_WhenDataDoesNotCoverRequestedRange_AdjustsStartAndEndOffsets()
        {
            var list = new[] {1, 2, 3, 4};
            var slice = ArraySliceMetrics.Create(list, 4, 10, strictCheck: false);


            Assert.Equal(10, slice.TotalCount);
            Assert.Equal(4, slice.SliceSize);
            Assert.Equal(4, slice.StartIndex);
            Assert.Equal(4, slice.StartOffset);
            Assert.Equal(7, slice.EndIndex);
            Assert.Equal(7, slice.EndOffset);
            Assert.True(slice.HasNext);
            Assert.True(slice.HasPrevious);
            Assert.Equal(list, slice.Slice);
        }

        [Fact]
        public void CreateInNonStrictMode_WhenDataSliceEndsBeforeRange_ReturnsEmptyResult()
        {
            var list = new[] {0, 1, 2, 3, 4, 5, 6, 7};
            var slice = ArraySliceMetrics.Create(list, 1, 20, after: ConnectionUtils.OffsetToCursor(10),
                strictCheck: false);
            Assert.True(slice.IsEmpty);
            Assert.Equal(8, slice.SliceSize);
            Assert.Equal(1, slice.StartIndex);
            Assert.Equal(8, slice.EndIndex);

            Assert.Equal(11, slice.StartOffset);
            Assert.Equal(10, slice.EndOffset);

            Assert.False(slice.HasNext);
            Assert.False(slice.HasPrevious);
            Assert.Equal(new int[] { }, slice.Slice);
        }

        [Fact]
        public void CreateInNonStrictMode_WhenDataSliceStartsAfterRange_ReturnsEmptyResult()
        {
            var list = new[] {0, 1, 2, 3, 4, 5, 6, 7};
            var slice = ArraySliceMetrics.Create(list, 5, 20, 3, strictCheck: false);
            Assert.True(slice.IsEmpty);
            Assert.Equal(8, slice.SliceSize);
            Assert.Equal(5, slice.StartIndex);
            Assert.Equal(12, slice.EndIndex);

            Assert.Equal(5, slice.StartOffset);
            Assert.Equal(4, slice.EndOffset);

            Assert.False(slice.HasNext);
            Assert.False(slice.HasPrevious);
            Assert.Equal(new int[] { }, slice.Slice);
        }

        [Fact]
        public void HasNext_WhenDataIsSliceAndEndOffsetIsLastIndex_ReturnsFalse()
        {
            var list = new[] {0, 1, 2, 3, 4, 5, 6, 7};
            var slice = ArraySliceMetrics.Create(list, 10, 18, last: 1);

            Assert.Equal(17, slice.StartOffset);
            Assert.Equal(17, slice.EndOffset);
            Assert.False(slice.HasNext);
        }


        [Fact]
        public void HasNext_WhenDataIsSliceAndEndOffsetIsLessThanLastIndex_ReturnsTrue()
        {
            var list = new[] {0, 1, 2, 3, 4, 5, 6, 7};
            var slice = ArraySliceMetrics.Create(list, 10, 18, last: 2, before: ConnectionUtils.OffsetToCursor(17));

            Assert.Equal(15, slice.StartOffset);
            Assert.Equal(16, slice.EndOffset);
            Assert.True(slice.HasNext);
        }

        [Fact]
        public void HasNext_WhenEndOffsetIsLastIndex_ReturnsFalse()
        {
            var list = new[] {0, 1, 2, 3, 4, 5, 6, 7};
            var slice = ArraySliceMetrics.Create(list, 0, list.Length, last: 1);

            Assert.Equal(7, slice.StartOffset);
            Assert.Equal(7, slice.EndOffset);
            Assert.False(slice.HasNext);
        }

        [Fact]
        public void HasNext_WhenEndOffsetIsLessThanLastIndex_ReturnsTrue()
        {
            var list = new[] {0, 1, 2, 3, 4, 5, 6, 7};
            var slice = ArraySliceMetrics.Create(list, 0, list.Length, list.Length - 1);

            Assert.Equal(0, slice.StartOffset);
            Assert.Equal(6, slice.EndOffset);
            Assert.True(slice.HasNext);
        }

        [Fact]
        public void HasPrevious_WhenDataIsSliceAndStartOffsetIsGreater0_ReturnsTrue()
        {
            var list = new[] {0, 1, 2, 3, 4, 5, 6, 7};
            var slice = ArraySliceMetrics.Create(list, 10, 20, 2, ConnectionUtils.OffsetToCursor(9));

            Assert.Equal(10, slice.StartOffset);
            Assert.Equal(11, slice.EndOffset);
            Assert.True(slice.HasPrevious);
        }

        [Fact]
        public void HasPrevious_WhenDataIsSliceAndStartOffsetIsZero_ReturnsFalse()
        {
            var list = new[] {0, 1, 2, 3, 4, 5, 6, 7};
            var slice = ArraySliceMetrics.Create(list, 0, 20, 2);

            Assert.Equal(0, slice.StartOffset);
            Assert.Equal(1, slice.EndOffset);
            Assert.False(slice.HasPrevious);
        }

        [Fact]
        public void HasPrevious_WhenStartOffsetIsGreater0_ReturnsTrue()
        {
            var list = new[] {0, 1, 2, 3, 4, 5, 6, 7};
            var slice = ArraySliceMetrics.Create(list, 0, list.Length, after: ConnectionUtils.OffsetToCursor(0));

            Assert.Equal(1, slice.StartOffset);
            Assert.Equal(7, slice.EndOffset);
            Assert.True(slice.HasPrevious);
        }

        [Fact]
        public void HasPrevious_WhenStartOffsetIsZero_ReturnsFalse()
        {
            var list = new[] {0, 1, 2, 3, 4, 5, 6, 7};
            var slice = ArraySliceMetrics.Create(list, 0, list.Length);

            Assert.Equal(0, slice.StartOffset);
            Assert.Equal(7, slice.EndOffset);
            Assert.False(slice.HasPrevious);
        }
    }
}
