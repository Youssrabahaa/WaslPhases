using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

namespace phase_1.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        public IActionResult AddReport(int sessionId)
        {
            ViewBag.SessionId = sessionId;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReport(CreateReportDTO dto)
        {
            var reporterUserId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (!ModelState.IsValid)
            {
                ViewBag.SessionId = dto.SessionId;
                return View(dto);
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
