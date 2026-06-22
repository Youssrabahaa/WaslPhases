using phase_1.Models;
using phase_1.Repositories;

namespace phase_1.Services.Identity;

public class UserManager : IUserManager
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;

    public UserManager(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public Task<ApplicationUser?> FindByIdAsync(int id) => _userRepository.GetByIdAsync(id);

    public Task<ApplicationUser?> FindByPhoneAsync(string phone) => _userRepository.GetByPhoneAsync(phone);

    public Task<ApplicationUser?> FindByPhoneWithProfileAsync(string phone) => _userRepository.GetByPhoneWithProfileAsync(phone);

    public Task<ApplicationUser?> FindByIdWithProfileAsync(int id) => _userRepository.GetByIdWithProfileAsync(id);

    public Task<ApplicationUser?> FindByRefreshTokenAsync(string refreshToken)
    {
        return _userRepository.GetByRefreshTokenHashAsync(_tokenService.HashToken(refreshToken));
    }

    public Task<bool> PhoneExistsAsync(string phone) => _userRepository.PhoneExistsAsync(phone);

    public Task<bool> EmailExistsAsync(string email) => _userRepository.EmailExistsAsync(email);

    public async Task CreateAsync(ApplicationUser user)
    {
        await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();
    }

    public Task UpdateAsync(ApplicationUser user)
    {
        _userRepository.Update(user);
        return _userRepository.SaveChangesAsync();
    }

    public bool CheckPassword(ApplicationUser user, string password)
    {
        return _tokenService.VerifyPassword(password, user.PasswordHash);
    }

    public Task SetPasswordAsync(ApplicationUser user, string newPassword)
    {
        user.PasswordHash = _tokenService.HashPassword(newPassword);
        user.UpdatedAt = DateTime.UtcNow;
        return UpdateAsync(user);
    }
}
