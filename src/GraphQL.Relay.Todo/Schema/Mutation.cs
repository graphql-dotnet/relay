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
                    var text = context.GetArgument<Todo>("input").Text;

                    var todo = Database.AddTodo(text);

                    return new
                    {
                        TodoEdge = new Edge<Todo>
                        {
                            Node = todo,
                            Cursor = ConnectionUtils.CursorForObjectInConnection(Database.GetTodos(), todo)
                        },
                        Viewer = Database.GetViewer()
                    };
                });

            Field<ChangeTodoStatusPayload>(
                "changeTodoStatus",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<ChangeTodoStatusInput>> { Name = "input" }
                ),
                resolve: context =>
                {
                    var todo = context.GetArgument<Todo>("input");

                    return new
                    {
                        Viewer = Database.GetViewer(),
                        Todo = Database.ChangeTodoStatus(Node.FromGlobalId(todo.Id).Id, todo.Complete)
                    };
                });

            Field<MarkAllTodosPayload>(
                "markAllTodos",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<MarkAllTodosInput>> { Name = "input" }
                ),
                resolve: context =>
                {
                    var todo = context.GetArgument<Todo>("input");

                    return new
                    {
                        Viewer = Database.GetViewer(),
                        ChangedTodos = Database.MarkAllTodos(todo.Complete)
                    };
                });

            Field<RemoveCompletedTodosPayload>(
                "removeCompletedTodos",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<RemoveCompletedTodosInput>> { Name = "input" }
                ),
                resolve: context =>
                {
                    var todo = context.GetArgument<Todo>("input");

                    return new
                    {
                        Viewer = Database.GetViewer(),
                        DeletedTodoIds = Database.RemoveCompletedTodos(todo.Complete).Select(id => Node.ToGlobalId("Todo", id))
                    };
                });



            Field<RemoveTodoPayload>(
                "removeTodo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<RemoveTodoInput>> { Name = "input" }
                ),
                resolve: context =>
                {
                    var todo = context.GetArgument<Todo>("input");

                    Database.RemoveTodo(Node.FromGlobalId(todo.Id).Id);

                    return new
                    {
                        Viewer = Database.GetViewer(),
                        deletedTodoId = todo.Id
                    };
                });


            Field<RenameTodoPayload>(
                "renameTodo",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<RenameTodoInput>> { Name = "input" }
                ),
                resolve: context =>
                {
                    var todo = context.GetArgument<Todo>("input");

                    return new
                    {
                        Viewer = Database.GetViewer(),
                        Todo = Database.RenameTodo(Node.FromGlobalId(todo.Id).Id, todo.Text)
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

    public class ChangeTodoStatusInput : InputObjectGraphType
    {
        public ChangeTodoStatusInput()
        {
            Name = "ChangeTodoStatusInput";

            Field<IdGraphType>("id");
            Field<BooleanGraphType>("complete");
        }
    }

    public class ChangeTodoStatusPayload : ObjectGraphType
    {
        public ChangeTodoStatusPayload()
        {
            Name = "ChangeTodoStatusPayload";

            Field<TodoGraphType>("todo");
            Field<UserGraphType>("viewer");
        }
    }

    public class MarkAllTodosInput : InputObjectGraphType
    {
        public MarkAllTodosInput()
        {
            Name = "MarkAllTodosInput";

            Field<BooleanGraphType>("complete");
        }
    }

    public class MarkAllTodosPayload : ObjectGraphType
    {
        public MarkAllTodosPayload()
        {
            Name = "MarkAllTodosPayload";

            Field<ListGraphType<TodoGraphType>>("changedTodos");
            Field<UserGraphType>("viewer");
        }
    }

    public class RemoveCompletedTodosInput : InputObjectGraphType
    {
        public RemoveCompletedTodosInput()
        {
            Name = "RemoveCompletedTodosInput";

            Field<BooleanGraphType>("complete");
        }
    }

    public class RemoveCompletedTodosPayload : ObjectGraphType
    {
        public RemoveCompletedTodosPayload()
        {
            Name = "RemoveCompletedTodosPayload";

            Field<ListGraphType<IdGraphType>>("deletedTodoIds");
            Field<UserGraphType>("viewer");
        }
    }

    public class RemoveTodoInput : InputObjectGraphType
    {
        public RemoveTodoInput()
        {
            Name = "RemoveTodoInput";

            Field<IdGraphType>("id");
        }
    }

    public class RemoveTodoPayload : ObjectGraphType
    {
        public RemoveTodoPayload()
        {
            Name = "RemoveTodoPayload";

            Field<IdGraphType>("deletedTodoId");
            Field<UserGraphType>("viewer");
        }
    }

    public class RenameTodoInput : InputObjectGraphType
    {
        public RenameTodoInput()
        {
            Name = "RenameTodoInput";

            Field<IdGraphType>("id");
            Field<StringGraphType>("text");
        }
    }

    public class RenameTodoPayload : ObjectGraphType
    {
        public RenameTodoPayload()
        {
            Name = "RenameTodoPayload";

            Field<TodoGraphType>("todo");
            Field<UserGraphType>("viewer");
        }
    }
}
