using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace June19SignalRTaskManager.Data
{
    public class TaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public TaskItem AddTask(string taskTitle)
        {
            using var context = new TasksDataContext(_connectionString);
            var task = new TaskItem
            {
                Title = taskTitle
            };
            context.Tasks.Add(task);
            context.SaveChanges();
            return task;
        }
        public void DeleteTask(int taskId)
        {
            using var context = new TasksDataContext(_connectionString);
            var task = context.Tasks.FirstOrDefault(t => t.Id == taskId);
            context.Tasks.Remove(task);
            context.SaveChanges();
        }
        public List<TaskItem> GetAllTasks()
        {
            using var context = new TasksDataContext(_connectionString);
            return context.Tasks.Include(t => t.User).ToList();
        }
        public void AddUserToTask(int taskId, int userId)
        {
            var context = new TasksDataContext(_connectionString);
            var task = context.Tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                task.UserId = userId;
            }
            context.SaveChanges();
        }
    }
}
