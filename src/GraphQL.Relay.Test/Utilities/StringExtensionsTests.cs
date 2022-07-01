
namespace GraphQL.Relay.Test.Utilities
{
    public class StringExtensionsTests
    {
        [Fact]
        public void Base64Encode_IfValueIsNull_ReturnsNull()
        {
            var encoded = Relay.Utilities.StringExtensions.Base64Encode(null);
            Assert.Null(encoded);
        }

        [Fact]
        public void Base64Decode_IfValueIsNull_ReturnsNull()
        {
            var decoded = Relay.Utilities.StringExtensions.Base64Decode(null);
            Assert.Null(decoded);
        }
    }
}
