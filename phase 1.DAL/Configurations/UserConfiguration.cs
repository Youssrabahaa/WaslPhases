using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using phase_1.Models;

namespace phase_1.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Make Email uniqueness safe when Email is optional by adding a filtered index
        builder
            .HasIndex(u => u.Email)
            .IsUnique()
            .HasFilter("[Email] IS NOT NULL");
    }
}
