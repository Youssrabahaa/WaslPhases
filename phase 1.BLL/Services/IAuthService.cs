using phase_1.DTOs;

namespace phase_1.Services;

public interface IAuthService
{
    Task<(bool Success, string? Error, ProfileDTO? Profile)> RegisterAsync(RegisterDTO dto, string? ipAddress = null);

    Task<(bool Success, string? Error, string? AccessToken, string? RefreshToken, ProfileDTO? Profile)> LoginAsync(LoginDTO dto, string? ipAddress = null);

    Task<(bool Success, string? Error, string? AccessToken, string? RefreshToken, ProfileDTO? Profile)> RefreshTokenAsync(string refreshToken, string? ipAddress = null);

    Task<bool> RevokeRefreshTokenAsync(string refreshToken, string? reason = null, string? ipAddress = null);

    Task<bool> VerifyOtpAsync(VerifyOtpDTO dto);

    Task<bool> ForgotPasswordAsync(ForgotPasswordDTO dto, string? ipAddress = null);

    Task<bool> ResetPasswordAsync(ResetPasswordDTO dto);

    Task<ProfileDTO?> GetProfileAsync(int userId);
}
