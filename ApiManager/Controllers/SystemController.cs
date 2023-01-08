using Microsoft.AspNetCore.Mvc;
using ApiManager._keenthemes.libs;

namespace ApiManager.Controllers;

public class SystemController : Controller
{

    private readonly ILogger<DashboardsController> _logger;
    private readonly IKTTheme _theme;

    public SystemController(ILogger<DashboardsController> logger, IKTTheme theme)
    {
        _logger = logger;
        _theme = theme;
    }

    public IActionResult notFound()
    {
        return View(_theme.getPageView("System", "NotFound.cshtml"));
    }

    public IActionResult error()
    {
        return View(_theme.getPageView("System", "Error.cshtml"));
    }
}
