using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class CaseController : Controller
{
    public IActionResult CreateCase()
    {
        return View();
    }

    public IActionResult MyCases()
    {
        return View();
    }

    public IActionResult CaseDetails(int id)
    {
        return View();
    }

    public IActionResult EditCase(int id)
    {
        return View();
    }

    public IActionResult DeleteCase(int id)
    {
        return View();
    }
}
