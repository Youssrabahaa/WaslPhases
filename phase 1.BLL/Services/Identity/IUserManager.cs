using phase_1.DAL.Models;

namespace phase_1.Services.Identity;

public interface IUserManager
{
    Task<ApplicationUser?> FindByIdAsync(int id);

    Task<ApplicationUser?> FindByPhoneAsync(string phone);

    Task<ApplicationUser?> FindByPhoneWithProfileAsync(string phone);

    Task<ApplicationUser?> FindByIdWithProfileAsync(int id);

    Task<ApplicationUser?> FindByRefreshTokenAsync(string refreshToken);

    Task<bool> PhoneExistsAsync(string phone);

    Task<bool> EmailExistsAsync(string email);

    Task<bool> StudentCodeExistsAsync(string studentCode);

    Task CreateAsync(ApplicationUser user);

    Task UpdateAsync(ApplicationUser user);

    bool CheckPassword(ApplicationUser user, string password);

    Task SetPasswordAsync(ApplicationUser user, string newPassword);
}
