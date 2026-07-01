using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using phase_1.DAL.Models;

namespace phase_1.DAL.Configurations;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        builder.Property(r => r.Description)
               .HasMaxLength(2000);

        builder.HasOne(r => r.ReporterUser)
               .WithMany(u => u.ReportsMade)
               .HasForeignKey(r => r.ReporterUserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.ReportedUser)
               .WithMany(u => u.ReportsReceived)
               .HasForeignKey(r => r.ReportedUserId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Session)
               .WithMany()
               .HasForeignKey(r => r.SessionId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => new { r.Status, r.CreatedAt });
    }
}
