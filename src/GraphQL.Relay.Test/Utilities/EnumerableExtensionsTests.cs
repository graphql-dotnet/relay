namespace GraphQL.Relay.Test.Utilities
{
    public class EnumerableExtensionsTests
    {

        [Fact]
        public void Slice_IfStartAndEndAreLessThanZero_ReturnsEnumerableStartingAtTheEnd()
        {
            var source = new[] { 1, 2, 3, 4, 5 };
            var slice = Relay.Utilities.EnumerableExtensions
                .Slice(source, -2, -1)
                .ToList();

            Assert.Equal(source[^2], slice[0]);
        }
    }
}
