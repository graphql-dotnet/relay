using GraphQL.Types;

namespace GraphQL.Relay.Types
{
    public interface IMutationPayload<out T>
    {
        T MutateAndGetPayload(MutationInputs inputs, IResolveFieldContext<object> context);
    }

    public abstract class MutationPayloadGraphType<TSource, TOut> : ObjectGraphType<TSource>, IMutationPayload<TOut>
    {
        public abstract TOut MutateAndGetPayload(MutationInputs inputs, IResolveFieldContext<object> context);
    }

    public abstract class MutationPayloadGraphType<TSource> : MutationPayloadGraphType<TSource, TSource>
    {
    }

    public abstract class MutationPayloadGraphType : MutationPayloadGraphType<object>
    {
    }
}
