using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class PatientsController : Controller
{
    public IActionResult Profile()
    {
        return View("~/Views/Patient/Profile.cshtml");
    }

    public IActionResult EditProfile()
    {
        return View("~/Views/Patient/EditProfile.cshtml");
    }

    public IActionResult PatientProfile()
    {
        return View("~/Views/Patient/Profile.cshtml");
    }
}
