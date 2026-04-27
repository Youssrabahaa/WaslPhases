using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using phase_1.Models;

namespace phase_1.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {

        builder.HasOne(r => r.Reporter)
              .WithMany(u => u.ReportsMade)
              .HasForeignKey(r => r.ReporterId)
              .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Reported)
               .WithMany(u => u.ReportsReceived)
               .HasForeignKey(r => r.ReportedId)
               .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.HandledByAdmin)
            .WithMany(x => x.ReportsHandled)
            .HasForeignKey(x => x.HandledByAdminId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.TargetType, x.TargetId });
    }
}
