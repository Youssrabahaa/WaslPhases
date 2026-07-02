using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

namespace phase_1.Controllers;

public class MatchController : Controller
{
    private readonly IMatchService _matchService;

    public MatchController(IMatchService matchService)
    {
        _matchService = matchService;
    }

    //  الـ Index بيقرأ UserId و Role من الـ Session ويوجه للقائمة الصح
    public async Task<IActionResult> Index()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var userRole = HttpContext.Session.GetInt32("UserRole");

        if (!userId.HasValue)
            return RedirectToAction("Login", "Auth");

        var matches = userRole == 1
            ? await _matchService.GetPatientMatchesAsync(userId.Value)
            : await _matchService.GetStudentMatchesAsync(userId.Value);

        return View(matches);
    }

    //  إصلاح: بيقرأ من Session بدل URL parameter
    public async Task<IActionResult> PatientMatches()
    {
        var patientId = HttpContext.Session.GetInt32("UserId") ?? 0;
        var matches = await _matchService.GetPatientMatchesAsync(patientId);
        return View("Index", matches);
    }

    //  إصلاح: بيقرأ من Session بدل URL parameter
    public async Task<IActionResult> StudentMatches()
    {
        var studentId = HttpContext.Session.GetInt32("UserId") ?? 0;
        var matches = await _matchService.GetStudentMatchesAsync(studentId);
        return View("Index", matches);
    }

    public async Task<IActionResult> MatchDetails(int id)
    {
        var match = await _matchService.GetMatchByIdAsync(id);

        if (match == null)
            return NotFound();

        return View(match);
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
        //  إصلاح: PatientMatches لا تحتاج parameter — بتقرأ من Session
        return RedirectToAction(nameof(PatientMatches));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(int id)
    {
        var result = await _matchService.CompleteMatchAsync(id);

        if (!result)
        {
            TempData["Error"] = "لا يمكن إتمام هذه المطابقة. تأكد أن جميع الجلسات مكتملة.";
            return RedirectToAction(nameof(MatchDetails), new { id });
        }

        TempData["Success"] = "تم إتمام المطابقة بنجاح.";
        return RedirectToAction(nameof(MatchDetails), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id, CancelMatchDto dto)
    {
        //  بيقرأ UserId من Session — أكثر أمانًا من الـ form
        var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
        var result = await _matchService.CancelMatchAsync(id, userId);

        if (!result)
        {
            TempData["Error"] = "لا يمكن إلغاء هذه المطابقة. العلاج بدأ بالفعل.";
            return RedirectToAction(nameof(MatchDetails), new { id });
        }

        TempData["Success"] = "تم إلغاء المطابقة.";
        return RedirectToAction(nameof(MatchDetails), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelExpired()
    {
        await _matchService.CancelExpiredMatchesAsync();
        TempData["Success"] = "تم فحص المطابقات المنتهية.";
        return RedirectToAction(nameof(Index));
    }
}