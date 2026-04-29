using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class ReviewController : Controller
{
    public IActionResult AddReview()
    {
        return View();
    }

    public IActionResult ViewReviews()
    {
        return View();
    }
}
