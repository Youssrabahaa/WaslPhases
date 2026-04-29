using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class OfferController : Controller
{
    public IActionResult BrowseCases()
    {
        return View();
    }

    [HttpGet]
    public IActionResult CreateOffer(int? caseId)
    {
        ViewData["CaseId"] = caseId ?? 1;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CreateOffer(int caseId, decimal? proposedPrice, int? estimatedSessionsCount)
    {
        if (proposedPrice is null || estimatedSessionsCount is null)
        {
            ViewData["CaseId"] = caseId;
            ModelState.AddModelError(string.Empty, "السعر وعدد الجلسات المتوقعان مطلوبان.");
            return View();
        }

        TempData["StatusMessage"] = "تم إرسال العرض للمريض.";
        return RedirectToAction(nameof(MyOffers));
    }

    public IActionResult MyOffers()
    {
        return View();
    }

    public IActionResult OfferDetails(int id = 1)
    {
        ViewData["OfferId"] = id;
        return View();
    }
}
