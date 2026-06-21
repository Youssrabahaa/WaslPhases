using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class MatchController : Controller
{
    public IActionResult MatchDetails()
    {
        return View();
    }
}
