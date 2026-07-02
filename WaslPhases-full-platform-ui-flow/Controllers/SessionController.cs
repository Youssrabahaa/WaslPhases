using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

namespace phase_1.BLL.Controllers;

public class SessionController : Controller
{
    private readonly ISessionService _sessionService;
    private readonly IMatchService _matchService;

    public SessionController(ISessionService sessionService, IMatchService matchService)
    {
        _sessionService = sessionService;
        _matchService = matchService;
    }

    public async Task<IActionResult> SessionList(int matchId)
    {
        if (matchId <= 0)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userRole = HttpContext.Session.GetInt32("UserRole");

            if (!userId.HasValue)
                return RedirectToAction("Login", "Auth");

            var matches = userRole == 1
                ? await _matchService.GetPatientMatchesAsync(userId.Value)
                : await _matchService.GetStudentMatchesAsync(userId.Value);

            var allSessions = new List<SessionDTO>();
            foreach (var userMatch in matches)
                allSessions.AddRange(await _sessionService.GetByMatchAsync(userMatch.MatchId));

            ViewBag.MatchId = 0;
            return View(allSessions.OrderBy(x => x.StartAt).ToList());
        }

        var sessions = await _sessionService.GetByMatchAsync(matchId);
        var match = await _matchService.GetMatchByIdAsync(matchId);
        var currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
        if (match == null)
            return NotFound();

        if (match.PatientUserId != currentUserId && match.StudentUserId != currentUserId)
            return Forbid();

        ViewBag.MatchId = matchId;
        return View(sessions);
    }

    public async Task<IActionResult> SessionDetails(int id)
    {
        var session = await _sessionService.GetDetailsAsync(id);

        if (session == null)
            return NotFound();

        var currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
        if (session.PatientUserId != currentUserId && session.StudentUserId != currentUserId)
            return Forbid();

        return View(session);
    }

    public async Task<IActionResult> AddSession(int matchId)
    {
        if (HttpContext.Session.GetInt32("UserRole") != 1)
            return RedirectToAction(nameof(SessionList));

        if (matchId <= 0)
        {
            TempData["ErrorMessage"] = "افتح المطابقة أولا قبل إضافة جلسة.";
            return RedirectToAction(nameof(SessionList));
        }

        var match = await _matchService.GetMatchByIdAsync(matchId);
        var currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
        if (match == null)
            return NotFound();

        if (match.PatientUserId != currentUserId)
            return Forbid();

        ViewBag.MatchId = matchId;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddSession(CreateSessionDTO dto)
    {
        if (HttpContext.Session.GetInt32("UserRole") != 1)
            return RedirectToAction(nameof(SessionList));

        var match = await _matchService.GetMatchByIdAsync(dto.MatchId);
        var currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
        if (match == null)
            return NotFound();

        if (match.PatientUserId != currentUserId)
            return Forbid();

        if (!ModelState.IsValid)
        {
            ViewBag.MatchId = dto.MatchId;
            return View(dto);
        }

        var created = await _sessionService.CreateAsync(dto);
        if (!created)
        {
            ViewBag.MatchId = dto.MatchId;
            TempData["ErrorMessage"] = "لم يتم حفظ الجلسة. تأكد من اختيار مطابقة صحيحة وأن موعد النهاية بعد البداية.";
            return View(dto);
        }

        TempData["SuccessMessage"] = "تم إضافة الجلسة بنجاح.";
        return RedirectToAction(nameof(SessionList), new { matchId = dto.MatchId, portal = "patient" });
    }

    public async Task<IActionResult> EditSession(int id)
    {
        var session = await _sessionService.GetDetailsAsync(id);

        if (session == null)
            return NotFound();

        var currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
        if (session.PatientUserId != currentUserId)
            return Forbid();

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

        var session = await _sessionService.GetDetailsAsync(dto.Id);
        if (session == null)
            return NotFound();

        var currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
        if (session.PatientUserId != currentUserId)
            return Forbid();

        var result = await _sessionService.UpdateAsync(dto);

        if (!result)
        {
            TempData["ErrorMessage"] = "لا يمكن تعديل الجلسة. يجب أن تكون في حالة مجدولة.";
            return View(dto);
        }

        TempData["SuccessMessage"] = "تم تعديل الجلسة بنجاح.";
        return RedirectToAction(nameof(SessionDetails), new { id = dto.Id, portal = "patient" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Start(int id)
    {
        var session = await _sessionService.GetDetailsAsync(id);
        if (session == null)
            return NotFound();

        var currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
        if (currentUserId != session.StudentUserId)
        {
            TempData["ErrorMessage"] = "بدء الجلسة متاح للطالب المرتبط بالمطابقة فقط.";
            return RedirectToAction(nameof(SessionDetails), new { id, portal = "student" });
        }

        var result = await _sessionService.StartAsync(id);

        if (!result)
        {
            TempData["ErrorMessage"] = "لا يمكن بدء الجلسة.";
            return RedirectToAction(nameof(SessionDetails), new { id, portal = "student" });
        }

        TempData["SuccessMessage"] = "تم بدء الجلسة. يمكنك متابعة التواصل في الشات.";
        return RedirectToAction("Chat", "Conversation", new { matchId = session.MatchId, portal = "student" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Finish(int id)
    {
        var session = await _sessionService.GetDetailsAsync(id);
        if (session == null)
            return NotFound();

        var currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
        if (currentUserId != session.StudentUserId)
        {
            TempData["ErrorMessage"] = "إنهاء الجلسة متاح للطالب المرتبط بالمطابقة فقط.";
            return RedirectToAction(nameof(SessionDetails), new { id, portal = "student" });
        }

        var result = await _sessionService.FinishAsync(id);

        if (!result)
            TempData["ErrorMessage"] = "لا يمكن إنهاء الجلسة.";
        else
            TempData["SuccessMessage"] = "تم إنهاء الجلسة بنجاح.";

        return RedirectToAction(nameof(SessionDetails), new { id, portal = "student" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id, int matchId)
    {
        var session = await _sessionService.GetDetailsAsync(id);
        if (session == null)
            return NotFound();

        var currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
        if (session.PatientUserId != currentUserId)
            return Forbid();

        var result = await _sessionService.CancelAsync(id, 0, "");

        if (!result)
            TempData["ErrorMessage"] = "لا يمكن إلغاء الجلسة.";
        else
            TempData["SuccessMessage"] = "تم إلغاء الجلسة.";

        return RedirectToAction(nameof(SessionList), new { matchId, portal = "patient" });
    }
}
