using System.Linq;
using GraphQL.Relay.Types;
using GraphQL.Types;
using GraphQL.Types.Relay;
using GraphQL.Types.Relay.DataObjects;

namespace GraphQL.Relay.Todo.Schema
{
    public class TodoMutation: MutationGraphType
    {
        public TodoMutation(): base() {
            Mutation<AddTodoInput, AddTodoPayload>("addTodo");
            Mutation<ChangeTodoStatusInput, ChangeTodoStatusPayload>("changeTodoStatus");
            Mutation<MarkAllTodosInput, MarkAllTodosPayload>("markAllTodos");
            Mutation<RemoveCompletedTodosInput, RemoveCompletedTodosPayload>("removeCompletedTodos");
            Mutation<RemoveTodoInput, RemoveTodoPayload>("removeTodo");
            Mutation<RenameTodoInput, RenameTodoPayload>("renameTodo");
        }
    }

    public class AddTodoInput: MutationInputGraphType {
        public  AddTodoInput() {
            Name = "AddTodoInput";

            Field<StringGraphType>("text");
        }
    }

    public class AddTodoPayload: MutationPayloadGraphType {

        public  AddTodoPayload() {
            Name = "AddTodoPayload";
            Field<EdgeType<TodoGraphType>>("todoEdge");
            Field<UserGraphType>("viewer");
        }

        public override object MutateAndGetPayload(
            MutationInputs inputs,
            ResolveFieldContext<object> context
        ) {
            var todo = Database.AddTodo(inputs.Get<string>("text"));

            return new {
                TodoEdge = new Edge<Todo> {
                    Node = todo,
                    Cursor = ConnectionUtils.CursorForObjectInConnection(Database.GetTodos(), todo)
                },
                Viewer = Database.GetViewer(),
            };
        }
    }

    public class ChangeTodoStatusInput: MutationInputGraphType {
        public  ChangeTodoStatusInput() {
            Name = "ChangeTodoStatusInput";

            Field<IdGraphType>("id");
            Field<BooleanGraphType>("complete");
        }
    }

    public class ChangeTodoStatusPayload: MutationPayloadGraphType {

        public  ChangeTodoStatusPayload() {
            Name = "ChangeTodoStatusPayload";

            Field<TodoGraphType>("todo");
            Field<UserGraphType>("viewer");
        }

        public override object MutateAndGetPayload(
            MutationInputs inputs,
            ResolveFieldContext<object> context
        ) {
            return new {
                Viewer = Database.GetViewer(),
                Todo = Database.ChangeTodoStatus(
                    Node.FromGlobalId(inputs.Get<string>("id")).Id,
                    inputs.Get<bool>("complete")
                ),
            };
        }
    }

    public class MarkAllTodosInput: MutationInputGraphType {
        public  MarkAllTodosInput() {
            Name = "MarkAllTodosInput";

            Field<BooleanGraphType>("complete");
        }
    }

    public class MarkAllTodosPayload: MutationPayloadGraphType {

        public  MarkAllTodosPayload() {
            Name = "MarkAllTodosPayload";

            Field<ListGraphType<TodoGraphType>>("changedTodos");
            Field<UserGraphType>("viewer");
        }

        public override object MutateAndGetPayload(
            MutationInputs inputs,
            ResolveFieldContext<object> context
        ) {
            return new {
                Viewer = Database.GetViewer(),
                ChangedTodos = Database.MarkAllTodos(
                    inputs.Get<bool>("complete")
                ),
            };
        }
    }

    public class RemoveCompletedTodosInput: MutationInputGraphType {
        public  RemoveCompletedTodosInput() {
            Name = "RemoveCompletedTodosInput";
        }
    }

    public class RemoveCompletedTodosPayload: MutationPayloadGraphType {

        public  RemoveCompletedTodosPayload() {
            Name = "RemoveCompletedTodosPayload";

            Field<ListGraphType<IdGraphType>>("deletedTodoIds");
            Field<UserGraphType>("viewer");
        }

        public override object MutateAndGetPayload(
            MutationInputs inputs,
            ResolveFieldContext<object> context
        ) {
            return new {
                Viewer = Database.GetViewer(),
                DeletedTodoIds = Database
                    .RemoveCompletedTodos(inputs.Get<bool>("complete"))
                    .Select(id => Node.ToGlobalId("Todo", id)),
            };
        }
    }

    public class RemoveTodoInput: MutationInputGraphType {
        public  RemoveTodoInput() {
            Name = "RemoveTodoInput";

            Field<IdGraphType>("id");
        }
    }

    public class RemoveTodoPayload: MutationPayloadGraphType {

        public  RemoveTodoPayload() {
            Name = "RemoveTodoPayload";

            Field<IdGraphType>("deletedTodoId");
            Field<UserGraphType>("viewer");
        }

        public override object MutateAndGetPayload(
            MutationInputs inputs,
            ResolveFieldContext<object> context
        ) {
            Database.RemoveTodo(
                Node.FromGlobalId(inputs.Get<string>("id")).Id
            );

            return new {
                Viewer = Database.GetViewer(),
                deletedTodoId = inputs.Get<string>("id"),
            };
        }
    }

    public class RenameTodoInput: MutationInputGraphType {
        public  RenameTodoInput() {
            Name = "RenameTodoInput";

            Field<IdGraphType>("id");
            Field<StringGraphType>("text");
        }
    }

    public class RenameTodoPayload: MutationPayloadGraphType {

        public  RenameTodoPayload() {
            Name = "RenameTodoPayload";

            Field<TodoGraphType>("todo");
            Field<UserGraphType>("viewer");
        }

        public override object MutateAndGetPayload(
            MutationInputs inputs,
            ResolveFieldContext<object> context
        ) {
            return new {
                Viewer = Database.GetViewer(),
                Todo = Database.RenameTodo(
                    Node.FromGlobalId(inputs.Get<string>("id")).Id,
                    inputs.Get<string>("text")
                ),
            };
        }
    }
}
