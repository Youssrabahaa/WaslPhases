using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

namespace phase_1.Controllers
{
    public class AdminController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IReviewService _reviewService;

        public AdminController(IReportService reportService, IReviewService reviewService)
        {
            _reportService = reportService;
            _reviewService = reviewService;
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetInt32("UserRole");
            return role == 3;
        }

        public async Task<IActionResult> Dashboard()
        {
            if (!IsAdmin()) return Forbid();
            var reports = await _reportService.GetAllAsync();
            return View(reports);
        }

        public async Task<IActionResult> Reports(int? status)
        {
            if (!IsAdmin()) return Forbid();

            var reports = status.HasValue
                ? await _reportService.GetByStatusAsync(status.Value)
                : await _reportService.GetAllAsync();

            return View(reports);
        }

        public async Task<IActionResult> ReportDetails(int id)
        {
            if (!IsAdmin()) return Forbid();
            var report = await _reportService.GetByIdAsync(id);
            return View(report);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResolveReport(int id, ResolveReportDTO dto)
        {
            if (!IsAdmin()) return Forbid();

            try
            {
                await _reportService.ResolveReportAsync(id, dto);
                TempData["Success"] = "تم تحديث حالة البلاغ.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction(nameof(ReportDetails), new { id });
        }
    }
}