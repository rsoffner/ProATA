using Microsoft.AspNetCore.Mvc;

namespace APIManager.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IFormCollection data)
        {
            var temp = data;


            return RedirectToAction(nameof(Index));
        }
    }
}
