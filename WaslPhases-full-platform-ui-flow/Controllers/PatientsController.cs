using Microsoft.AspNetCore.Mvc;
using phase_1.DTOs;
using phase_1.Services;

namespace phase_1.Controllers;

public class PatientsController : Controller
{
    private readonly IAuthService _authService;

    public PatientsController(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<IActionResult> Profile()
    {
        var profile = await GetCurrentProfileAsync();
        if (profile is null)
            return RedirectToAction("Login", "Auth");

        if (profile.Role != 1)
            return Redirect("/student");

        return View("~/Views/Patient/Profile.cshtml", profile);
    }

    public async Task<IActionResult> EditProfile()
    {
        var profile = await GetCurrentProfileAsync();
        if (profile is null)
            return RedirectToAction("Login", "Auth");

        if (profile.Role != 1)
            return Redirect("/student");

        return View("~/Views/Patient/EditProfile.cshtml", profile);
    }

    public async Task<IActionResult> PatientProfile()
    {
        return await Profile();
    }

    private async Task<ProfileDTO?> GetCurrentProfileAsync()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        return userId.HasValue ? await _authService.GetProfileAsync(userId.Value) : null;
    }
}
