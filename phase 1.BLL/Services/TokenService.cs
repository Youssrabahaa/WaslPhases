using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using phase_1.Models;

namespace phase_1.Services;

public class TokenService : ITokenService
{
    private const int PasswordSaltSize = 16;
    private const int PasswordHashSize = 32;
    private const int PasswordIterations = 100_000;
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(ApplicationUser user)
    {
        var expiresAt = DateTimeOffset.UtcNow.AddMinutes(GetAccessTokenMinutes()).ToUnixTimeSeconds();
        var header = new Dictionary<string, object> { ["alg"] = "HS256", ["typ"] = "JWT" };
        var payload = new Dictionary<string, object?>
        {
            ["sub"] = user.Id,
            ["name"] = user.FullName,
            ["phone"] = user.Phone,
            ["role"] = user.Role,
            ["exp"] = expiresAt,
            ["iat"] = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            ["jti"] = Guid.NewGuid().ToString("N")
        };

        var encodedHeader = Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(header));
        var encodedPayload = Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(payload));
        var unsignedToken = $"{encodedHeader}.{encodedPayload}";

        return $"{unsignedToken}.{Sign(unsignedToken, GetAccessTokenSecret())}";
    }

    public int? ValidateAccessToken(string accessToken)
    {
        try
        {
            var parts = accessToken.Split('.');
            if (parts.Length != 3)
                return null;

            var unsignedToken = $"{parts[0]}.{parts[1]}";
            if (!FixedTimeEquals(parts[2], Sign(unsignedToken, GetAccessTokenSecret())))
                return null;

            using var document = JsonDocument.Parse(Base64UrlDecode(parts[1]));
            var payload = document.RootElement;

            if (!payload.TryGetProperty("exp", out var expElement))
                return null;

            if (DateTimeOffset.FromUnixTimeSeconds(expElement.GetInt64()) <= DateTimeOffset.UtcNow)
                return null;

            if (!payload.TryGetProperty("sub", out var subElement))
                return null;

            return subElement.ValueKind == JsonValueKind.Number
                ? subElement.GetInt32()
                : int.TryParse(subElement.GetString(), out var userId) ? userId : null;
        }
        catch
        {
            return null;
        }
    }

    public string GenerateRefreshToken()
    {
        return Base64UrlEncode(RandomNumberGenerator.GetBytes(64));
    }

    public string HashToken(string token)
    {
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
    }

    public string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(PasswordSaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, PasswordIterations, HashAlgorithmName.SHA256, PasswordHashSize);
        return $"PBKDF2-SHA256${PasswordIterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        var parts = passwordHash.Split('$');
        if (parts.Length != 4 || parts[0] != "PBKDF2-SHA256" || !int.TryParse(parts[1], out var iterations))
            return false;

        var salt = Convert.FromBase64String(parts[2]);
        var expectedHash = Convert.FromBase64String(parts[3]);
        var actualHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expectedHash.Length);

        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }

    private string GetAccessTokenSecret()
    {
        var secret = _configuration["Auth:AccessTokenSecret"];
        if (string.IsNullOrWhiteSpace(secret) || secret.Length < 32)
            throw new InvalidOperationException("Auth:AccessTokenSecret must be at least 32 characters.");

        return secret;
    }

    private int GetAccessTokenMinutes()
    {
        return int.TryParse(_configuration["Auth:AccessTokenMinutes"], out var minutes) ? minutes : 15;
    }

    private static string Sign(string value, string secret)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        return Base64UrlEncode(hmac.ComputeHash(Encoding.UTF8.GetBytes(value)));
    }

    private static string Base64UrlEncode(byte[] bytes)
    {
        return Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    private static byte[] Base64UrlDecode(string value)
    {
        var base64 = value.Replace('-', '+').Replace('_', '/');
        var padding = 4 - base64.Length % 4;
        if (padding < 4)
            base64 = base64.PadRight(base64.Length + padding, '=');

        return Convert.FromBase64String(base64);
    }

    private static bool FixedTimeEquals(string left, string right)
    {
        var leftBytes = Encoding.UTF8.GetBytes(left);
        var rightBytes = Encoding.UTF8.GetBytes(right);
        return leftBytes.Length == rightBytes.Length && CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
    }
}
