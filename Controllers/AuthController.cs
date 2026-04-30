using Microsoft.AspNetCore.Mvc;

namespace phase_1.Controllers;

public class AuthController : Controller
{
    private const string PatientRole = "patient";
    private const string StudentRole = "student";

    public IActionResult Login(string? role)
    {
        PopulateRoleViewData(role);
        return View();
    }

    public IActionResult Register(string? role)
    {
        PopulateRoleViewData(role);
        return View();
    }

    public IActionResult VerifyOTP(string? role)
    {
        PopulateRoleViewData(role);
        return View();
    }

    public IActionResult Logout(string? role)
    {
        return RedirectToAction(nameof(Login), new { role = NormalizeRole(role) });
    }

    public IActionResult ForgotPassword(string? role)
    {
        PopulateRoleViewData(role);
        return View();
    }

    public IActionResult ResetPassword(string? role)
    {
        PopulateRoleViewData(role);
        return View();
    }

    public IActionResult SelectRole()
    {
        return View();
    }

    public IActionResult EnterPortal(string? role)
    {
        var normalizedRole = NormalizeRole(role);

        return normalizedRole == StudentRole
            ? RedirectToAction("Index", "Home")
            : RedirectToAction("Profile", "Patients");
    }

    private void PopulateRoleViewData(string? role)
    {
        var normalizedRole = NormalizeRole(role);
        var isPatient = normalizedRole == PatientRole;

        ViewData["SelectedRole"] = normalizedRole;
        ViewData["SelectedRoleTitle"] = isPatient ? "مريض" : "طالب طب أسنان";
        ViewData["SelectedPortalTitle"] = isPatient ? "بوابة المريض" : "بوابة الطالب";
        ViewData["DashboardController"] = isPatient ? "Patients" : "Home";
        ViewData["DashboardAction"] = isPatient ? "Profile" : "Index";
    }

    private static string NormalizeRole(string? role)
    {
        return string.Equals(role, StudentRole, StringComparison.OrdinalIgnoreCase)
            ? StudentRole
            : PatientRole;
    }
}
