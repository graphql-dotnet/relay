using GraphQL.Relay.Types;
using GraphQL.Types;
using GraphQL.Types.Relay;
using GraphQL.Types.Relay.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQL.Relay.Todo.Schema
{
    public class TodoMutation : ObjectGraphType
    {
        public TodoMutation()
        {
            Field<AddTodoPayload>(
                "addTodo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<AddTodoInput>> { Name = "input" }
                ),
                resolve: context =>
                {
                    var todo = Database.AddTodo(context.GetArgument<Todo>("input").Text);

                    return new
                    {
                        TodoEdge = new Edge<Todo>
                        {
                            Node = todo,
                            Cursor = ConnectionUtils.CursorForObjectInConnection(Database.GetTodos(), todo)
                        },
                        Viewer = Database.GetViewer(),
                    };
                });
        }
    }

    public class AddTodoInput : InputObjectGraphType
    {
        public AddTodoInput()
        {
            Name = "AddTodoInput";

            Field<StringGraphType>("text");
        }
    }

    public class AddTodoPayload : ObjectGraphType
    {
        public AddTodoPayload()
        {
            Name = "AddTodoPayload";
            Field<EdgeType<TodoGraphType>>("todoEdge");
            Field<UserGraphType>("viewer");
        }
    }
}
