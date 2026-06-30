using Microsoft.EntityFrameworkCore;
using phase_1.Data;
using phase_1.DAL.Models;

namespace phase_1.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<ApplicationUser?> GetByIdAsync(int id)
    {
        return _context.Users.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<ApplicationUser?> GetByPhoneAsync(string phone)
    {
        return _context.Users.FirstOrDefaultAsync(x => x.Phone == phone);
    }

    public Task<ApplicationUser?> GetByPhoneWithProfileAsync(string phone)
    {
        return _context.Users
            .Include(x => x.PatientProfile)
            .Include(x => x.StudentProfile)
                .ThenInclude(x => x!.Faculty)
            .FirstOrDefaultAsync(x => x.Phone == phone);
    }

    public Task<ApplicationUser?> GetByIdWithProfileAsync(int id)
    {
        return _context.Users
            .Include(x => x.PatientProfile)
            .Include(x => x.StudentProfile)
                .ThenInclude(x => x!.Faculty)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<ApplicationUser?> GetByRefreshTokenHashAsync(string refreshTokenHash)
    {
        return _context.Users
            .Include(x => x.PatientProfile)
            .Include(x => x.StudentProfile)
                .ThenInclude(x => x!.Faculty)
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(x => x.RefreshTokens.Any(t => t.TokenHash == refreshTokenHash));
    }

    public Task<bool> PhoneExistsAsync(string phone)
    {
        return _context.Users.AnyAsync(x => x.Phone == phone);
    }

    public Task<bool> EmailExistsAsync(string email)
    {
        return _context.Users.AnyAsync(x => x.Email == email);
    }

    public async Task AddAsync(ApplicationUser user)
    {
        await _context.Users.AddAsync(user);
    }

    public void Update(ApplicationUser user)
    {
        _context.Users.Update(user);
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
