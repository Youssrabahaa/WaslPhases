using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace phase_1.Models;

[Index(nameof(MatchId), nameof(ReviewerUserId), nameof(ReviewedUserId), IsUnique = true)]
public class Review
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int MatchId { get; set; }

    [Required]
    public int ReviewerUserId { get; set; }

    [Required]
    public int ReviewedUserId { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }

    [MaxLength(2000)]
    public string? Comment { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Match Match { get; set; } = default!;

    public User ReviewerUser { get; set; } = default!;

    public User ReviewedUser { get; set; } = default!;
}
