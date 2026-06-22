using phase_1.Services;

namespace phase_1.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ITokenService tokenService)
    {
        var token = GetBearerToken(context.Request.Headers.Authorization.FirstOrDefault());
        if (!string.IsNullOrWhiteSpace(token))
        {
            var userId = tokenService.ValidateAccessToken(token);
            if (userId.HasValue)
                context.Items["UserId"] = userId.Value;
        }

        await _next(context);
    }

    private static string? GetBearerToken(string? authorizationHeader)
    {
        const string bearerPrefix = "Bearer ";
        return authorizationHeader is not null && authorizationHeader.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase)
            ? authorizationHeader[bearerPrefix.Length..].Trim()
            : null;
    }
}
