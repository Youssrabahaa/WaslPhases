using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class NotificationController : Controller
{
    public IActionResult GetNotifications()
    {
        return View("List");
    }
}
