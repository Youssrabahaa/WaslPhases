using Microsoft.AspNetCore.Mvc;
using phase_1.Services;

namespace phase_1.Controllers;

public class MatchController : Controller
{
    private readonly IPatientPortalService _patientPortalService;

    public MatchController(IPatientPortalService patientPortalService)
    {
        _patientPortalService = patientPortalService;
    }

    public IActionResult MatchDetails(int id = 401)
    {
        var viewModel = _patientPortalService.GetMatchDetails(id);
        return viewModel is null ? NotFound() : View(viewModel);
    }
}
