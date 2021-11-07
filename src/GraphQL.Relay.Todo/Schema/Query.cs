using System.Linq;
using GraphQL.Relay.Extensions;
using GraphQL.Relay.Types;
using GraphQL.Types;

namespace GraphQL.Relay.Todo.Schema
{
    public class TodoQuery: QueryGraphType {
        public TodoQuery() {
            Name = "Query";

            Field<UserGraphType>(
                name: "viewer",
                resolve: ctx => Database.GetViewer()
            );
        }
    }

    public class TodoGraphType: NodeGraphType<Todo>
    {
        public TodoGraphType() {
            Name = "Todo";

            Id(t => t.Id);
            Field(t => t.Text);
            Field("complete", t => t.Completed);
        }

        public override Todo GetById(IResolveFieldContext<object> context, string id) =>
            Database.GetTodoById(id);
    }

    public class UserGraphType: NodeGraphType<User>
    {
        public UserGraphType()
        {
            Name = "User";

            Id(t => t.Id);

            Connection<TodoGraphType>()
                .Name("todos")
                .Argument<StringGraphType, string>(
                    name: "status",
                    description: "Filter todos by their status",
                    defaultValue: "any"
                )
                .Resolve(ctx => ctx.ToConnection(
                    Database.GetTodosByStatus(ctx.GetArgument<string>("status"))
                ));

            Field<IntGraphType>(
                name: "totalCount",
                resolve: ctx => Database.GetTodos().Count()
            );
            Field<IntGraphType>(
                name: "completedCount",
                resolve: ctx => Database.GetTodosByStatus("completed").Count()
            );
        }

        public override User GetById(IResolveFieldContext<object> context, string id) =>
            Database.GetUserById(id);
    }


}
