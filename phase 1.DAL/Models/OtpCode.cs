using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace phase_1.Models;

[Index(nameof(Phone))]
[Index(nameof(ExpiresAt))]
public class OtpCode
{
    [Key]
    public int Id { get; set; }

    public int? UserId { get; set; }

    [Required, MaxLength(30)]
    public string Phone { get; set; } = default!;

    [Required, MaxLength(200)]
    public string CodeHash { get; set; } = default!;

    [Required]
    public DateTime ExpiresAt { get; set; }

    public DateTime? UsedAt { get; set; }

    [Required]
    public int Attempts { get; set; }

    [Required]
    public int Purpose { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User? User { get; set; }
}
