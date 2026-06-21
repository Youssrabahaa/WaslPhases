using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using phase_1.Models;

namespace phase_1.Configurations;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        // Model attributes already specify status/createdAt constraints.

        builder
            .HasOne(x => x.Case)
            .WithMany(x => x.Matches)
            .HasForeignKey(x => x.CaseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Offer)
            .WithOne(x => x.Match)
            .HasForeignKey<Match>(x => x.OfferId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.PatientUser)
            .WithMany(x => x.PatientMatches)
            .HasForeignKey(x => x.PatientUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.StudentUser)
            .WithMany(x => x.StudentMatches)
            .HasForeignKey(x => x.StudentUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.CaseId)
            .IsUnique()
            .HasFilter($"[{nameof(Match.Status)}] IN (1, 2)");
    }
}
