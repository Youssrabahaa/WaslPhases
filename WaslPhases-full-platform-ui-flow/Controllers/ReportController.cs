using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

namespace phase_1.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly ISessionService _sessionService;

        public ReportController(IReportService reportService, ISessionService sessionService)
        {
            _reportService = reportService;
            _sessionService = sessionService;
        }

        public async Task<IActionResult> AddReport(int sessionId)
        {
            var session = await _sessionService.GetDetailsAsync(sessionId);
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (session == null)
                return NotFound();

            if (session.PatientUserId != userId && session.StudentUserId != userId)
                return Forbid();

            ViewBag.SessionId = sessionId;
            ViewBag.ReportedUserId = userId == session.PatientUserId ? session.StudentUserId : session.PatientUserId;
            ViewBag.ReportedUserName = userId == session.PatientUserId ? session.StudentName : session.PatientName;
            return View("AddReportDynamic", new CreateReportDTO { SessionId = sessionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReport(CreateReportDTO dto)
        {
            var reporterUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var session = await _sessionService.GetDetailsAsync(dto.SessionId);
            if (session == null)
                return NotFound();

            if (session.PatientUserId != reporterUserId && session.StudentUserId != reporterUserId)
                return Forbid();

            dto.ReportedUserId = reporterUserId == session.PatientUserId
                ? session.StudentUserId
                : session.PatientUserId;

            if (!ModelState.IsValid)
            {
                ViewBag.SessionId = dto.SessionId;
                ViewBag.ReportedUserId = dto.ReportedUserId;
                ViewBag.ReportedUserName = reporterUserId == session.PatientUserId ? session.StudentName : session.PatientName;
                return View("AddReportDynamic", dto);
            }

            try
            {
                await _reportService.CreateReportAsync(reporterUserId, dto);
                TempData["Success"] = "?? ????? ?????? ?????.";
                return RedirectToAction("SessionDetails", "Session", new { id = dto.SessionId });
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("SessionDetails", "Session", new { id = dto.SessionId });
            }
        }

        public async Task<IActionResult> MyReports()
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var reports = await _reportService.GetMyReportsAsync(userId);
            return View(reports);
        }
    }
}
