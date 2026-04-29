using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class MatchController : Controller
{
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AcceptOffer(int id)
    {
        TempData["StatusMessage"] = "تم قبول العرض وتحويله إلى Match.";
        return RedirectToAction(nameof(MatchDetails), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult RejectOffer(int id)
    {
        TempData["StatusMessage"] = "تم رفض العرض.";
        return RedirectToAction("MyOffers", "Offer");
    }

    public IActionResult MatchDetails(int id = 1)
    {
        ViewData["MatchId"] = id;
        return View();
    }
}
