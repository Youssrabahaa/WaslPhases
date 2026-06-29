using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using phase_1.DTOs;
using phase_1.Models;
using phase_1.Services;

namespace phase_1.Controllers
{
    [Authorize]
    [Route("api/reports")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        // POST api/reports
        // أي مستخدم (Patient/Student) يقدر يبلّغ عن جلسة هو طرف فيها
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReportDTO dto)
        {
            var userId = GetCurrentUserId();

            try
            {
                var result = await _reportService.CreateReportAsync(userId, dto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET api/reports/mine
        // المستخدم يشوف البلاغات اللي هو عملها بس
        [HttpGet("mine")]
        public async Task<IActionResult> GetMyReports()
        {
            var userId = GetCurrentUserId();
            var reports = await _reportService.GetMyReportsAsync(userId);
            return Ok(reports);
        }

        // ------------------------------
        // عمليات الأدمن فقط من هنا تحت
        // ------------------------------

        // GET api/reports
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ReportStatus? status)
        {
            var reports = status.HasValue
                ? await _reportService.GetByStatusAsync(status.Value)
                : await _reportService.GetAllAsync();

            return Ok(reports);
        }

        // GET api/reports/{id}
        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var report = await _reportService.GetByIdAsync(id);
                return Ok(report);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // PUT api/reports/{id}/resolve
        // الأدمن بس هو اللي يقدر يقفل/يتخذ إجراء على البلاغ
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/resolve")]
        public async Task<IActionResult> Resolve(int id, [FromBody] ResolveReportDTO dto)
        {
            var adminId = GetCurrentUserId();

            try
            {
                var result = await _reportService.ResolveReportAsync(adminId, id, dto);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("User not authenticated.");
        }
    }
}
