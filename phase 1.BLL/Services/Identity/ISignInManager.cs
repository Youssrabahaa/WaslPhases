using phase_1.DAL.Models;

namespace phase_1.Services.Identity;

public interface ISignInManager
{
    Task<(bool Success, string? Error, ApplicationUser? User)> PasswordSignInAsync(string phone, string password);
}
