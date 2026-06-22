using Microsoft.AspNetCore.Mvc;
using phase_1.DTOs;
using phase_1.Services;

namespace phase_1.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult VerifyOTP()
    {
        return View();
    }

    public IActionResult Logout()
    {
        return RedirectToAction(nameof(Login));
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    public IActionResult ResetPassword()
    {
        return View();
    }

    public IActionResult SelectRole()
    {
        return View();
    }

    [HttpPost("/api/auth/register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RegisterAsync(dto, GetIpAddress());
        return result.Success ? Ok(result.Profile) : BadRequest(new { message = result.Error });
    }

    [HttpPost("/api/auth/login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(dto, GetIpAddress());
        if (!result.Success || result.AccessToken is null || result.RefreshToken is null || result.Profile is null)
            return Unauthorized(new { message = result.Error });

        return Ok(new AuthResponseDTO
        {
            AccessToken = result.AccessToken,
            RefreshToken = result.RefreshToken,
            Profile = result.Profile
        });
    }

    [HttpPost("/api/auth/verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var isVerified = await _authService.VerifyOtpAsync(dto);
        return isVerified ? Ok(new { message = "OTP verified successfully." }) : BadRequest(new { message = "Invalid OTP." });
    }

    [HttpPost("/api/auth/forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _authService.ForgotPasswordAsync(dto, GetIpAddress());
        return Ok(new { message = "If the phone number exists, an OTP has been sent." });
    }

    [HttpPost("/api/auth/reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var isReset = await _authService.ResetPasswordAsync(dto);
        return isReset ? Ok(new { message = "Password reset successfully." }) : BadRequest(new { message = "Invalid OTP or phone number." });
    }

    [HttpPost("/api/auth/refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RefreshTokenAsync(dto.RefreshToken, GetIpAddress());
        if (!result.Success || result.AccessToken is null || result.RefreshToken is null || result.Profile is null)
            return Unauthorized(new { message = result.Error });

        return Ok(new AuthResponseDTO
        {
            AccessToken = result.AccessToken,
            RefreshToken = result.RefreshToken,
            Profile = result.Profile
        });
    }

    [HttpPost("/api/auth/revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshToken([FromBody] RevokeRefreshTokenDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var isRevoked = await _authService.RevokeRefreshTokenAsync(dto.RefreshToken, dto.Reason, GetIpAddress());
        return isRevoked ? Ok(new { message = "Refresh token revoked successfully." }) : BadRequest(new { message = "Invalid refresh token." });
    }

    private string? GetIpAddress()
    {
        return HttpContext.Connection.RemoteIpAddress?.ToString();
    }
}
