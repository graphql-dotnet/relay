using System;
using GraphQL.Relay.Types;
using Xunit;

namespace GraphQL.Relay.Test.Types
{
    public class IncompleteSliceExceptionTests
    {
        [Fact]
        public void Message_WhenNoMessageWasPassedToCtor_ReturnsDefaultMessage()
        {
            var ex = new IncompleteSliceException();
            Assert.Equal("The provided data slice does not contain all expected items.", ex.Message);
        }

        [Fact]
        public void Message_WhenMessageIsProvidedInCtor_ReturnsPassedMessage()
        {
            var ex = new IncompleteSliceException("Test");
            Assert.Equal("Test", ex.Message);
            ex = new IncompleteSliceException("Test", "paramName");
            Assert.StartsWith($"Test{Environment.NewLine}", ex.Message);
            Assert.Equal("paramName", ex.ParamName);
        }

        [Fact]
        public void InnerException_WhenInnerExceptionIsPassedToCtor_ReturnsPassedException()
        {
            var inner = new Exception();
            var ex = new IncompleteSliceException("Test", inner);
            Assert.Equal("Test", ex.Message);
            Assert.Equal(inner, ex.InnerException);
            
            ex = new IncompleteSliceException("Test", "paramName", inner);
            Assert.StartsWith($"Test{Environment.NewLine}", ex.Message);
            Assert.Equal("paramName", ex.ParamName);
            Assert.Equal(inner, ex.InnerException);
        }
    }
}
