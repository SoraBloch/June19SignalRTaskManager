using June19SignalRTaskManager.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace June5apis.Data
{
    public class TasksDataContext : DbContext
    {
        private readonly string _connectionString;

        public TasksDataContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>().HasKey(t => t.Id); 
        }
        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
    }
}