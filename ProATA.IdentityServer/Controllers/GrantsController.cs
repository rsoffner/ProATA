using Microsoft.AspNetCore.Mvc;

namespace ProATA.IdentityServer.Controllers
{
    public class GrantsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
