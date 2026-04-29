using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class StudentController : Controller
{
    public IActionResult Profile()
    {
        return View();
    }

    [HttpGet]
    public IActionResult EditProfile()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditProfile(string fullName, string studentCode)
    {
        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(studentCode))
        {
            ModelState.AddModelError(string.Empty, "الاسم وكود الطالب مطلوبان لحفظ الملف.");
            return View();
        }

        TempData["StatusMessage"] = "تم حفظ بيانات الطالب.";
        return RedirectToAction(nameof(Profile));
    }
}
