using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace phase_1.DAL.Models;

[Index(nameof(Phone), IsUnique = true)]
public class ApplicationUser
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(30)]
    public string Phone { get; set; } = default!;

    [Required, MaxLength(150)]
    public string FullName { get; set; } = default!;

    [MaxLength(200)]
    public string? Email { get; set; }

    [Required, MaxLength(500)]
    public string PasswordHash { get; set; } = default!;

    [Required]
    public int Role { get; set; }

    [Required]
    public int Status { get; set; } = 1;

    [Required]
    public bool IsPhoneVerified { get; set; }

    [Required]
    public bool IsEmailVerified { get; set; }

    public DateTime? LockedUntil { get; set; }

    [Required]
    public int FailedLoginAttempts { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public PatientProfile? PatientProfile { get; set; }
    public StudentProfile? StudentProfile { get; set; }
    public List<OtpCode> OtpCodes { get; set; } = new();
    public List<RefreshToken> RefreshTokens { get; set; } = new();
    public List<Case> CasesPosted { get; set; } = new();
    public List<Offer> OffersMade { get; set; } = new();
    public List<Match> PatientMatches { get; set; } = new();
    public List<Match> StudentMatches { get; set; } = new();
    public List<Message> MessagesSent { get; set; } = new();
    public List<Message> MessagesReceived { get; set; } = new();
    public List<Review> ReviewsWritten { get; set; } = new();
    public List<Review> ReviewsReceived { get; set; } = new();
    public ICollection<Report> ReportsMade { get; set; } = new List<Report>();
    public ICollection<Report> ReportsReceived { get; set; } = new List<Report>();
    public List<NoShowStrike> NoShowStrikes { get; set; } = new();
    public List<Session> SessionsSupervised { get; set; } = new();
    public List<Session> SessionsCancelled { get; set; } = new();
}
