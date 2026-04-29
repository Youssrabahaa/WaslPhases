using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class ReviewController : Controller
{
    [HttpGet]
    public IActionResult AddReview()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddReview(int rating, string? comment)
    {
        if (rating is < 1 or > 5)
        {
            ModelState.AddModelError(nameof(rating), "اختر تقييم من 1 إلى 5.");
            return View();
        }

        TempData["StatusMessage"] = "تم حفظ التقييم.";
        return RedirectToAction(nameof(ViewReviews));
    }

    public IActionResult ViewReviews()
    {
        return View();
    }
}
