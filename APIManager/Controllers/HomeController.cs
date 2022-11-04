using Microsoft.AspNetCore.Mvc;

namespace APIManager.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ApiQueue()
        {
            return View();
        }
    }
}
