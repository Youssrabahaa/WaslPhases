using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace phase_1.Models;

[Index(nameof(CaseId), nameof(StudentUserId), IsUnique = true)]
public class Offer
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int CaseId { get; set; }

    [Required]
    public int StudentUserId { get; set; }

    [MaxLength(1500)]
    public string? Message { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? ProposedPrice { get; set; }

    public int? EstimatedSessionsCount { get; set; }

    [Required]
    public int Status { get; set; } = 1;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? DecidedAt { get; set; }

    public Case Case { get; set; } = default!;

    public User StudentUser { get; set; } = default!;

    public Match? Match { get; set; }
}
