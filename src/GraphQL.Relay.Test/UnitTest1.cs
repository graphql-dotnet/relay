using System;
using Xunit;

namespace GraphQL.Relay.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var z = SystemTextJson.StringExtensions.FromJson<MyClass>("{}");
        }

        public class MyClass
        {

        }
    }
}
