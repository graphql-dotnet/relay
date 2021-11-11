using Microsoft.EntityFrameworkCore;

namespace GraphQL.Relay.Todo
{
    public class TodosContext : DbContext
    {
        public TodosContext(DbContextOptions<TodosContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region ConfigureItem

            modelBuilder.Entity<Todo>(
                b =>
                {
                    b.Property(e => e.Id);
                    b.Property(e => e.Text);
                    b.Property(e => e.Completed);
                });

            #endregion
        }
    }
}