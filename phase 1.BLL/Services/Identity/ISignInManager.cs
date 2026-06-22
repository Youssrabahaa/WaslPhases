using phase_1.Models;

namespace phase_1.Services.Identity;

public interface ISignInManager
{
    Task<(bool Success, string? Error, ApplicationUser? User)> PasswordSignInAsync(string phone, string password);
}
