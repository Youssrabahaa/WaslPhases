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
        var matches = await _matchService.GetPatientMatchesAsync(patientId);
        return View(matches);
    }

    public async Task<IActionResult> StudentMatches(int studentId)
    {
        var matches = await _matchService.GetStudentMatchesAsync(studentId);
        return View(matches);
    }

    // ? ?? ??? action ?????? ???? ??? View ?????? ????
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
            TempData["Error"] = "????? ???? ??? ?????.";
            return RedirectToAction("CaseDetails", "Case", new { id = caseId });
        }

        TempData["Success"] = "?? ???? ????? ?????.";
        return RedirectToAction(nameof(PatientMatches));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Complete(int id)
    {
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
        var result = await _matchService.CancelMatchAsync(id, dto.UserId);

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
        await _matchService.CancelExpiredMatchesAsync();
        TempData["Success"] = "?? ??? ????????? ????????.";
        return RedirectToAction(nameof(PatientMatches));
    }
}
