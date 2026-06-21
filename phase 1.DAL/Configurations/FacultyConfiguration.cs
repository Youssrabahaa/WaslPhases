using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using phase_1.Models;

namespace phase_1.Configurations;

public class FacultyConfiguration : IEntityTypeConfiguration<Faculty>
{
    public void Configure(EntityTypeBuilder<Faculty> builder)
    {
        builder
            .HasMany(x => x.StudentProfiles)
            .WithOne(x => x.Faculty)
            .HasForeignKey(x => x.FacultyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
