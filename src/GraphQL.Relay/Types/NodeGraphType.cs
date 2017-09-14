using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQL.Types.Relay;
using Panic.StringUtils;

namespace GraphQL.Relay.Types
{
    public interface IRelayNode<out T>
    {
        T GetById(string id);
    }

    public static class Node
    {
        public static NodeGraphType<TSource, TOut> For<TSource, TOut>(Func<string, TOut> getById)
        {
            var type = new DefaultNodeGraphType<TSource, TOut>(getById);
            return type;
        }

        public static string ToGlobalId(string name, object id)
        {
            return StringUtils.Base64Encode("{0}:{1}".ToFormat(name, id));
        }

        public static Tuple<string, string> FromGlobalId(string globalId)
        {
            var parts = StringUtils.Base64Decode(globalId).Split(':');
            return new Tuple<string, string>(
                parts[0],
                string.Join(":", parts.Skip(count: 1))
            );
        }
    }


    public abstract class NodeGraphType<T, TOut> : ObjectGraphType<T>, IRelayNode<TOut>
    {
        public static Type Edge => typeof(EdgeType<NodeGraphType<T, TOut>>);

        public static Type Connection => typeof(ConnectionType<NodeGraphType<T, TOut>>);

        protected NodeGraphType()
        {
            Interface<NodeInterface>();
        }

        public abstract TOut GetById(string id);

        public FieldType Id<TReturnType>(Expression<Func<T, TReturnType>> expression)
        {
            string name = null;
            try
            {
                name = StringUtils.ToCamelCase(expression.NameOf());
            }
            catch
            {
            }


            return Id(name, expression);
        }

        public FieldType Id<TReturnType>(string name, Expression<Func<T, TReturnType>> expression)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                // if there is a field called "ID" on the object, namespace it to "contactId"
                if (name.ToLower() == "id")
                {
                    if (string.IsNullOrWhiteSpace(Name))
                        throw new InvalidOperationException(
                            "The parent GraphQL type must define a Name before declaring the Id field " +
                            "in order to properly prefix the local id field");

                    name = StringUtils.ToCamelCase(Name + "Id");
                }

                Field(name, expression)
                    .Description($"The Id of the {Name ?? "node"}")
                    .FieldType.Metadata["RelayLocalIdField"] = true;
            }

            var field = Field(
                name: "id",
                description: $"The Global Id of the {Name ?? "node"}",
                type: typeof(NonNullGraphType<IdGraphType>),
                resolve: context => Node.ToGlobalId(
                    context.ParentType.Name,
                    expression.Compile()(context.Source)
                )
            );

            field.Metadata["RelayGlobalIdField"] = true;

            if (!string.IsNullOrWhiteSpace(name))
                field.Metadata["RelayRelatedLocalIdField"] = name;

            return field;
        }
    }

    public abstract class NodeGraphType<TSource> : NodeGraphType<TSource, TSource>
    {
    }

    public abstract class NodeGraphType : NodeGraphType<object>
    {
    }

    public abstract class AsyncNodeGraphType<T> : NodeGraphType<T, Task<T>>
    {
    }

    public class DefaultNodeGraphType<TSource, TOut> : NodeGraphType<TSource, TOut>
    {
        private readonly Func<string, TOut> _getById;

        public DefaultNodeGraphType(Func<string, TOut> getById)
        {
            _getById = getById;
        }

        public override TOut GetById(string id)
        {
            return _getById(id);
        }
    }
}