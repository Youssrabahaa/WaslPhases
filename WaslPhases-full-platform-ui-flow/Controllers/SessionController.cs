using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

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
        if (matchId <= 0)
        {
            TempData["ErrorMessage"] = "معرف المطابقة غير صحيح.";
            return View(new List<SessionDTO>());
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
        ViewBag.MatchId = matchId;
        return View();
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

        var dto = new UpdateSessionDTO
        {
            Id = session.Id,
            StartAt = session.StartAt,
            EndAt = session.EndAt,
            LocationText = session.LocationText,
            ClinicRoom = session.ClinicRoom
        };

        return View(dto);
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
        var result = await _sessionService.CancelAsync(id, 0, "");

        if (!result)
            TempData["ErrorMessage"] = "لا يمكن إلغاء الجلسة.";
        else
            TempData["SuccessMessage"] = "تم إلغاء الجلسة.";

        return RedirectToAction(nameof(SessionList), new { matchId });
    }
}