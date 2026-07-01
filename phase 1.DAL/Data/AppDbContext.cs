using Microsoft.EntityFrameworkCore;
using phase_1.DAL.Models;
using phase_1.SeedData;

namespace phase_1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; } = null!;
        public DbSet<OtpCode> OtpCodes { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;
        public DbSet<PatientProfile> PatientProfiles { get; set; } = null!;
        public DbSet<StudentProfile> StudentProfiles { get; set; } = null!;
        public DbSet<University> Universities { get; set; } = null!;
        public DbSet<Faculty> Faculties { get; set; } = null!;
        public DbSet<ServiceType> ServiceTypes { get; set; } = null!;
        public DbSet<TreatmentCategory> TreatmentCategories { get; set; } = null!;
        public DbSet<Case> Cases { get; set; } = null!;
        public DbSet<Offer> Offers { get; set; } = null!;
        public DbSet<Match> Matches { get; set; } = null!;
        public DbSet<Session> Sessions { get; set; } = null!;
        public DbSet<Reminder> Reminders { get; set; } = null!;
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<NoShowStrike> NoShowStrikes { get; set; } = null!;
        public DbSet<Report> Reports { get; set; } = null!;
        public DbSet<Conversation> Conversations { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.ApplyAppSeeds();
        }
    }
}
