using Microsoft.AspNetCore.Mvc;
using ApiManager._keenthemes.libs;

namespace ApiManager.Controllers;

public class DashboardsController : Controller
{
    private readonly ILogger<DashboardsController> _logger;
    private readonly IKTTheme _theme;

    public DashboardsController(ILogger<DashboardsController> logger, IKTTheme theme)
    {
        _logger = logger;
        _theme = theme;
    }

    public IActionResult index()
    {
        return View(_theme.getPageView("Dashboards", "Index.cshtml"));
    }
}
