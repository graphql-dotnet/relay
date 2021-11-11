using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

    public class Users : ConcurrentDictionary<string, User>
    {
        public Users() : base()
        {
            this["me"] = new User { Id = "me" };
        }
    }


    internal class TodoDatabaseContext
    {
        public readonly Users users;

        internal TodoDatabaseContext()
        {
            users = new Users();
        }
    }

    public static class Database
    {
        private static TodoDatabaseContext _context
            = new TodoDatabaseContext();

        readonly static Lazy<TodosContext> todosContext = new(
            () => new TodosContext(
                new DbContextOptionsBuilder<TodosContext>()
                        .UseInMemoryDatabase("TestDatabase")
                        .Options));

        public static User GetUser(string id)
        {
            return _context.users[id];
        }

        public static User GetViewer()
        {
            return GetUser("me");
        }

        public static Todo AddTodo(string text, bool complete = false)
        {
            var todo = new Todo
            {
                Id = Guid.NewGuid().ToString(),
                Text = text,
                Completed = complete,
            };

            todosContext.Value.Add(todo);
            todosContext.Value.SaveChanges();

            return todo;
        }

        public static Todo GetTodoById(string id) => todosContext.Value
            .Set<Todo>()
            .FirstOrDefault(t => t.Id == id);

        public static IQueryable<Todo> GetTodos() => GetTodosByStatus();

        public static IQueryable<Todo> GetTodosByStatus(string status = "any") => todosContext.Value
            .Set<Todo>()
            .Where(t => status == "any" || (t.Completed && (status == "completed")));

        public static Todo ChangeTodoStatus(string id, bool complete)
        {
            var todo = GetTodoById(id);
            todo.Completed = complete;

            todosContext.Value.SaveChanges();

            return todo;
        }

        public static IEnumerable<Todo> MarkAllTodos(bool complete)
        {
            var todos = GetTodosByStatus();

            foreach (var todo in todos)
            {
                todo.Completed = complete;
            }

            todosContext.Value.SaveChanges();

            return todos;
        }

        public static Todo RemoveTodo(string id)
        {
            var todo = todosContext.Value.Remove(GetTodoById(id)).Entity;

            todosContext.Value.SaveChanges();

            return todo;
        }

        public static Todo RenameTodo(string id, string text)
        {
            var todo = GetTodoById(id);
            todo.Text = text;

            todosContext.Value.SaveChanges();

            return todo;
        }

        public static IEnumerable<string> RemoveCompletedTodos()
        {
            var deleted = new List<string>();

            foreach (var todo in GetTodosByStatus("completed"))
            {
                deleted.Add(todo.Id);
                todosContext.Value.Remove(todo);
            }

            todosContext.Value.SaveChanges();

            return deleted;
        }

        public static User GetUserById(string id)
        {
            return _context.users[id];
        }

    }
}
