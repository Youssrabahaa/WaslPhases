using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace phase_1.Models;

[Index(nameof(SessionId), nameof(ScheduledAt))]
public class Reminder
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int SessionId { get; set; }

    [Required]
    public int Channel { get; set; } = 1;

    [Required]
    public DateTime ScheduledAt { get; set; }

    public DateTime? SentAt { get; set; }

    [Required]
    public int Status { get; set; } = 1;

    [MaxLength(120)]
    public string? ProviderMessageId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Session Session { get; set; } = default!;
}
