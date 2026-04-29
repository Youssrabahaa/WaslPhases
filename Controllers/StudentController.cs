using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class StudentController : Controller
{
    public IActionResult Profile()
    {
        return View();
    }

    public IActionResult EditProfile()
    {
        return View();
    }
}
