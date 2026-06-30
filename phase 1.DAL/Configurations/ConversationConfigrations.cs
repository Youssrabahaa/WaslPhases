using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using phase_1.DAL.Models;

namespace phase_1.DAL.Configurations
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder
                .HasOne(c => c.Match)
                .WithOne(m => m.Conversation)
                .HasForeignKey<Conversation>(c => c.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .Property(c => c.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
