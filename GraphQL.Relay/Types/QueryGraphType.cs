using GraphQL.Language.AST;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Relay.Types
{
    public class QueryGraphType : ObjectGraphType
    {
        public QueryGraphType()
        {
            Name = "Query";

            Field<NodeInterface>()
                .Name("node")
                .Description("Fetches an object given its global Id")
                .Argument<NonNullGraphType<IdGraphType>>("id", "The global Id of the object")
                .Resolve(ResolveObjectFromGlobalId);
        }

        private object ResolveObjectFromGlobalId(ResolveFieldContext<object> context)
        {
            string globalId = context.GetArgument<string>("id");
            var parts = Node.FromGlobalId(globalId);
            var node = (IRelayNode<object>)context.Schema.FindType(parts.Item1);

            return node.GetById(parts.Item2);
        }
    }
}
