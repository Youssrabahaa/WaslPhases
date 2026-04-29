using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class SessionController : Controller
{
    public IActionResult SessionList()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult StartSession(int id)
    {
        TempData["StatusMessage"] = "بدأت الجلسة التجريبية.";
        return RedirectToAction(nameof(SessionDetails), new { id });
    }

    public IActionResult SessionDetails(int id = 1)
    {
        ViewData["SessionId"] = id;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EndSession(int id)
    {
        TempData["StatusMessage"] = "تم إنهاء الجلسة وحفظ الحالة.";
        return RedirectToAction(nameof(SessionDetails), new { id });
    }
}
