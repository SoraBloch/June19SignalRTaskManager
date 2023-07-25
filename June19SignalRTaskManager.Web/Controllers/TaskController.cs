using June19SignalRTaskManager.Data;
using June5apis.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace June19SignalRTaskManager.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly string _connectionString;
        private IHubContext<TasksHub> _hub;  

        public TaskController(IConfiguration configuration, IHubContext<TasksHub> hub)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _hub = hub;
        }
        [HttpGet]
        [Route("getalltasks")]
        public List<TaskItem> GetAllTasks()
        {
            var repo = new TaskRepository(_connectionString);
            return repo.GetAllTasks();
        }
        [HttpPost]
        [Route("addtask")]
        public void AddTask(string taskTitle)
        {
            var repo = new TaskRepository(_connectionString);
            var task = repo.AddTask(taskTitle);
            _hub.Clients.All.SendAsync("taskAdded", task);
        }
        [HttpPost]
        [Route("deletetask")]
        public void DeleteTask(int taskId)
        {
            var repo = new TaskRepository(_connectionString);
            repo.DeleteTask(taskId);
            _hub.Clients.All.SendAsync("taskDeleted", taskId);
        }
        [HttpPost]
        [Route("adduseridtotask")]
        public void AddUserIdToTask(int taskId)
        {
            var repo = new TaskRepository(_connectionString);
            var user = GetCurrentUser();
            repo.AddUserToTask(taskId, user.Id);
            _hub.Clients.All.SendAsync("taskStatusChanged", repo.GetAllTasks());
        }
        [HttpGet]
        [Route("getcurrentuser")]
        public User GetCurrentUser()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }

            var repo = new UserRepository(_connectionString);
            return repo.GetByEmail(User.Identity.Name);
        }
    }
}
