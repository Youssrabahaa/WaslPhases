using Microsoft.AspNetCore.Mvc;
using phase_1.Services;

namespace phase_1.BLL.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IAuthService _authService;

    public AccountController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> Profile()
    {
        var userId = GetCurrentUserId();
        if (!userId.HasValue)
            return Unauthorized(new { message = "Access token is missing or invalid." });

        var profile = await _authService.GetProfileAsync(userId.Value);
        return profile is null ? NotFound(new { message = "User not found." }) : Ok(profile);
    }

    private int? GetCurrentUserId()
    {
        return HttpContext.Items.TryGetValue("UserId", out var value) && value is int userId ? userId : null;
    }
}
