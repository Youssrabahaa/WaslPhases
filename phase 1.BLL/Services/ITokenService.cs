using phase_1.DAL.Models;

namespace phase_1.Services;

public interface ITokenService
{
    string GenerateAccessToken(ApplicationUser user);

    int? ValidateAccessToken(string accessToken);

    string GenerateRefreshToken();

    string HashToken(string token);

    string HashPassword(string password);

    bool VerifyPassword(string password, string passwordHash);
}
