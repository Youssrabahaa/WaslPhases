using Microsoft.Extensions.Configuration;
using phase_1.DAL.Models;
using phase_1.DTOs;
using phase_1.Services.Identity;

namespace phase_1.Services;

public class AuthService : IAuthService
{
    private const int RegisterOtpPurpose = 1;
    private const int ForgotPasswordOtpPurpose = 3;

    private readonly IConfiguration _configuration;
    private readonly IOTPService _otpService;
    private readonly ITokenService _tokenService;
    private readonly IUserManager _userManager;
    private readonly IRoleManager _roleManager;
    private readonly ISignInManager _signInManager;

    public AuthService(
        IConfiguration configuration,
        IOTPService otpService,
        ITokenService tokenService,
        IUserManager userManager,
        IRoleManager roleManager,
        ISignInManager signInManager)
    {
        _configuration = configuration;
        _otpService = otpService;
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }

    public async Task<(bool Success, string? Error, ProfileDTO? Profile)> RegisterAsync(RegisterDTO dto, string? ipAddress = null)
    {
        var phone = NormalizePhone(dto.Phone);
        var email = NormalizeEmail(dto.Email);

        if (!_roleManager.IsSupportedRole(dto.Role))
            return (false, "Invalid user role.", null);

        if (await _userManager.PhoneExistsAsync(phone))
            return (false, "Phone number is already registered.", null);

        if (email is not null && await _userManager.EmailExistsAsync(email))
            return (false, "Email is already registered.", null);

        var studentCode = dto.StudentCode?.Trim();
        if (_roleManager.IsStudent(dto.Role) &&
            studentCode is not null &&
            await _userManager.StudentCodeExistsAsync(studentCode))
        {
            return (false, "Student code is already registered.", null);
        }

        var user = new ApplicationUser
        {
            Phone = phone,
            FullName = dto.FullName.Trim(),
            Email = email,
            PasswordHash = _tokenService.HashPassword(dto.Password),
            Role = dto.Role,
            Status = 1,
            IsPhoneVerified = false,
            IsEmailVerified = false,
            CreatedAt = DateTime.UtcNow
        };

        if (_roleManager.IsPatient(dto.Role))
        {
            user.PatientProfile = new PatientProfile
            {
                BirthDate = dto.BirthDate,
                Gender = dto.Gender?.Trim(),
                Notes = dto.Notes?.Trim()
            };
        }

        if (_roleManager.IsStudent(dto.Role))
        {
            user.StudentProfile = new StudentProfile
            {
                FacultyId = dto.FacultyId!.Value,
                AcademicYear = dto.AcademicYear!.Value,
                ClinicName = dto.ClinicName?.Trim(),
                StudentCode = studentCode,
                SupervisorName = dto.SupervisorName?.Trim(),
                RequiredCasesCount = dto.RequiredCasesCount ?? 0,
                CompletedCasesCount = 0,
                IsVerified = false
            };
        }

        await _userManager.CreateAsync(user);
        await _otpService.GenerateOtpAsync(user.Phone, RegisterOtpPurpose, user.Id, ipAddress);

        return (true, null, MapProfile(user));
    }

    public async Task<(bool Success, string? Error, string? AccessToken, string? RefreshToken, ProfileDTO? Profile)> LoginAsync(LoginDTO dto, string? ipAddress = null)
    {
        var signInResult = await _signInManager.PasswordSignInAsync(NormalizePhone(dto.Phone), dto.Password);
        if (!signInResult.Success || signInResult.User is null)
            return (false, signInResult.Error, null, null, null);

        var tokenResult = await CreateTokenPairAsync(signInResult.User, dto.RememberMe, ipAddress);
        return (true, null, tokenResult.AccessToken, tokenResult.RefreshToken, MapProfile(signInResult.User));
    }

    public async Task<(bool Success, string? Error, string? AccessToken, string? RefreshToken, ProfileDTO? Profile)> RefreshTokenAsync(string refreshToken, string? ipAddress = null)
    {
        var user = await _userManager.FindByRefreshTokenAsync(refreshToken);
        if (user is null)
            return (false, "Invalid refresh token.", null, null, null);

        var refreshTokenHash = _tokenService.HashToken(refreshToken);
        var storedToken = user.RefreshTokens.FirstOrDefault(x => x.TokenHash == refreshTokenHash);
        if (storedToken is null || !storedToken.IsActive)
            return (false, "Invalid refresh token.", null, null, null);

        var tokenResult = await CreateTokenPairAsync(user, rememberMe: false, ipAddress);
        storedToken.RevokedAt = DateTime.UtcNow;
        storedToken.RevokedByIp = ipAddress;
        storedToken.ReplacedByTokenHash = _tokenService.HashToken(tokenResult.RefreshToken);
        storedToken.RevocationReason = "Rotated";

        await _userManager.UpdateAsync(user);
        return (true, null, tokenResult.AccessToken, tokenResult.RefreshToken, MapProfile(user));
    }

