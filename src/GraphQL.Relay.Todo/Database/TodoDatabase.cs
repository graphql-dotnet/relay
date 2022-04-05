using System.Collections.Concurrent;

namespace GraphQL.Relay.Todo
{
    public class Todo
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public bool Completed { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
        // public Lazy<List<Todo>> Todos { get; set; }
    }

    public class Todos : ConcurrentDictionary<string, Todo>
    {
    }

    public class Users : ConcurrentDictionary<string, User>
    {
        public Users() : base()
        {
            this["me"] = new User { Id = "me" };
        }
    }

    internal class TodoDatabaseContext
    {
        public readonly Todos todos;
        public readonly Users users;

        internal TodoDatabaseContext()
        {
            todos = new Todos();
            users = new Users();
        }
    }

    public static class Database
    {
        private static readonly TodoDatabaseContext _context = new();

        public static User GetUser(string id) => _context.users[id];

        public static User GetViewer() => GetUser("me");

        public static Todo AddTodo(string text, bool complete = false)
        {
            var todo = new Todo
            {
                Id = Guid.NewGuid().ToString(),
                Text = text,
                Completed = complete,
            };

            _context.todos[todo.Id] = todo;
            return todo;
        }

        public static Todo GetTodoById(string id) => _context.todos[id];

        public static IEnumerable<Todo> GetTodos() => GetTodosByStatus();

        public static IEnumerable<Todo> GetTodosByStatus(string status = "any")
        {
            var todos = _context.todos.Select(t => t.Value);
            if (status == "any")
                return todos;
            return todos.Where(t => t.Completed == (status == "completed"));
        }

        private static Todo ChangeTodoStatus(Todo todo, bool complete)
        {
            todo.Completed = complete;
            return todo;
        }

        public static Todo ChangeTodoStatus(string id, bool complete)
        {
            return ChangeTodoStatus(GetTodoById(id), complete);
        }

        public static IEnumerable<Todo> MarkAllTodos(bool complete)
        {
            return GetTodosByStatus()
                .Select(t => ChangeTodoStatus(t, complete));
        }

        public static void RemoveTodo(string id)
        {
            _context.todos.Remove(id, out _);
        }

        public static Todo RenameTodo(string id, string text)
        {
            var todo = GetTodoById(id);
            todo.Text = text;
            return todo;
        }

        public static IEnumerable<string> RemoveCompletedTodos(bool complete)
        {
            complete.ToString(); // TODO: unused parameter, check this example

            var deleted = new List<string>();

            foreach (var todo in GetTodosByStatus("completed"))
            {
                deleted.Add(todo.Id);
                RemoveTodo(todo.Id);
            }

            return deleted;
        }

        public static User GetUserById(string id) => _context.users[id];
    }
}
