using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace phase_1.Models;

[Index(nameof(OfferId), IsUnique = true)]
public class Match
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CaseId { get; set; }

    [Required]
    public int OfferId { get; set; }

    [Required]
    public int PatientUserId { get; set; }

    [Required]
    public int StudentUserId { get; set; }

    [Required]
    public int Status { get; set; } = 1;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? AcceptedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public Case Case { get; set; } = default!;

    public Offer Offer { get; set; } = default!;

    public User PatientUser { get; set; } = default!;

    public User StudentUser { get; set; } = default!;

    // Conversation removed — use phone calls. If true, contact via phone only.
    public bool PhoneContactOnly { get; set; } = true;
    public List<Session> Sessions { get; set; } = new List<Session>();
    public List<Review> Reviews { get; set; } = new List<Review>();
}
