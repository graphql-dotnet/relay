using System.Collections.Generic;
using GraphQL.Types;

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
            where TMutationInput : MutationInputGraphType
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
                    // TODO: is this correct??
                    return ((TMutationType)c.FieldDefinition.ResolvedType).MutateAndGetPayload(new MutationInputs(inputs), c);
                }
            );
        }
    }
}