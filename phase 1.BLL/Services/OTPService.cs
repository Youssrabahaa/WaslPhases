using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using phase_1.Data;
using phase_1.DAL.Models;

namespace phase_1.Services;

public class OTPService : IOTPService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ITokenService _tokenService;

    public OTPService(AppDbContext context, IConfiguration configuration, ITokenService tokenService)
    {
        _context = context;
        _configuration = configuration;
        _tokenService = tokenService;
    }

    public async Task<string> GenerateOtpAsync(string phone, int purpose, int? userId = null, string? ipAddress = null)
    {
        var code = UseFixedOtp()
            ? GetFixedOtpCode()
            : Random.Shared.Next(100000, 999999).ToString();

        _context.OtpCodes.Add(new OtpCode
        {
            UserId = userId,
            Phone = phone.Trim(),
            CodeHash = _tokenService.HashToken(code),
            ExpiresAt = DateTime.UtcNow.AddMinutes(GetOtpMinutes()),
            Attempts = 0,
            MaxAttempts = 5,
            Purpose = purpose,
            CreatedByIp = ipAddress,
            CreatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        return code;
    }

    public async Task<bool> VerifyOtpAsync(string phone, string code, int purpose)
    {
        var otp = await GetActiveOtpAsync(phone, purpose);
        if (otp is null || otp.Attempts >= otp.MaxAttempts)
            return false;

        otp.Attempts++;
        var normalizedCode = code.Trim();
        var isValid = otp.CodeHash == _tokenService.HashToken(normalizedCode)
            || (UseFixedOtp() && normalizedCode == GetFixedOtpCode());
        if (isValid)
            otp.UsedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return isValid;
    }

    public Task<OtpCode?> GetActiveOtpAsync(string phone, int purpose)
    {
        var normalizedPhone = phone.Trim();
        return _context.OtpCodes
            .Where(x => x.Phone == normalizedPhone && x.Purpose == purpose && x.UsedAt == null && x.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync();
    }

    private int GetOtpMinutes()
    {
        return int.TryParse(_configuration["Auth:OtpMinutes"], out var minutes) ? minutes : 10;
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
