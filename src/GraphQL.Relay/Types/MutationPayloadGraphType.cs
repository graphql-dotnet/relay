using System.Collections.Generic;
using System.Linq;
using GraphQL.Language.AST;
using GraphQL.Types;

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
                .Where(s => s is Field)
                .Cast<Field>()
                .First(s => IsCorrectSelection(context, s));

            var arg = field.Arguments.First(a => a.Name == "input");

            if (arg.Value is VariableReference)
            {
                var name = ((VariableReference) arg.Value).Name;
                var inputs = context.Variables.First(v => v.Name == name).Value as Dictionary<string, object>;

                return inputs["clientMutationId"] as string;
            }

            var value =
                ((ObjectValue) arg.Value).ObjectFields.First(f => f.Name == "clientMutationId").Value as StringValue;
            return value.Value;
        }

        private bool IsCorrectSelection(IResolveFieldContext<TSource> context, Field field)
        {
            return Enumerable.Any(field.SelectionSet.Selections,
                s => s.SourceLocation.Equals(context.FieldAst.SourceLocation));
        }
    }


    public abstract class MutationPayloadGraphType<TSource> : MutationPayloadGraphType<TSource, TSource>
    {
    }

    public abstract class MutationPayloadGraphType : MutationPayloadGraphType<object>
    {
    }
}