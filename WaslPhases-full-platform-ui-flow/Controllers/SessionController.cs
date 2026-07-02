using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

// ✅ إصلاح: كان phase_1.BLL.Controllers — namespace غلط
namespace phase_1.Controllers;

public class SessionController : Controller
{
    private readonly ISessionService _sessionService;

    public SessionController(ISessionService sessionService)
    {
        _sessionService = sessionService;
    }

    public async Task<IActionResult> SessionList(int matchId)
    {
        // ✅ إصلاح: لو matchId = 0 (من الـ Navbar) نحاول نجيبه من أحدث match
        if (matchId <= 0)
        {
            TempData["ErrorMessage"] = "يرجى فتح الجلسات من تفاصيل المطابقة.";
            return RedirectToAction("Index", "Match");
        }

        var sessions = await _sessionService.GetByMatchAsync(matchId);
        ViewBag.MatchId = matchId;
        return View(sessions);
    }

    public async Task<IActionResult> SessionDetails(int id)
    {
        var session = await _sessionService.GetDetailsAsync(id);

        if (session == null)
            return NotFound();

        return View(session);
    }

    public IActionResult AddSession(int matchId)
    {
        if (matchId <= 0)
            return RedirectToAction("Index", "Match");

        ViewBag.MatchId = matchId;
        return View(new CreateSessionDTO { MatchId = matchId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSession(CreateSessionDTO dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.MatchId = dto.MatchId;
            return View(dto);
        }

        await _sessionService.CreateAsync(dto);

        TempData["SuccessMessage"] = "تم إضافة الجلسة بنجاح.";
        return RedirectToAction(nameof(SessionList), new { matchId = dto.MatchId });
    }

    public async Task<IActionResult> EditSession(int id)
    {
        var session = await _sessionService.GetDetailsAsync(id);

        if (session == null)
            return NotFound();

        return View(new UpdateSessionDTO
        {
            Id = session.Id,
            StartAt = session.StartAt,
            EndAt = session.EndAt,
            LocationText = session.LocationText,
            ClinicRoom = session.ClinicRoom
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditSession(UpdateSessionDTO dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        var result = await _sessionService.UpdateAsync(dto);

        if (!result)
        {
            TempData["ErrorMessage"] = "لا يمكن تعديل الجلسة. يجب أن تكون في حالة مجدولة.";
            return View(dto);
        }

        TempData["SuccessMessage"] = "تم تعديل الجلسة بنجاح.";
        return RedirectToAction(nameof(SessionDetails), new { id = dto.Id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Start(int id)
    {
        var result = await _sessionService.StartAsync(id);

        if (!result)
            TempData["ErrorMessage"] = "لا يمكن بدء الجلسة.";
        else
            TempData["SuccessMessage"] = "تم بدء الجلسة.";

        return RedirectToAction(nameof(SessionDetails), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Finish(int id)
    {
        var result = await _sessionService.FinishAsync(id);

        if (!result)
            TempData["ErrorMessage"] = "لا يمكن إنهاء الجلسة.";
        else
            TempData["SuccessMessage"] = "تم إنهاء الجلسة بنجاح.";

        return RedirectToAction(nameof(SessionDetails), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id, int matchId)
    {
        var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
        var result = await _sessionService.CancelAsync(id, userId, "");

        if (!result)
            TempData["ErrorMessage"] = "لا يمكن إلغاء الجلسة.";
        else
            TempData["SuccessMessage"] = "تم إلغاء الجلسة.";

        return RedirectToAction(nameof(SessionList), new { matchId });
    }
}