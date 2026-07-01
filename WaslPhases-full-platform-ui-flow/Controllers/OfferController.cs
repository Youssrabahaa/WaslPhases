using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

namespace phase_1.Controllers;

public class OfferController : Controller
{
    private readonly IOfferService _offerService;
    private readonly IMatchService _matchService;

    public OfferController(IOfferService offerService, IMatchService matchService)
    {
        _offerService = offerService;
        _matchService = matchService;
    }

    [HttpGet]
    public IActionResult CreateOffer(int caseId)
    {
        var studentId = HttpContext.Session.GetInt32("UserId") ?? 0;
        var model = new CreateOfferDTO { CaseId = caseId, StudentUserId = studentId };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOffer(CreateOfferDTO model)
    {
        model.StudentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;

        if (!ModelState.IsValid)
            return View(model);

        await _offerService.CreateOfferAsync(model);
        TempData["Success"] = "تم إرسال العرض بنجاح.";
        return RedirectToAction(nameof(MyOffers));
    }

    public async Task<IActionResult> MyOffers()
    {
        var studentId = HttpContext.Session.GetInt32("UserId") ?? 0;
        var offers = await _offerService.GetOffersByStudentAsync(studentId);
        return View(offers);
    }

    public async Task<IActionResult> BrowseCases()
    {
        // يُستبدل بـ CaseService لاحقًا
        return View();
    }

    public async Task<IActionResult> OfferDetails(int id)
    {
        var offer = await _offerService.GetOfferDetailsAsync(id);
        if (offer == null) return NotFound();
        return View(offer);
    }

    // ✅ AcceptOffer يمر عبر MatchService — هو اللي بينشئ Match + Conversation + يرفض باقي العروض
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptOffer(int offerId, int caseId)
    {
        var result = await _matchService.AcceptOfferAsync(offerId);

        if (!result)
        {
            TempData["Error"] = "تعذّر قبول هذا العرض.";
            return RedirectToAction("OfferDetails", new { id = offerId });
        }

        TempData["Success"] = "تم قبول العرض وإنشاء المطابقة.";
        return RedirectToAction("PatientMatches", "Match");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectOffer(int id)
    {
        await _offerService.RejectOfferAsync(id);
        TempData["Success"] = "تم رفض العرض.";
        return RedirectToAction("OfferDetails", new { id });
    }
}