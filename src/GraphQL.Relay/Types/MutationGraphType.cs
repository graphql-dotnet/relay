using GraphQL.Types;
using System.Collections.Generic;

namespace GraphQL.Relay.Types
{
    public class MutationGraphType : ObjectGraphType
    {
        public MutationGraphType()
        {
            Name = "Mutation";
        }

        public FieldType Mutation<TMutationInput, TMutationType>(string name)
            where TMutationType : IMutationPayload<object>
            where TMutationInput : InputObjectGraphType
        {
            return Field(
                name: name,
                type: typeof(TMutationType),
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<TMutationInput>> { Name = "input" }
                ),
                resolve: c =>
                {
                    var inputs = c.GetArgument<Dictionary<string, object>>("input");
                    return ((TMutationType)c.FieldDefinition.ResolvedType).MutateAndGetPayload(new MutationInputs(inputs), c);
                }
            );
        }
    }
}
