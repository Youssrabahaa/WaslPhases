using Microsoft.AspNetCore.Mvc;
using phase_1.Models; // تأكدي أن هذا هو اسم الـ Namespace الصحيح لموديلات مشروعك

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
            // 1. تشيك بسيط: هل الخانات فاضية؟
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "يرجى إدخال البريد الإلكتروني وكلمة المرور";
                return View();
            }

            // 2. تجربة وهمية (Dummy Login) لحد ما نربط الداتابيز
            if (email == "admin@wasla.com" && password == "123456")
            {
                // لو البيانات صح، بنوديه لصفحة الـ Home مثلاً
                return RedirectToAction("Index", "Home");
            }

            // 3. لو البيانات غلط، بنرجعه لنفس الصفحة ونطلع له رسالة
            ViewBag.Error = "البريد الإلكتروني أو كلمة المرور غير صحيحة";
            return View();
        }
        //// 1. صفحة تسجيل الدخول
        //public IActionResult Login()
        //{
        //    return View();

        //}

        //// 1. دالة عرض صفحة اللوجن (لما يفتح اللينك لأول مرة)
        //[HttpGet]


        //// 2. دالة استلام رقم الموبايل وإرسال الـ OTP
        //[HttpPost]
        //public IActionResult Login(string phoneNumber)
        //{
        //    if (string.IsNullOrEmpty(phoneNumber))
        //    {
        //        ModelState.AddModelError("", "Please enter your phone number");
        //        return View();
        //    }

        //    // هنا المفروض نتشيك في الداتابيز: هل الرقم ده موجود؟
        //    // var user = _context.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber);

        //    // مؤقتاً هنفترض إن الرقم موجود وهنبعت OTP
        //    // _otpService.Send(phoneNumber); 

        //    // بعد ما نبعت الكود، بنحوله لصفحة التأكيد (اللي فيها خانة الـ 6 أرقام)
        //    return RedirectToAction("VerifyOTP", new { phone = phoneNumber });
        //}

        // 2. صفحة إنشاء حساب جديد
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            // التأكد من أن البيانات مطابقة لشروط الـ Validation اللي حطيناها في الـ ViewModel
            if (ModelState.IsValid)
            {
                // هنا المفروض نكتب كود حفظ المستخدم في قاعدة البيانات
                // لكن دلوقتي هنعمل الـ Flow (التسجيل -> اختيار الدور)

                // التوجيه لصفحة اختيار الدور (طالب أم مريض)
                return RedirectToAction("SelectRole");
            }

            // لو البيانات فيها مشكلة، بنرجعه لنفس الصفحة مع عرض الأخطاء
            return View(model);
        }

        // 3. صفحة اختيار نوع المستخدم (طالب أم مريض)
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

        // 4. صفحة التحقق من كود الـ OTP
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

        // 5. صفحة طلب استعادة كلمة المرور
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            // هنا مستقبلاً هتبعتي إيميل حقيقي
            // دلوقتي ممكن نوديه لصفحة إدخال الـ OTP
            return RedirectToAction("VerifyOTP");
        }



    }
}