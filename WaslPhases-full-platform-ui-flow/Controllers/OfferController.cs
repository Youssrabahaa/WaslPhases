using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

namespace phase_1.Controllers;

public class OfferController : Controller
{
    private readonly IOfferService _offerService;
    private readonly IMatchService _matchService;
    private readonly ICaseService _caseService;

    public OfferController(
        IOfferService offerService,
        IMatchService matchService,
        ICaseService caseService)
    {
        _offerService = offerService;
        _matchService = matchService;
        _caseService = caseService;
    }

    public async Task<IActionResult> BrowseCases()
    {
        var cases = await _caseService.GetOpenCasesAsync();
        return View(cases);
    }

    [HttpGet]
    public async Task<IActionResult> CreateOffer(int caseId)
    {
        var studentId = HttpContext.Session.GetInt32("UserId") ?? 0;

        var existing = await _offerService.GetExistingOfferAsync(studentId, caseId);
        if (existing != null)
        {
            TempData["Error"] = "لقد قدمت عرضًا على هذه الحالة مسبقًا.";
            return RedirectToAction(nameof(OfferDetails), new { id = existing.Id });
        }

        return View(new CreateOfferDTO { CaseId = caseId, StudentUserId = studentId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOffer(CreateOfferDTO model)
    {
        model.StudentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await _offerService.CreateOfferAsync(model);
            TempData["Success"] = "تم إرسال العرض بنجاح.";
            return RedirectToAction(nameof(MyOffers));
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("CaseDetails", "Case", new { id = model.CaseId });
        }
    }

    public async Task<IActionResult> MyOffers()
    {
        var studentId = HttpContext.Session.GetInt32("UserId") ?? 0;
        var offers = await _offerService.GetOffersByStudentAsync(studentId);
        return View(offers);
    }

    public async Task<IActionResult> OfferDetails(int id)
    {
        var offer = await _offerService.GetOfferDetailsAsync(id);
        if (offer == null) return NotFound();
        return View(offer);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptOffer(int offerId, int caseId)
    {
        var result = await _matchService.AcceptOfferAsync(offerId);

        if (!result)
        {
            TempData["Error"] = "تعذّر قبول هذا العرض.";
            return RedirectToAction("CaseDetails", "Case", new { id = caseId });
        }

        TempData["Success"] = "تم قبول العرض وإنشاء المطابقة.";
        return RedirectToAction("PatientMatches", "Match");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectOffer(int id)
    {
        var offer = await _offerService.GetOfferDetailsAsync(id);
        await _offerService.RejectOfferAsync(id);
        TempData["Success"] = "تم رفض العرض.";
        return RedirectToAction("CaseDetails", "Case", new { id = offer?.CaseId ?? 0 });
    }
}