using Microsoft.EntityFrameworkCore;
using Task = ToDo.Models.Task;

namespace ToDo.Data
{
    public class MyDbContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public MyDbContext(DbContextOptions<MyDbContext> opt) : base(opt)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed initial data
            modelBuilder.Entity<Task>().HasData(
               new Task() { Id = 1, Title = "Hometask", Description = "Write 3 cover letters and get offer)", StartDate = DateTime.Now },
               new Task() { Id = 2, Title = "Programming", Description = "Learn new Framework", StartDate = DateTime.Now },
               new Task() { Id = 3, Title = "Cook", Description = "Cook chicken on dinner", StartDate = DateTime.Now },
               new Task() { Id = 4, Title = "English", Description = "Time to learn new words", StartDate=DateTime.Now }
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
