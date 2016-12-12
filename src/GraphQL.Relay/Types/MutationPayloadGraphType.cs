using GraphQL.Language.AST;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQL.Relay.Types
{
    public interface IMutationPayload<out T>
    {
        T MutateAndGetPayload(MutationInputs inputs, ResolveFieldContext<object> context);
    }


    public abstract class MutationPayloadGraphType<TSource, TOut> : ObjectGraphType<TSource>, IMutationPayload<TOut>
    {
        public abstract TOut MutateAndGetPayload(MutationInputs inputs, ResolveFieldContext<object> context);

        public MutationPayloadGraphType()
        {
            Field(
                name: "clientMutationId",
                type: typeof(StringGraphType),
                resolve: c =>
                {
                    return GetClientId(c);
                });
        }

        private string GetClientId(ResolveFieldContext<TSource> context)
        {
            var Field = context.Operation.SelectionSet.Selections
                .Where(s => s is Field)
                .Cast<Field>()
                .First(s => isCorrectSelection(context, s));

            var Arg = Field.Arguments.First(a => a.Name == "input") as Argument;

            if (Arg.Value is VariableReference)
            {
                string name = ((VariableReference) Arg.Value).Name;
                var inputs = context.Variables.First(v => v.Name == name).Value as Dictionary<string, object>;

                return inputs["clientMutationId"] as string;
            }

            var Value = ((ObjectValue)Arg.Value).ObjectFields.First(f => f.Name == "clientMutationId").Value as StringValue;
            return Value.Value;
        }

        private bool isCorrectSelection(ResolveFieldContext<TSource> context, Field field)
        {
            return field.SelectionSet.Selections.Any(s => s.SourceLocation.Equals(context.FieldAst.SourceLocation));
        }
    }


    public abstract class MutationPayloadGraphType<TSource> : MutationPayloadGraphType<TSource, TSource> { }
    public abstract class MutationPayloadGraphType : MutationPayloadGraphType<object> { }
}
