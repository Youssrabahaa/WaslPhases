using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.Services;

namespace phase_1.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public async Task<IActionResult> GetNotifications()
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var notifications = await _notificationService.GetForUserAsync(userId);
            return View("List", notifications);
        }

        [HttpGet]
        public async Task<IActionResult> UnreadCount()
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Json(new { count });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            await _notificationService.MarkAsReadAsync(id, userId);
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            await _notificationService.MarkAllAsReadAsync(userId);
            TempData["Success"] = "تم تعليم جميع الإشعارات كمقروءة.";
            return RedirectToAction(nameof(GetNotifications));
        }
    }
}