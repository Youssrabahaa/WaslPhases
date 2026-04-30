using Microsoft.AspNetCore.Mvc;
using phase_1.Services;

namespace phase_1.Controllers;

public class SessionController : Controller
{
    private readonly IPatientPortalService _patientPortalService;

    public SessionController(IPatientPortalService patientPortalService)
    {
        _patientPortalService = patientPortalService;
    }

    public IActionResult SessionList()
    {
        return View(_patientPortalService.GetSessions());
    }

    public IActionResult SessionDetails(int id)
    {
        var viewModel = _patientPortalService.GetSessionDetails(id);
        return viewModel is null ? NotFound() : View(viewModel);
    }
}
