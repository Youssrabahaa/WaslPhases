using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class ReportController : Controller
{
    public IActionResult AddReport()
    {
        return View();
    }

    public IActionResult ViewReport()
    {
        return View();
    }
}
