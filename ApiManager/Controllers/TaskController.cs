using ApiManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskProcessing.Core.Repositories;

namespace ApiManager.Controllers
{
    public class TaskController : Controller
    {
        private readonly ILogger<TaskController> _logger;
        private readonly ISchedulerRepository _schedulerRepository;

        public TaskController(ILogger<TaskController> logger, ISchedulerRepository schedulerRepository)
        {
            _logger = logger;
            _schedulerRepository = schedulerRepository;
        }

        public IActionResult Index()
        {

            var schedulerItems = _schedulerRepository.GetSchedulers();
            IList<SelectListItem> schedulers = new List<SelectListItem>();
            foreach (var schedulerItem in schedulerItems)
            {
                schedulers.Add(new SelectListItem
                {
                    Value = schedulerItem.Id.ToString(),
                    Text = schedulerItem.HostName,
                    Selected = schedulerItem.DefaultHost
                });
            }

            return View(new TaskViewModel
            {
                Schedulers = schedulers
            });
        }
    }
}
