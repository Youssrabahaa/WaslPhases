using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using phase_1.Models;

namespace phase_1.Configurations;

public class UniversityConfiguration : IEntityTypeConfiguration<University>
{
    public void Configure(EntityTypeBuilder<University> builder)
    {
        builder
            .HasMany(x => x.Faculties)
            .WithOne(x => x.University)
            .HasForeignKey(x => x.UniversityId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
