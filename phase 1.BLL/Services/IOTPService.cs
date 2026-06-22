using phase_1.Models;

namespace phase_1.Services;

public interface IOTPService
{
    Task<string> GenerateOtpAsync(string phone, int purpose, int? userId = null, string? ipAddress = null);

    Task<bool> VerifyOtpAsync(string phone, string code, int purpose);

    Task<OtpCode?> GetActiveOtpAsync(string phone, int purpose);
}
