using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using phase_1.DAL.Models;

namespace phase_1.Configurations;

public class NoShowStrikeConfiguration : IEntityTypeConfiguration<NoShowStrike>
{
    public void Configure(EntityTypeBuilder<NoShowStrike> builder)
    {

        builder
            .HasOne(x => x.Session)
            .WithOne(x => x.NoShowStrike)
            .HasForeignKey<NoShowStrike>(x => x.SessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(x => x.User)
            .WithMany(x => x.NoShowStrikes)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
