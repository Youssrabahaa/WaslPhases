using Microsoft.AspNetCore.Mvc;
using phase_1.DTOs;
using phase_1.Services;

namespace phase_1.Controllers;

public class StudentController : Controller
{
    private readonly IAuthService _authService;

    public StudentController(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<IActionResult> Profile()
    {
        var profile = await GetCurrentProfileAsync();
        if (profile is null)
            return RedirectToAction("Login", "Auth");

        if (profile.Role != 2)
            return Redirect("/patient");

        return View(profile);
    }

    public async Task<IActionResult> EditProfile()
    {
        var profile = await GetCurrentProfileAsync();
        if (profile is null)
            return RedirectToAction("Login", "Auth");

        if (profile.Role != 2)
            return Redirect("/patient");

        return View(profile);
    }

    private async Task<ProfileDTO?> GetCurrentProfileAsync()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        return userId.HasValue ? await _authService.GetProfileAsync(userId.Value) : null;
    }
}
