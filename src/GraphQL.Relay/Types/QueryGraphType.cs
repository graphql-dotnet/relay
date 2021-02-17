﻿using GraphQL.Types;

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

        private object ResolveObjectFromGlobalId(IResolveFieldContext<object> context)
        {
            var globalId = context.GetArgument<string>("id");
            var parts = Node.FromGlobalId(globalId);
            var node = (IRelayNode<object>) context.Schema.FindType(parts.Type);

            return node.GetById(parts.Id);
        }
    }
}
