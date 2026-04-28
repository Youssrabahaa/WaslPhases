using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class PatientsController : Controller
{
    public IActionResult Profile()
    {
        return View();
    }

    public IActionResult PatientProfile()
    {
        return View();
    }
}
