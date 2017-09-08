using GraphQL.Types;

namespace GraphQL.Relay.Types
{
    public class MutationInputGraphType : InputObjectGraphType
    {
        public MutationInputGraphType()
        {
            Field<StringGraphType>("clientMutationId");
        }
    }
}