using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

namespace phase_1.Controllers
{
    public class NoShowStrikeController : Controller
    {
        private readonly INoShowStrikeService _noShowStrikeService;
        private readonly ISessionService _sessionService;

        public NoShowStrikeController(INoShowStrikeService noShowStrikeService, ISessionService sessionService)
        {
            _noShowStrikeService = noShowStrikeService;
            _sessionService = sessionService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateNoShowStrikeDTO dto)
        {
            var session = await _sessionService.GetDetailsAsync(dto.SessionId);
            var currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (session == null)
                return NotFound();

            if (session.PatientUserId != currentUserId && session.StudentUserId != currentUserId)
                return Forbid();

            dto.ReportedByUserId = currentUserId;
            dto.AbsentUserId = currentUserId == session.PatientUserId
                ? session.StudentUserId
                : session.PatientUserId;

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "بيانات غير صحيحة.";
                return RedirectToAction("SessionDetails", "Session", new { id = dto.SessionId });
            }

            var result = await _noShowStrikeService.CreateStrikeAsync(dto);

            if (result)
                TempData["SuccessMessage"] = "تم تسجيل عدم الحضور.";
            else
                TempData["ErrorMessage"] = "لا يمكن تسجيل عدم الحضور. ربما تم تسجيله مسبقًا.";

            return RedirectToAction("SessionDetails", "Session", new { id = dto.SessionId });
        }
    }
}
