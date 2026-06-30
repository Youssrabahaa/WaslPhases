using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using phase_1.DAL.Models;

namespace phase_1.Configurations;

public class CaseConfiguration : IEntityTypeConfiguration<Case>
{
    public void Configure(EntityTypeBuilder<Case> builder)
    {
        builder.Property(x => x.EstimatedPriceMin).HasColumnType("decimal(10,2)");
        builder.Property(x => x.EstimatedPriceMax).HasColumnType("decimal(10,2)");

        builder
            .HasOne(x => x.PatientUser)
            .WithMany(x => x.CasesPosted)
            .HasForeignKey(x => x.PatientUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.ServiceType)
            .WithMany(x => x.Cases)
            .HasForeignKey(x => x.ServiceTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.TreatmentCategory)
            .WithMany(x => x.Cases)
            .HasForeignKey(x => x.TreatmentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.PatientUserId, x.Status, x.CreatedAt });

        builder
            .HasIndex(x => new { x.Status, x.Governorate, x.City, x.ServiceTypeId, x.CreatedAt });
    }
}
