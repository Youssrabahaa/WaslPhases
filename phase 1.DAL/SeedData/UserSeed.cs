using phase_1.DAL.Models;

namespace phase_1.SeedData;

public static class UserSeed
{
    public static readonly ApplicationUser[] Data =
    {
        new()
        {
            Id = SeedIds.Users.PatientMona,
            Phone = "01010000001",
            FullName = "Mona Ahmed Abdelrahman",
            Email = "mona.ahmed@example.com",
            PasswordHash = "seed-password-hash",
            Role = 1,
            Status = 1,
            IsPhoneVerified = true,
            IsEmailVerified = true,
            CreatedAt = new DateTime(2026, 1, 10, 9, 0, 0, DateTimeKind.Utc),
            LastLoginAt = new DateTime(2026, 4, 10, 18, 30, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = SeedIds.Users.PatientYoussef,
            Phone = "01010000002",
            FullName = "Youssef Mohamed Elsayed",
            Email = "youssef.mohamed@example.com",
            PasswordHash = "seed-password-hash",
            Role = 1,
            Status = 1,
            IsPhoneVerified = true,
            IsEmailVerified = true,
            CreatedAt = new DateTime(2026, 1, 14, 11, 15, 0, DateTimeKind.Utc),
            LastLoginAt = new DateTime(2026, 4, 12, 14, 0, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = SeedIds.Users.StudentAhmed,
            Phone = "01010000003",
            FullName = "Ahmed Khaled Mansour",
            Email = "ahmed.khaled@example.com",
            PasswordHash = "seed-password-hash",
            Role = 2,
            Status = 1,
            IsPhoneVerified = true,
            IsEmailVerified = true,
            CreatedAt = new DateTime(2026, 1, 18, 8, 45, 0, DateTimeKind.Utc),
            LastLoginAt = new DateTime(2026, 4, 15, 10, 20, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = SeedIds.Users.StudentSalma,
            Phone = "01010000004",
            FullName = "Salma Tarek Hassan",
            Email = "salma.tarek@example.com",
            PasswordHash = "seed-password-hash",
            Role = 2,
            Status = 1,
            IsPhoneVerified = true,
            IsEmailVerified = true,
            CreatedAt = new DateTime(2026, 1, 20, 13, 0, 0, DateTimeKind.Utc),
            LastLoginAt = new DateTime(2026, 4, 16, 9, 10, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = SeedIds.Users.PatientNadine,
            Phone = "01010000005",
            FullName = "Nadine Samir Fathy",
            Email = "nadine.samir@example.com",
            PasswordHash = "seed-password-hash",
            Role = 1,
            Status = 1,
            IsPhoneVerified = true,
            IsEmailVerified = true,
            CreatedAt = new DateTime(2026, 2, 2, 10, 0, 0, DateTimeKind.Utc),
            LastLoginAt = new DateTime(2026, 6, 28, 16, 0, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = SeedIds.Users.PatientKarim,
            Phone = "01010000006",
            FullName = "Karim Hany Mostafa",
            Email = "karim.hany@example.com",
            PasswordHash = "seed-password-hash",
            Role = 1,
            Status = 1,
            IsPhoneVerified = true,
            IsEmailVerified = true,
            CreatedAt = new DateTime(2026, 2, 4, 10, 30, 0, DateTimeKind.Utc),
            LastLoginAt = new DateTime(2026, 6, 29, 18, 0, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = SeedIds.Users.StudentOmar,
            Phone = "01010000007",
            FullName = "Omar Nabil Farouk",
            Email = "omar.nabil@example.com",
            PasswordHash = "seed-password-hash",
            Role = 2,
            Status = 1,
            IsPhoneVerified = true,
            IsEmailVerified = true,
            CreatedAt = new DateTime(2026, 2, 8, 9, 0, 0, DateTimeKind.Utc),
            LastLoginAt = new DateTime(2026, 6, 30, 11, 0, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = SeedIds.Users.StudentLaila,
            Phone = "01010000008",
            FullName = "Laila Ashraf Amin",
            Email = "laila.ashraf@example.com",
            PasswordHash = "seed-password-hash",
            Role = 2,
            Status = 1,
            IsPhoneVerified = true,
            IsEmailVerified = true,
            CreatedAt = new DateTime(2026, 2, 10, 12, 0, 0, DateTimeKind.Utc),
            LastLoginAt = new DateTime(2026, 6, 30, 12, 30, 0, DateTimeKind.Utc)
        }
    };
}
