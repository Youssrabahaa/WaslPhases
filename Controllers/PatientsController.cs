using Microsoft.AspNetCore.Mvc;
using phase_1.Services;

namespace phase_1.Controllers;

public class PatientsController : Controller
{
    private readonly IPatientPortalService _patientPortalService;

    public PatientsController(IPatientPortalService patientPortalService)
    {
        _patientPortalService = patientPortalService;
    }

    public IActionResult Profile()
    {
        return View("~/Views/Patient/Profile.cshtml", _patientPortalService.GetDashboard());
    }

    public IActionResult EditProfile()
    {
        return View("~/Views/Patient/EditProfile.cshtml");
    }

    public IActionResult PatientProfile()
    {
        return View("~/Views/Patient/Profile.cshtml", _patientPortalService.GetDashboard());
    }
}
