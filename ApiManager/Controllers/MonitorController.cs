using ApiManager._keenthemes.libs;
using ApiManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using TaskProcessing.Core.Repositories;

namespace ApiManager.Controllers
{
    public class MonitorController : Controller
    {
        private readonly ILogger<MonitorController> _logger;
        private readonly ISchedulerRepository _schedulerRepository;
		private readonly IKTTheme _theme;

		public MonitorController(ILogger<MonitorController> logger, ISchedulerRepository schedulerRepository, IKTTheme theme)
		{
            _logger = logger;
            _schedulerRepository = schedulerRepository;
            _theme = theme;
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

            return View(_theme.getPageView("Monitor", "Index.cshtml"), new MonitorViewModel
            {
                Schedulers = schedulers
            });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}