using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using phase_1.Models;

namespace phase_1.Configurations;

public class PatientProfileConfiguration : IEntityTypeConfiguration<PatientProfile>
{
    public void Configure(EntityTypeBuilder<PatientProfile> builder)
    {
        // Column/length constraints are defined via attributes on the model.

        builder
            .HasOne(x => x.User)
            .WithOne(x => x.PatientProfile)
            .HasForeignKey<PatientProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
