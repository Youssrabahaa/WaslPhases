using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class ReportController : Controller
{
    [HttpGet]
    public IActionResult AddReport()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddReport(string reason)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            ModelState.AddModelError(nameof(reason), "سبب البلاغ مطلوب.");
            return View();
        }

        TempData["StatusMessage"] = "تم إرسال التقرير للمراجعة.";
        return RedirectToAction(nameof(ViewReport), new { id = 1 });
    }

    public IActionResult ViewReport(int id = 1)
    {
        ViewData["ReportId"] = id;
        return View();
    }
}
