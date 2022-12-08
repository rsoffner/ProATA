using ApiManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskProcessing.Core.Repositories;

namespace ApiManager.Controllers
{
    public class LogController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly ITaskRepository _taskRepository;

        public LogController(ILogger<HomeController> logger, IUserRepository userRepository, ITaskRepository taskRepository)
        {
            _logger = logger;
            _userRepository = userRepository;
            _taskRepository = taskRepository;
        }

        public IActionResult Index()
        {
            var userItems = _userRepository.GetUsers();
            IList<SelectListItem> users = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "0",
                    Text = "- Select a user -"
                }
            };
            foreach (var item in userItems)
            {
                users.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.UserName
                });
            }

            var taskItems = _taskRepository.GetAllTasks();
            IList<SelectListItem> tasks = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = new Guid().ToString(),
                    Text = "- Select a task -"
                }
            };
            foreach (var item in taskItems)
            {
                tasks.Add(new SelectListItem
                {
                    Value = item.Id.ToString(),
                    Text = item.Title
                });
            }

            return View(new LogViewModel
            {
                Users = users,
                Tasks = tasks
            });
        }
    }
}
