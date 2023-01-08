using Microsoft.AspNetCore.Mvc;
using ApiManager._keenthemes.libs;

namespace ApiManager.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<DashboardsController> _logger;
    private readonly IKTTheme _theme;

    public AuthController(ILogger<DashboardsController> logger, IKTTheme theme)
    {
        _logger = logger;
        _theme = theme;
    }

    public IActionResult signIn()
    {
        return View(_theme.getPageView("Auth", "SignIn.cshtml"));
    }

    public IActionResult signUp()
    {
        return View(_theme.getPageView("Auth", "SignUp.cshtml"));
    }

    public IActionResult resetPassword()
    {
        return View(_theme.getPageView("Auth", "ResetPassword.cshtml"));
    }

    public IActionResult newPassword()
    {
        return View(_theme.getPageView("Auth", "NewPassword.cshtml"));
    }    
}
