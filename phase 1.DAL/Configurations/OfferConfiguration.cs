using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using phase_1.Models;

namespace phase_1.Configurations;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        // Property constraints handled by model annotations.

        builder
            .HasOne(x => x.Case)
            .WithMany(x => x.Offers)
            .HasForeignKey(x => x.CaseId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.StudentUser)
            .WithMany(x => x.OffersMade)
            .HasForeignKey(x => x.StudentUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => new { x.StudentUserId, x.Status, x.CreatedAt });
    }
}
