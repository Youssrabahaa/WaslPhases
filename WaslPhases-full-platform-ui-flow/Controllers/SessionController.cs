using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class SessionController : Controller
{
    public IActionResult SessionList()
    {
        return View();
    }

    public IActionResult SessionDetails()
    {
        return View();
    }
    public IActionResult AddSession()
    {
        return View();
    }

    // New Action for editing a session
    public IActionResult EditSession()
    {
        return View();
    }
}
