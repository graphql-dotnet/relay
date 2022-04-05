using GraphQL.Relay.Utilities;

namespace GraphQL.Relay.Test.Utilities
{
    public class EdgeRangeTests
    {
        [Fact]
        public void Ctor_WhenStartOffsetIsNegative_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new EdgeRange(-1, 0));
        }

        [Fact]
        public void Ctor_WhenEndOffsetIsLessThanMinusOne_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new EdgeRange(0, -2));
        }

        [Theory]
        [InlineData(3)]
        [InlineData(6)]
        public void Count_WhenStartEqualsEnd_Returns1(int offset)
        {
            var range = new EdgeRange(offset, offset);
            Assert.Equal(1, range.Count);
        }

        [Theory]
        [InlineData(10, 9)]
        [InlineData(10, 8)]
        public void Ctor_WhenEndOffsetIsLessThanStartOffset_ClampsEndOffsetToOneLessThanStart(int start, int end)
        {
            var range = new EdgeRange(start, end);
            Assert.Equal(start, range.StartOffset);
            Assert.Equal(start - 1, range.EndOffset);
            Assert.Equal(0, range.Count);
            Assert.True(range.IsEmpty);
        }

        [Fact]
        public void LimitCountFromStart_IfMaxLengthIsNegative_ThrowsException()
        {
            var range = new EdgeRange(0, 10);
            Assert.Throws<ArgumentOutOfRangeException>(() => range.LimitCountFromStart(-1));
        }

        [Fact]
        public void LimitCountFromStart_WhenProvidingMaxLengthLessThanCount_MovesEndOffet()
        {
            var range = new EdgeRange(0, 9);
            Assert.Equal(10, range.Count);

            range.LimitCountFromStart(5);

            Assert.Equal(5, range.Count);
            Assert.Equal(4, range.EndOffset);
            Assert.Equal(0, range.StartOffset);
        }

        [Fact]
        public void LimitCountToEnd_IfMaxLengthIsNegative_ThrowsException()
        {
            var range = new EdgeRange(0, 10);
            Assert.Throws<ArgumentOutOfRangeException>(() => range.LimitCountToEnd(-1));
        }

        [Fact]
        public void LimitCountToEnd_WhenProvidingMaxLengthLessThanCount_MovesStartOffet()
        {
            var range = new EdgeRange(0, 9);
            Assert.Equal(10, range.Count);

            range.LimitCountToEnd(5);

            Assert.Equal(5, range.Count);
            Assert.Equal(9, range.EndOffset);
            Assert.Equal(5, range.StartOffset);
        }
    }
}
