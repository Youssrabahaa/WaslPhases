using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class NotificationController : Controller
{
    public IActionResult GetNotifications()
    {
        return View("List");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult MarkAsRead(int id)
    {
        TempData["StatusMessage"] = "تم تعليم الإشعار كمقروء.";
        return RedirectToAction(nameof(GetNotifications));
    }
}
