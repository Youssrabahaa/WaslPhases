using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace phase_1.Models;

[Index(nameof(UserId), nameof(CreatedAt))]
[Index(nameof(SessionId), nameof(UserId), IsUnique = true)]
public class NoShowStrike
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int SessionId { get; set; }

    [Required]
    public int Points { get; set; } = 1;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ExpiresAt { get; set; }

    [MaxLength(400)]
    public string? Notes { get; set; }

    public User User { get; set; } = default!;

    public Session Session { get; set; } = default!;
}
