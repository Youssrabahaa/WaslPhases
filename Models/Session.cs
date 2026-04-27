using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace phase_1.Models;

[Index(nameof(MatchId), nameof(Number), IsUnique = true)]
[Index(nameof(MatchId), nameof(StartAt))]
public class Session
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int MatchId { get; set; }

    [Required]
    public int Number { get; set; } = 1;

    [Required]
    public DateTime StartAt { get; set; }

    [Required]
    public DateTime EndAt { get; set; }

    [MaxLength(300)]
    public string? LocationText { get; set; }

    [MaxLength(150)]
    public string? ClinicRoom { get; set; }

    public int? SupervisorUserId { get; set; }

    [Required]
    public int Status { get; set; } = 1;

    public int? CancelledByUserId { get; set; }

    [MaxLength(400)]
    public string? CancelReason { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public Match Match { get; set; } = default!;

    public User? SupervisorUser { get; set; }

    public User? CancelledByUser { get; set; }

    public List<Reminder> Reminders { get; set; } = new List<Reminder>();
    public NoShowStrike? NoShowStrike { get; set; }
}
