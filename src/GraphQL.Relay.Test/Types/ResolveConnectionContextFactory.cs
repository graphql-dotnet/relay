using System.Collections.Generic;
using GraphQL.Builders;
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
                new ResolveFieldContext
                {
                    FieldAst = new Field(),
                    FieldDefinition = new FieldType(),
                    Source = new TestParent(),
                    ParentType = new ObjectGraphType(),
                    Arguments = new Dictionary<string, object>
                    {
                        ["first"] = first,
                        ["last"] = last,
                        ["after"] = after,
                        ["before"] = before
                    },
                    Path = new[] { "children" },
                }, isUnidirectional: false, defaultPageSize: null);
        }

        public class TestParent
        {
        }
    }
}
