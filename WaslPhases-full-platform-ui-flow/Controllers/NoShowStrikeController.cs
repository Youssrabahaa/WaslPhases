using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

namespace phase_1.Controllers
{
    public class NoShowStrikeController : Controller
    {
        private readonly INoShowStrikeService _noShowStrikeService;

        public NoShowStrikeController(INoShowStrikeService noShowStrikeService)
        {
            _noShowStrikeService = noShowStrikeService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateNoShowStrikeDTO dto)
        {
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