using phase_1.DAL.Models;

namespace phase_1.SeedData;

public static class ProfileSeed
{
    public static readonly PatientProfile[] PatientProfiles =
    {
        new()
        {
            UserId = SeedIds.Users.PatientMona,
            BirthDate = new DateTime(2019, 5, 12),
            Gender = "Female",
            Notes = "Young child who needs regular follow-up for articulation and communication skills."
        },
        new()
        {
            UserId = SeedIds.Users.PatientYoussef,
            BirthDate = new DateTime(1988, 11, 3),
            Gender = "Male",
            Notes = "Needs speech rehabilitation sessions after a stroke with family support."
        }
    };

    public static readonly StudentProfile[] StudentProfiles =
    {
        new()
        {
            UserId = SeedIds.Users.StudentAhmed,
            FacultyId = SeedIds.Faculties.CairoDentistry,
            AcademicYear = 4,
            ClinicName = "Kasr Al-Ainy Teaching Dental Clinic",
            StudentCode = "CAI-DEN-2401",
            SupervisorName = "Dr. Hala Samir",
            RequiredCasesCount = 8,
            CompletedCasesCount = 5,
            IsVerified = true,
            VerifiedAt = new DateTime(2026, 2, 1, 12, 0, 0, DateTimeKind.Utc)
        },
        new()
        {
            UserId = SeedIds.Users.StudentSalma,
            FacultyId = SeedIds.Faculties.AlexandriaDentistry,
            AcademicYear = 3,
            ClinicName = "Alexandria University Dental Hospital",
            StudentCode = "ALX-DEN-1732",
            SupervisorName = "Dr. Naglaa Adel",
            RequiredCasesCount = 6,
            CompletedCasesCount = 3,
            IsVerified = true,
            VerifiedAt = new DateTime(2026, 2, 5, 10, 30, 0, DateTimeKind.Utc)
        }
    };
}
