using Dastone.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Dastone.Controllers
{
    public class PagesController : Controller
    {
        private readonly ILogger<StatisticsController> _logger;

        public PagesController(ILogger<StatisticsController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index() => View();
        public IActionResult blogs() => View();
        public IActionResult faqs() => View();
        public IActionResult pricing() => View();
        public IActionResult profile() => View();
        public IActionResult starter() => View();
        public IActionResult timeline() => View();
        public IActionResult treeview() => View();
    }
}