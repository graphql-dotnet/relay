using GraphQL.Types;
using GraphQLParser.AST;

namespace GraphQL.Relay.Types
{
    public interface IMutationPayload<out T>
    {
        T MutateAndGetPayload(MutationInputs inputs, IResolveFieldContext<object> context);
    }


    public abstract class MutationPayloadGraphType<TSource, TOut> : ObjectGraphType<TSource>, IMutationPayload<TOut>
    {
        protected MutationPayloadGraphType()
        {
            Field(
                name: "clientMutationId",
                type: typeof(StringGraphType),
                resolve: GetClientId);
        }

        public abstract TOut MutateAndGetPayload(MutationInputs inputs, IResolveFieldContext<object> context);

        private string GetClientId(IResolveFieldContext<TSource> context)
        {
            var field = context.Operation.SelectionSet.Selections
                .Where(s => s is GraphQLField)
                .Cast<GraphQLField>()
                .First(s => IsCorrectSelection(context, s));

            var arg = field.Arguments.First(a => a.Name == "input");

            if (arg.Value is GraphQLVariable variable)
            {
                var name = variable.Name;
                var inputs = context.Variables.First(v => v.Name == name).Value as Dictionary<string, object>;

                return inputs["clientMutationId"] as string;
            }

            var value =
                ((GraphQLObjectValue)arg.Value).Fields.First(f => f.Name == "clientMutationId").Value as GraphQLStringValue;
            return (string)value.Value; // TODO: string allocation
        }

        private bool IsCorrectSelection(IResolveFieldContext<TSource> context, GraphQLField field)
        {
            return Enumerable.Any(field.SelectionSet.Selections,
                s => s.Location.Equals(context.FieldAst.Location));
        }
    }


    public abstract class MutationPayloadGraphType<TSource> : MutationPayloadGraphType<TSource, TSource>
    {
    }

    public abstract class MutationPayloadGraphType : MutationPayloadGraphType<object>
    {
    }
}
