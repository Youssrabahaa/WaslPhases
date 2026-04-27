using Microsoft.EntityFrameworkCore;
using phase_1.Models;

namespace phase_1.SeedData;

public static class ModelBuilderSeedExtensions
{
    public static void ApplyAppSeeds(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(UserSeed.Data);
        modelBuilder.Entity<University>().HasData(ReferenceSeed.Universities);
        modelBuilder.Entity<Faculty>().HasData(ReferenceSeed.Faculties);
        modelBuilder.Entity<PatientProfile>().HasData(ProfileSeed.PatientProfiles);
        modelBuilder.Entity<StudentProfile>().HasData(ProfileSeed.StudentProfiles);
        modelBuilder.Entity<ServiceType>().HasData(ReferenceSeed.ServiceTypes);
        modelBuilder.Entity<TreatmentCategory>().HasData(ReferenceSeed.TreatmentCategories);
        modelBuilder.Entity<Case>().HasData(CaseSeed.Data);
    }
}
