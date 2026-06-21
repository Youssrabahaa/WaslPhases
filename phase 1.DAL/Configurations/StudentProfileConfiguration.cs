using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using phase_1.Models;

namespace phase_1.Configurations;

public class StudentProfileConfiguration : IEntityTypeConfiguration<StudentProfile>
{
    public void Configure(EntityTypeBuilder<StudentProfile> builder)
    {
        builder
            .HasOne(x => x.User)
            .WithOne(x => x.StudentProfile)
            .HasForeignKey<StudentProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.Faculty)
            .WithMany(x => x.StudentProfiles)
            .HasForeignKey(x => x.FacultyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.IsVerified);
    }
}
