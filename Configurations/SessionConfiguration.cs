using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using phase_1.Models;

namespace phase_1.Configurations;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        // Property constraints provided by model attributes.

        builder
            .HasOne(x => x.Match)
            .WithMany(x => x.Sessions)
            .HasForeignKey(x => x.MatchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.SupervisorUser)
            .WithMany(x => x.SessionsSupervised)
            .HasForeignKey(x => x.SupervisorUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.CancelledByUser)
            .WithMany(x => x.SessionsCancelled)
            .HasForeignKey(x => x.CancelledByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.SupervisorUserId);
    }
}
