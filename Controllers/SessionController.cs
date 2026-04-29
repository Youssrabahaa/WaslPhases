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
}