    public async Task<bool> RevokeRefreshTokenAsync(string refreshToken, string? reason = null, string? ipAddress = null)
    {
        var user = await _userManager.FindByRefreshTokenAsync(refreshToken);
        if (user is null)
            return false;

        var refreshTokenHash = _tokenService.HashToken(refreshToken);
        var storedToken = user.RefreshTokens.FirstOrDefault(x => x.TokenHash == refreshTokenHash);
        if (storedToken is null || storedToken.IsRevoked)
            return false;

        storedToken.RevokedAt = DateTime.UtcNow;
        storedToken.RevokedByIp = ipAddress;
        storedToken.RevocationReason = reason;

        await _userManager.UpdateAsync(user);
        return true;
    }

    public async Task<bool> VerifyOtpAsync(VerifyOtpDTO dto)
    {
        var phone = NormalizePhone(dto.Phone);
        var isVerified = await _otpService.VerifyOtpAsync(phone, dto.Code, dto.Purpose);
        if (!isVerified || dto.Purpose != RegisterOtpPurpose)
            return isVerified;

        var user = await _userManager.FindByPhoneAsync(phone);
        if (user is null)
            return true;

        user.IsPhoneVerified = true;
        user.UpdatedAt = DateTime.UtcNow;
        await _userManager.UpdateAsync(user);

        return true;
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordDTO dto, string? ipAddress = null)
    {
        var phone = NormalizePhone(dto.Phone);
        var user = await _userManager.FindByPhoneAsync(phone);
        if (user is null)
            return true;

        await _otpService.GenerateOtpAsync(phone, ForgotPasswordOtpPurpose, user.Id, ipAddress);
        return true;
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDTO dto)
    {
        var phone = NormalizePhone(dto.Phone);
        var user = await _userManager.FindByPhoneAsync(phone);
        if (user is null)
            return false;

        var isFixedOtp = UseFixedOtp() && dto.Code.Trim() == GetFixedOtpCode();
        if (!isFixedOtp && !await _otpService.VerifyOtpAsync(phone, dto.Code, ForgotPasswordOtpPurpose))
            return false;

        user.FailedLoginAttempts = 0;
        user.LockedUntil = null;
        await _userManager.SetPasswordAsync(user, dto.NewPassword);

        return true;
    }

    public async Task<ProfileDTO?> GetProfileAsync(int userId)
    {
        var user = await _userManager.FindByIdWithProfileAsync(userId);
        return user is null ? null : MapProfile(user);
    }

    private async Task<(string AccessToken, string RefreshToken)> CreateTokenPairAsync(ApplicationUser user, bool rememberMe, string? ipAddress)
    {
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshTokens.Add(new RefreshToken
        {
            TokenHash = _tokenService.HashToken(refreshToken),
            ExpiresAt = DateTime.UtcNow.AddDays(GetRefreshTokenDays(rememberMe)),
            CreatedAt = DateTime.UtcNow,
            CreatedByIp = ipAddress
        });

        await _userManager.UpdateAsync(user);
        return (accessToken, refreshToken);
    }

    private int GetRefreshTokenDays(bool rememberMe)
    {
        var key = rememberMe ? "Auth:RememberMeRefreshTokenDays" : "Auth:RefreshTokenDays";
        return int.TryParse(_configuration[key], out var days) ? days : rememberMe ? 60 : 30;
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

    private static ProfileDTO MapProfile(ApplicationUser user)
    {
        return new ProfileDTO
        {
            Id = user.Id,
            FullName = user.FullName,
            Phone = user.Phone,
            Email = user.Email,
            Role = user.Role,
            Status = user.Status,
            IsPhoneVerified = user.IsPhoneVerified,
            IsEmailVerified = user.IsEmailVerified,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            LastLoginAt = user.LastLoginAt,
            PatientProfile = user.PatientProfile is null ? null : new PatientProfileDTO
            {
                BirthDate = user.PatientProfile.BirthDate,
                Gender = user.PatientProfile.Gender,
                Notes = user.PatientProfile.Notes
            },
            StudentProfile = user.StudentProfile is null ? null : new StudentProfileDTO
            {
                FacultyId = user.StudentProfile.FacultyId,
                FacultyName = user.StudentProfile.Faculty?.Name,
                AcademicYear = user.StudentProfile.AcademicYear,
                ClinicName = user.StudentProfile.ClinicName,
                StudentCode = user.StudentProfile.StudentCode,
                SupervisorName = user.StudentProfile.SupervisorName,
                RequiredCasesCount = user.StudentProfile.RequiredCasesCount,
                CompletedCasesCount = user.StudentProfile.CompletedCasesCount,
                IsVerified = user.StudentProfile.IsVerified,
                VerifiedAt = user.StudentProfile.VerifiedAt
            }
        };
    }

    private static string NormalizePhone(string phone) => phone.Trim();

    private static string? NormalizeEmail(string? email) => string.IsNullOrWhiteSpace(email) ? null : email.Trim().ToLowerInvariant();
}
