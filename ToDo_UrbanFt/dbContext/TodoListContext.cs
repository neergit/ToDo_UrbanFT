using Microsoft.EntityFrameworkCore;
using ToDo_UrbanFt.Entities;

namespace ToDo_UrbanFt.dbContext
{
    public class TodoListContext : DbContext
	{
		public TodoListContext(DbContextOptions<TodoListContext> options):base(options)
		{
		}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();
        }

        public DbSet<Tasks> Tasks { get; set; }
	}
}

