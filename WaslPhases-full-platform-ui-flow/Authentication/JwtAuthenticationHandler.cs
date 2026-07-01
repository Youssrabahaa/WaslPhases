using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using phase_1.Services;

namespace phase_1.Authentication;

public class JwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "Bearer";

    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;

    public JwtAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IAuthService authService,
        ITokenService tokenService)
        : base(options, logger, encoder)
    {
        _authService = authService;
        _tokenService = tokenService;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var token = GetBearerToken(Request.Headers.Authorization.FirstOrDefault());
        if (string.IsNullOrWhiteSpace(token))
            return AuthenticateResult.NoResult();

        var userId = _tokenService.ValidateAccessToken(token);
        if (!userId.HasValue)
            return AuthenticateResult.Fail("Invalid access token.");

        var profile = await _authService.GetProfileAsync(userId.Value);
        if (profile is null)
            return AuthenticateResult.Fail("User not found.");

        if (profile.Status != 1)
            return AuthenticateResult.Fail("User account is inactive.");

        if (!profile.IsPhoneVerified)
            return AuthenticateResult.Fail("Phone number is not verified.");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, profile.Id.ToString()),
            new(ClaimTypes.Name, profile.FullName),
            new(ClaimTypes.MobilePhone, profile.Phone),
            new("role_id", profile.Role.ToString())
        };

        var roleName = GetRoleName(profile.Role);
        if (!string.IsNullOrWhiteSpace(roleName))
            claims.Add(new Claim(ClaimTypes.Role, roleName));

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }

    private static string? GetBearerToken(string? authorizationHeader)
    {
        const string bearerPrefix = "Bearer ";
        return authorizationHeader is not null && authorizationHeader.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase)
            ? authorizationHeader[bearerPrefix.Length..].Trim()
            : null;
    }

    private static string? GetRoleName(int role)
    {
        return role switch
        {
            1 => "Patient",
            2 => "Student",
            3 => "Admin",
            _ => null
        };
    }
}
