using Microsoft.AspNetCore.Mvc;
using phase_1.DTOs;
using phase_1.Services;

namespace phase_1.Controllers;

public class AuthController : Controller
{
    private readonly IAuthService _authService;
    private readonly IConfiguration _configuration;

    public AuthController(IAuthService authService, IConfiguration configuration)
    {
        _authService = authService;
        _configuration = configuration;
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    public IActionResult VerifyOTP(string? phone = null, int purpose = 1)
    {
        ViewData["Phone"] = phone;
        ViewData["Purpose"] = purpose;
        ViewData["DebugOtp"] = UseFixedOtp() ? GetFixedOtpCode() : null;
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

    public IActionResult ResetPassword(string? phone = null)
    {
        ViewData["Phone"] = phone;
        ViewData["DebugOtp"] = UseFixedOtp() ? GetFixedOtpCode() : null;
        return View();
    }

    public IActionResult SelectRole()
    {
        return View();
    }

    [HttpGet("/api/auth/register")]
    public IActionResult RegisterPageRedirect()
    {
        return Redirect("/Auth/Register");
    }

    [HttpGet("/api/auth/login")]
    public IActionResult LoginPageRedirect()
    {
        return Redirect("/Auth/Login");
    }

    [HttpGet("/api/auth/verify-otp")]
    public IActionResult VerifyOtpPageRedirect(string? phone = null, int purpose = 1)
    {
        return Redirect($"/Auth/VerifyOTP?phone={Uri.EscapeDataString(phone ?? string.Empty)}&purpose={purpose}");
    }

    [HttpPost("/Auth/RegisterForm")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterForm(RegisterDTO dto)
    {
        if (dto.Role == 0 && Request.Form.TryGetValue("Role", out var roleValue))
        {
            var roleText = roleValue.ToString();
            dto.Role = roleText.Equals("Patient", StringComparison.OrdinalIgnoreCase) ? 1 :
                roleText.Equals("Student", StringComparison.OrdinalIgnoreCase) ? 2 : dto.Role;
            ModelState.Remove(nameof(RegisterDTO.Role));
        }

        if (!ModelState.IsValid)
            return View(nameof(Register));

        var result = await _authService.RegisterAsync(dto, GetIpAddress());
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "فشل إنشاء الحساب.");
            return View(nameof(Register));
        }

        return Redirect($"/Auth/VerifyOTP?phone={Uri.EscapeDataString(dto.Phone)}&purpose=1");
    }

    [HttpPost("/Auth/LoginForm")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LoginForm(LoginDTO dto)
    {
        if (!ModelState.IsValid)
            return View(nameof(Login));

        var result = await _authService.LoginAsync(dto, GetIpAddress());
        if (!result.Success || result.Profile is null)
        {
            ModelState.AddModelError(string.Empty, result.Error ?? "فشل تسجيل الدخول.");
            return View(nameof(Login));
        }

        return result.Profile.Role == 1
            ? Redirect("/patient")
            : Redirect("/student");
    }

    [HttpPost("/Auth/VerifyOtpForm")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifyOtpForm(VerifyOtpDTO dto)
    {
        ViewData["Phone"] = dto.Phone;
        ViewData["Purpose"] = dto.Purpose;
        ViewData["DebugOtp"] = UseFixedOtp() ? GetFixedOtpCode() : null;

        if (!ModelState.IsValid)
            return View(nameof(VerifyOTP));

        var isVerified = await _authService.VerifyOtpAsync(dto);
        if (!isVerified)
        {
            ModelState.AddModelError(string.Empty, "رمز التحقق غير صحيح.");
            return View(nameof(VerifyOTP));
        }

        TempData["AuthMessage"] = "تم تأكيد رقم الهاتف بنجاح. يمكنك تسجيل الدخول الآن.";
        return Redirect("/Auth/Login");
    }

    [HttpPost("/Auth/ForgotPasswordForm")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPasswordForm(ForgotPasswordDTO dto)
    {
        if (!ModelState.IsValid)
            return View(nameof(ForgotPassword));

        await _authService.ForgotPasswordAsync(dto, GetIpAddress());
        return Redirect($"/Auth/ResetPassword?phone={Uri.EscapeDataString(dto.Phone)}");
    }

    [HttpPost("/Auth/ResetPasswordForm")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPasswordForm(ResetPasswordDTO dto)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Phone"] = dto.Phone;
            ViewData["DebugOtp"] = UseFixedOtp() ? GetFixedOtpCode() : null;
            return View(nameof(ResetPassword));
        }

        var isReset = await _authService.ResetPasswordAsync(dto);
        if (!isReset)
        {
            ViewData["Phone"] = dto.Phone;
            ViewData["DebugOtp"] = UseFixedOtp() ? GetFixedOtpCode() : null;
            ModelState.AddModelError(string.Empty, "رمز التحقق أو رقم الهاتف غير صحيح.");
            return View(nameof(ResetPassword));
        }

        return Redirect("/Auth/Login");
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
        return isVerified ? Ok(new { message = "تم تأكيد رمز التحقق بنجاح." }) : BadRequest(new { message = "رمز التحقق غير صحيح." });
    }

    [HttpPost("/api/auth/forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _authService.ForgotPasswordAsync(dto, GetIpAddress());
        return Ok(new { message = "إذا كان رقم الهاتف موجودا، سيتم إرسال رمز تحقق." });
    }

    [HttpPost("/api/auth/reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var isReset = await _authService.ResetPasswordAsync(dto);
        return isReset ? Ok(new { message = "تم تغيير كلمة المرور بنجاح." }) : BadRequest(new { message = "رمز التحقق أو رقم الهاتف غير صحيح." });
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
        return isRevoked ? Ok(new { message = "تم إلغاء رمز التحديث بنجاح." }) : BadRequest(new { message = "رمز التحديث غير صحيح." });
    }

    private string? GetIpAddress()
    {
        return HttpContext.Connection.RemoteIpAddress?.ToString();
    }

    private bool UseFixedOtp()
    {
        return bool.TryParse(_configuration["Auth:UseFixedOtp"], out var useFixedOtp) && useFixedOtp;
    }

    private string GetFixedOtpCode()
    {
        var configuredCode = _configuration["Auth:FixedOtpCode"];
        return string.IsNullOrWhiteSpace(configuredCode) ? "123456" : configuredCode.Trim();
    }
}
