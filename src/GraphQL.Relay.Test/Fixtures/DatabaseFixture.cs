using System;
using System.Data.Common;
using System.Linq;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GraphQL.Relay.Test.Fixtures
{
    public class DatabaseFixture : IDisposable
    {
        private readonly DbConnection _connection;
        private readonly DbContextOptions<BloggingContext> _contextOptions;

        public DatabaseFixture()
        {
            // Create and open a connection. This creates the SQLite in-memory database, which will persist until the connection is closed
            // at the end of the test (see Dispose below).
            _connection = new SqliteConnection("Filename=:memory:");
            _connection.Open();

            // These options will be used by the context instances in this test suite, including the connection opened above.
            _contextOptions = new DbContextOptionsBuilder<BloggingContext>()
                .UseSqlite(_connection)
                .Options;

            // Create the schema and seed some data
            Context = new(_contextOptions);

            _ = Context.Database.EnsureCreated();

            var blogs = Enumerable.Range(1, 1000).Select(i => new Blog
            {
                Name = $"Blog{i}",
                Url = $"http://sample.com/{i}"
            });

            Context.AddRange(blogs);

            _ = Context.SaveChanges();
        }

        public void Dispose()
        {
            // Remove all records from the database
            Context.RemoveRange(Context.Blogs);
            _ = Context.SaveChanges();

            Context.Dispose();
            _connection.Dispose();
            GC.SuppressFinalize(this);
        }

        public BloggingContext Context { get; }

        public class Blog
        {
            public int BlogId { get; set; }
            public string Name { get; set; }
            public string Url { get; set; }
        }

        public class BloggingContext : DbContext
        {
            public BloggingContext(DbContextOptions<BloggingContext> options) : base(options) {}

            public DbSet<Blog> Blogs => Set<Blog>();
        }
    }
}