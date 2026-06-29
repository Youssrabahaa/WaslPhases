using phase_1.Models;

namespace phase_1.Repositories;

public interface IUserRepository
{
    Task<ApplicationUser?> GetByIdAsync(int id);

    Task<ApplicationUser?> GetByPhoneAsync(string phone);

    Task<ApplicationUser?> GetByPhoneWithProfileAsync(string phone);

    Task<ApplicationUser?> GetByIdWithProfileAsync(int id);

    Task<ApplicationUser?> GetByRefreshTokenHashAsync(string refreshTokenHash);

    Task<bool> PhoneExistsAsync(string phone);

    Task<bool> EmailExistsAsync(string email);

    Task<bool> StudentCodeExistsAsync(string studentCode);

    Task AddAsync(ApplicationUser user);

    void Update(ApplicationUser user);

    Task SaveChangesAsync();
}
