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
            return Field(name, typeof(TMutationType))
                .Argument<NonNullGraphType<TMutationInput>>("input")
                .Resolve(c =>
                {
                    var inputs = c.GetArgument<Dictionary<string, object>>("input");
                    return ((TMutationType)c.FieldDefinition.ResolvedType).MutateAndGetPayload(new MutationInputs(inputs), c);
                })
                .FieldType;
        }
    }
}
