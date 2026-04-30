using Microsoft.AspNetCore.Mvc;
using phase_1.Services;

namespace phase_1.Controllers;

public class CaseController : Controller
{
    private readonly IPatientPortalService _patientPortalService;

    public CaseController(IPatientPortalService patientPortalService)
    {
        _patientPortalService = patientPortalService;
    }

    public IActionResult CreateCase()
    {
        return View();
    }

    public IActionResult MyCases()
    {
        return View(_patientPortalService.GetCases());
    }

    public IActionResult CaseDetails(int id)
    {
        var viewModel = _patientPortalService.GetCaseDetails(id);
        return viewModel is null ? NotFound() : View(viewModel);
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
