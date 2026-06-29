using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using phase_1.Services;

namespace phase_1.Controllers
{
    // كل الـ Endpoints هنا محمية بالكامل - الأدمن فقط
    [Authorize(Roles = "Admin")]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public AdminController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // GET api/admin/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var stats = await _dashboardService.GetStatsAsync();
            return Ok(stats);
        }
    }
}

