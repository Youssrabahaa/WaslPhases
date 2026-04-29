using Microsoft.AspNetCore.Mvc;
using phase_1.Models; 

namespace phase_1.Controllers
{
    public class AuthController : Controller
    {

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "يرجى إدخال البريد الإلكتروني وكلمة المرور";
                return View();
            }

            if (email == "admin@wasla.com" && password == "123456")
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "البريد الإلكتروني أو كلمة المرور غير صحيحة";
            return View();
        }
        
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("SelectRole");
            }

            return View(model);
        }

        public IActionResult SelectRole()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SelectRole(string role)
        {
            if (string.IsNullOrEmpty(role))
            {
                return View();
            }

            // هنخزن الدور المختار مؤقتاً عشان نكمل التسجيل
            TempData["SelectedRole"] = role;

            // بننقله للمحطة التالتة: التحقق من الكود (OTP)
            return RedirectToAction("VerifyOTP");
        }

        public IActionResult VerifyOTP()
        {
            return View();
        }

        [HttpPost]
        public IActionResult VerifyOTP(string otpCode)
        {
            // هنا مستقبلاً هنشيك على الكود من الداتا بيز
            if (otpCode == "123456") // تجربة مؤقتة
            {
                return RedirectToAction("Login");
            }

            ViewBag.Error = "كود التحقق غير صحيح";
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
           
            return RedirectToAction("VerifyOTP");
        }



    }
}