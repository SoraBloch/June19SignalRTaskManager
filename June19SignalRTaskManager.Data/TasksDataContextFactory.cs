using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace June19SignalRTaskManager.Data
{
    public class TasksDataContextFactory : IDesignTimeDbContextFactory<TasksDataContext>
    {
        public TasksDataContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}June19SignalRTaskManager.Web"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            return new TasksDataContext(config.GetConnectionString("ConStr"));
        }
    }
}