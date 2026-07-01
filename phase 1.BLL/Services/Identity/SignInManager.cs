using phase_1.Models;

namespace phase_1.Services.Identity;

public class SignInManager : ISignInManager
{
    private readonly IUserManager _userManager;

    public SignInManager(IUserManager userManager)
    {
        _userManager = userManager;
    }

    public async Task<(bool Success, string? Error, ApplicationUser? User)> PasswordSignInAsync(string phone, string password)
    {
        var user = await _userManager.FindByPhoneWithProfileAsync(phone);
        if (user is null)
            return (false, "Invalid phone or password.", null);

        if (user.LockedUntil.HasValue && user.LockedUntil > DateTime.UtcNow)
            return (false, "Account is temporarily locked.", null);

        if (user.Status != 1)
            return (false, "Account is inactive.", null);

        if (!user.IsPhoneVerified)
            return (false, "Phone number is not verified.", null);

        if (!_userManager.CheckPassword(user, password))
        {
            user.FailedLoginAttempts++;
            if (user.FailedLoginAttempts >= 5)
                user.LockedUntil = DateTime.UtcNow.AddMinutes(15);

            await _userManager.UpdateAsync(user);
            return (false, "Invalid phone or password.", null);
        }

        user.FailedLoginAttempts = 0;
        user.LockedUntil = null;
        user.LastLoginAt = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);
        return (true, null, user);
    }
}
