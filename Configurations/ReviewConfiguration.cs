using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using phase_1.Models;

namespace phase_1.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        // Rating/comment/createdAt configured via model attributes.
        builder.ToTable(t => t.HasCheckConstraint("CK_Review_Rating", "[Rating] >= 1 AND [Rating] <= 5"));

        builder
            .HasOne(x => x.Match)
            .WithMany(x => x.Reviews)
            .HasForeignKey(x => x.MatchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.ReviewerUser)
            .WithMany(x => x.ReviewsWritten)
            .HasForeignKey(x => x.ReviewerUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.ReviewedUser)
            .WithMany(x => x.ReviewsReceived)
            .HasForeignKey(x => x.ReviewedUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
