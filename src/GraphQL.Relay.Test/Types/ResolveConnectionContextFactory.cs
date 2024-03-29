using GraphQL.Builders;
using GraphQL.Execution;

namespace GraphQL.Relay.Test.Types
{
    public class ResolveConnectionContextFactory
    {
        public static ResolveConnectionContext<TestParent> CreateContext(
            int? first = null,
            string after = null,
            int? last = null,
            string before = null,
            int? defaultPageSize = null
        ) => CreateContext<TestParent>(first, after, last, before, defaultPageSize);

        public static ResolveConnectionContext<TSource> CreateContext<TSource>(int? first = null, string after = null,
            int? last = null, string before = null, int? defaultPageSize = null) => new(
                new ResolveFieldContext() { },
                true,
                defaultPageSize
            )
            {
                Arguments = new Dictionary<string, ArgumentValue>
                {
                    ["first"] = new ArgumentValue(first, ArgumentSource.Variable),
                    ["last"] = new ArgumentValue(last, ArgumentSource.Variable),
                    ["after"] = new ArgumentValue(after, ArgumentSource.Variable),
                    ["before"] = new ArgumentValue(before, ArgumentSource.Variable)
                }
            };

        public class TestParent
        {
        }
    }
}
