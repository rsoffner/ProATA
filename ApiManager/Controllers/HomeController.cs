using ApiManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using TaskProcessing.Core.Repositories;

namespace ApiManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ISchedulerRepository _schedulerRepository;

        public HomeController(ILogger<HomeController> logger, ISchedulerRepository schedulerRepository)
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

            return View(new MonitorViewModel
            {
                Schedulers = schedulers
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}