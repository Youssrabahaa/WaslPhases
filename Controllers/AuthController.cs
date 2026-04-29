using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class AuthController : Controller
{
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult VerifyOTP()
    {
        return View();
    }

    public IActionResult Logout()
    {
        return RedirectToAction(nameof(Login));
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    public IActionResult ResetPassword()
    {
        return View();
    }

    public IActionResult SelectRole()
    {
        return View();
    }
}
