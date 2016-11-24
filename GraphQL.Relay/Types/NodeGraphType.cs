using GraphQL.Types;
using Panic.StringUtils;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace GraphQL.Relay.Types
{
    public interface IRelayNode
    {
        object GetById(string id);
    }

    public static class Node
    {
        public static string ToGlobalId(string name, object id)
        {
            return StringUtils.Base64Encode("{0}:{1}".ToFormat(name, id));
        }

        public static Tuple<string, string> FromGlobalId(string globalId)
        {
            var parts = StringUtils.Base64Decode(globalId).Split(':');
            return new Tuple<string, string>(
                parts[0],
                string.Join(":", parts.Skip(1))
            );
        }
    }

    public abstract class NodeGraphType<T> : ObjectGraphType<T>, IRelayNode
    {
        public abstract object GetById(string id);

        public NodeGraphType()
        {
            Interface<NodeInterface>();
        }

        public FieldType Id<TReturnType>(Expression<Func<T, TReturnType>> expression)
        {
            string name = null;
            try
            {
                name = StringUtils.ToCamelCase(expression.NameOf());
            }
            catch { }


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

                Field(expression, nullable: false)
                    .Name(name)
                    .Description($"The Id of the {Name ?? "node"}");
            }
            
            return Field(
                name: "id",
                description: $"The Global Id of the {Name ?? "node"}",
                type: typeof(NonNullGraphType<IdGraphType>),
                resolve: context => Node.ToGlobalId(
                    context.ParentType.Name,
                    expression.Compile()(context.Source)
                )
            );
        }
    }
}
