using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

namespace phase_1.Controllers;

public class OfferController : Controller
{
    private readonly IOfferService _offerService;
    private readonly IMatchService _matchService;
    private readonly ICaseService _caseService;

    public OfferController(IOfferService offerService, IMatchService matchService, ICaseService caseService)
    {
        _offerService = offerService;
        _matchService = matchService;
        _caseService = caseService;
    }

    [HttpGet]
    public IActionResult CreateOffer(int caseId)
    {
        if (!IsStudent())
            return RedirectToPatientHome();

        var studentId = HttpContext.Session.GetInt32("UserId") ?? 0;
        var model = new CreateOfferDTO { CaseId = caseId, StudentUserId = studentId };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateOffer(CreateOfferDTO model)
    {
        if (!IsStudent())
            return RedirectToPatientHome();

        model.StudentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;

        var caseDetails = await _caseService.GetByIdAsync(model.CaseId);
        if (caseDetails == null || caseDetails.Status != 1)
        {
            TempData["Error"] = "لا يمكن تقديم عرض على هذه الحالة.";
            return RedirectToAction(nameof(BrowseCases));
        }

        if (!ModelState.IsValid)
            return View(model);

        await _offerService.CreateOfferAsync(model);
        TempData["Success"] = "تم إرسال العرض بنجاح.";
        return RedirectToAction(nameof(MyOffers));
    }

    public async Task<IActionResult> MyOffers()
    {
        if (!IsStudent())
            return RedirectToPatientHome();

        var studentId = HttpContext.Session.GetInt32("UserId") ?? 0;
        var offers = await _offerService.GetOffersByStudentAsync(studentId);
        return View("MyOffersDynamic", offers);
    }

    public async Task<IActionResult> BrowseCases()
    {
        if (!IsStudent())
            return RedirectToPatientHome();

        var cases = await _caseService.GetOpenCasesAsync();
        return View("BrowseCasesDynamic", cases);
    }

    public async Task<IActionResult> OfferDetails(int id)
    {
        var offer = await _offerService.GetOfferDetailsAsync(id);
        if (offer == null) return NotFound();

        var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
        if (userId != offer.StudentUserId && userId != offer.PatientUserId)
            return Forbid();

        return View("OfferDetailsDynamic", offer);
    }

    public async Task<IActionResult> CaseOffers(int caseId)
    {
        if (!IsPatient())
            return RedirectToStudentHome();

        var caseDetails = await _caseService.GetByIdAsync(caseId);
        var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
        if (caseDetails == null)
            return NotFound();

        if (caseDetails.PatientUserId != userId)
            return Forbid();

        ViewBag.CaseId = caseId;
        ViewBag.CaseTitle = caseDetails.Title;
        var offers = await _offerService.GetOffersForCaseAsync(caseId);
        return View("CaseOffersDynamic", offers.ToList());
    }

    // ✅ AcceptOffer يمر عبر MatchService — هو اللي بينشئ Match + Conversation + يرفض باقي العروض
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptOffer(int offerId, int caseId)
    {
        if (!IsPatient())
            return RedirectToStudentHome();

        var offer = await _offerService.GetOfferDetailsAsync(offerId);
        var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
        if (offer == null || offer.PatientUserId != userId)
            return Forbid();

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
        if (!IsPatient())
            return RedirectToStudentHome();

        var offer = await _offerService.GetOfferDetailsAsync(id);
        var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
        if (offer == null || offer.PatientUserId != userId)
            return Forbid();

        await _offerService.RejectOfferAsync(id);
        TempData["Success"] = "تم رفض العرض.";
        return RedirectToAction("OfferDetails", new { id });
    }

    private bool IsPatient() => HttpContext.Session.GetInt32("UserRole") == 1;
    private bool IsStudent() => HttpContext.Session.GetInt32("UserRole") == 2;
    private IActionResult RedirectToPatientHome() => RedirectToAction("MyCases", "Case", new { portal = "patient" });
    private IActionResult RedirectToStudentHome() => RedirectToAction("BrowseCases", "Offer", new { portal = "student" });
}
