using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace phase_1.Models;

[Index(nameof(Phone), IsUnique = true)]
public class User
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(30)]
    public string Phone { get; set; } = default!;

    [Required, MaxLength(150)]
    public string FullName { get; set; } = default!;

    [MaxLength(200)]
    public string? Email { get; set; }

    [Required]
    public int Role { get; set; }

    [Required]
    public int Status { get; set; } = 1;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? LastLoginAt { get; set; }

    public PatientProfile? PatientProfile { get; set; }
    public StudentProfile? StudentProfile { get; set; }
    public List<OtpCode> OtpCodes { get; set; } = new();
    public List<Case> CasesPosted { get; set; } = new();
    public List<Offer> OffersMade { get; set; } = new();
    public List<Match> PatientMatches { get; set; } = new();
    public List<Match> StudentMatches { get; set; } = new();
    // Messages/chat removed; phone-call flow used instead.
    public List<Review> ReviewsWritten { get; set; } = new();
    public List<Review> ReviewsReceived { get; set; } = new();
    public ICollection<Report> ReportsMade { get; set; } = new List<Report>();
    public ICollection<Report> ReportsReceived { get; set; } = new List<Report>();
    public ICollection<Report> ReportsHandled { get; set; } = new List<Report>();
    public List<NoShowStrike> NoShowStrikes { get; set; } = new();
    public List<Session> SessionsSupervised { get; set; } = new();
    public List<Session> SessionsCancelled { get; set; } = new();
}
