using Microsoft.EntityFrameworkCore;
using ToDo_UrbanFt.Entities;

namespace ToDo_UrbanFt.dbContext
{
    public static class ModelBuilderExtension
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tasks>().HasData(
                new Tasks { Id = 1, Title = "Title1", Description = "Destcription1", Status = "Pending", CreatedAt = DateTime.Now },
                new Tasks { Id = 2, Title = "Title2", Description = "Destcription2", Status = "In Progress", CreatedAt = DateTime.Now },
                new Tasks { Id = 3, Title = "Title3", Description = "Destcription3", Status = "Completed", CreatedAt = DateTime.Now },
                new Tasks { Id = 4, Title = "Title4", Description = "Destcription4", Status = "Pending", CreatedAt = DateTime.Now },
                new Tasks { Id = 5, Title = "Title5", Description = "Destcription5", Status = "Pending", CreatedAt = DateTime.Now }
            );
        }
    }
}

