using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class AuthController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            ModelState.AddModelError(nameof(phone), "رقم الهاتف مطلوب لتسجيل الدخول.");
            return View();
        }

        TempData["StatusMessage"] = "تم إرسال كود التحقق التجريبي إلى رقم الهاتف.";
        return RedirectToAction(nameof(VerifyOTP), new { phone });
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(string fullName, string phone, string role)
    {
        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(role))
        {
            ModelState.AddModelError(string.Empty, "اكمل الاسم ورقم الهاتف ونوع الحساب قبل المتابعة.");
            return View();
        }

        TempData["StatusMessage"] = "تم إنشاء الحساب مبدئيا. أكمل خطوة التحقق.";
        return RedirectToAction(nameof(VerifyOTP), new { phone });
    }

    [HttpGet]
    public IActionResult VerifyOTP(string? phone)
    {
        ViewData["Phone"] = phone ?? "01000000000";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult VerifyOTP(string phone, string code)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length < 4)
        {
            ViewData["Phone"] = phone;
            ModelState.AddModelError(nameof(code), "ادخل كود تحقق صحيح.");
            return View();
        }

        TempData["StatusMessage"] = "تم التحقق بنجاح.";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult Logout()
    {
        TempData["StatusMessage"] = "تم تسجيل الخروج بنجاح.";
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ForgotPassword(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
        {
            ModelState.AddModelError(nameof(phone), "رقم الهاتف مطلوب لاستعادة كلمة المرور.");
            return View();
        }

        TempData["StatusMessage"] = "تم إرسال كود إعادة التعيين.";
        return RedirectToAction(nameof(ResetPassword), new { phone });
    }

    [HttpGet]
    public IActionResult ResetPassword(string? phone)
    {
        ViewData["Phone"] = phone ?? "01000000000";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ResetPassword(string phone, string code, string password)
    {
        if (string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(password))
        {
            ViewData["Phone"] = phone;
            ModelState.AddModelError(string.Empty, "الكود وكلمة المرور الجديدة مطلوبان.");
            return View();
        }

        TempData["StatusMessage"] = "تم تحديث كلمة المرور.";
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    public IActionResult SelectRole()
    {
        return View();
    }
}
