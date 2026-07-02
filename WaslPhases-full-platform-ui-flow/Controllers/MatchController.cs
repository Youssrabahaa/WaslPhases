using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

namespace phase_1.Controllers;

public class MatchController : Controller
{
    private readonly IMatchService _matchService;
    private readonly IOfferService _offerService;

    public MatchController(IMatchService matchService, IOfferService offerService)
    {
        _matchService = matchService;
        _offerService = offerService;
    }

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

    public async Task<IActionResult> PatientMatches(int patientId)
    {
        if (HttpContext.Session.GetInt32("UserRole") != 1)
            return RedirectToAction("BrowseCases", "Offer", new { portal = "student" });

        if (patientId <= 0)
            patientId = HttpContext.Session.GetInt32("UserId") ?? 0;

        if (patientId <= 0)
            return RedirectToAction("Login", "Auth");

        var matches = await _matchService.GetPatientMatchesAsync(patientId);
        return View("Index", matches);
    }

    public async Task<IActionResult> StudentMatches(int studentId)
    {
        if (HttpContext.Session.GetInt32("UserRole") != 2)
            return RedirectToAction("MyCases", "Case", new { portal = "patient" });

        if (studentId <= 0)
            studentId = HttpContext.Session.GetInt32("UserId") ?? 0;

        if (studentId <= 0)
            return RedirectToAction("Login", "Auth");

        var matches = await _matchService.GetStudentMatchesAsync(studentId);
        return View("Index", matches);
    }

    public async Task<IActionResult> MatchDetails(int id)
    {
        var match = await _matchService.GetMatchByIdAsync(id);

        if (match == null)
            return NotFound();

        var currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
        if (match.PatientUserId != currentUserId && match.StudentUserId != currentUserId)
            return Forbid();

        return View(match);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptOffer(int offerId, int caseId)
    {
        if (HttpContext.Session.GetInt32("UserRole") != 1)
            return RedirectToAction("BrowseCases", "Offer", new { portal = "student" });

        var offer = await _offerService.GetOfferDetailsAsync(offerId);
        var currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
        if (offer == null || offer.PatientUserId != currentUserId)
            return Forbid();

        var result = await _matchService.AcceptOfferAsync(offerId);

        if (!result)
        {
            TempData["Error"] = "????? ???? ??? ?????.";
            return RedirectToAction("CaseDetails", "Case", new { id = caseId });
        }

        TempData["Success"] = "?? ???? ????? ?????.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(int id)
    {
        var match = await _matchService.GetMatchByIdAsync(id);
        var currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
        if (match == null)
            return NotFound();

        if (match.PatientUserId != currentUserId && match.StudentUserId != currentUserId)
            return Forbid();

        var result = await _matchService.CompleteMatchAsync(id);

        if (!result)
        {
            TempData["Error"] = "?? ???? ????? ??? ????????. ???? ?? ???? ??????? ??????.";
            return RedirectToAction(nameof(MatchDetails), new { id });
        }

        TempData["Success"] = "?? ????? ???????? ?????.";
        return RedirectToAction(nameof(MatchDetails), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id, CancelMatchDto dto)
    {
        var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
        var result = await _matchService.CancelMatchAsync(id, userId);
        //var result = await _matchService.CancelMatchAsync(id, dto.UserId);

        if (!result)
        {
            TempData["Error"] = "?? ???? ????? ??? ????????. ?????? ??? ??????.";
            return RedirectToAction(nameof(MatchDetails), new { id });
        }

        TempData["Success"] = "?? ????? ????????.";
        return RedirectToAction(nameof(MatchDetails), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelExpired()
    {
        return Forbid();
        /*
        await _matchService.CancelExpiredMatchesAsync();
        TempData["Success"] = "?? ??? ????????? ????????.";
        return RedirectToAction(nameof(Index));
        */
    }
}
