using Microsoft.AspNetCore.Mvc;
using phase_1.Services;

namespace phase_1.Controllers;

public class ReviewController : Controller
{
    private readonly IPatientPortalService _patientPortalService;

    public ReviewController(IPatientPortalService patientPortalService)
    {
        _patientPortalService = patientPortalService;
    }

    public IActionResult AddReview(int? matchId)
    {
        var viewModel = _patientPortalService.GetAddReviewModel(matchId);
        return viewModel is null ? NotFound() : View(viewModel);
    }

    public IActionResult ViewReviews()
    {
        return View(_patientPortalService.GetReviews());
    }
}
