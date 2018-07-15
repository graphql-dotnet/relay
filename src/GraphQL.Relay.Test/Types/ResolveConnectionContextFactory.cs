using System.Collections.Generic;
using GraphQL.Builders;
using GraphQL.Execution;
using GraphQL.Language.AST;
using GraphQL.Types;

namespace GraphQL.Relay.Test.Types
{
    public class ResolveConnectionContextFactory
    {
        public static ResolveConnectionContext<TestParent> CreateContext(int? first = null, string after = null,
            int? last = null, string before = null)
        {
            return new ResolveConnectionContext<TestParent>(
                new ResolveFieldContext(new ExecutionContext(),
                    new Field(),
                    new FieldType(),
                    new TestParent(),
                    new ObjectGraphType(),
                    new Dictionary<string, object>
                    {
                        ["first"] = first,
                        ["last"] = last,
                        ["after"] = after,
                        ["before"] = before
                    },
                    new[] {"children"}), false, null);
        }

        public class TestParent
        {
        }
    }
}