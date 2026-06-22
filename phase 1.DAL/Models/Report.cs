using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace phase_1.Models;

[Index(nameof(Status), nameof(CreatedAt))]
public class Report
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int ReporterId { get; set; }

    [Required]
    public int ReportedId { get; set; }

    [Required]
    public int TargetType { get; set; }

    [Required]
    public int TargetId { get; set; }

    [Required, MaxLength(500)]
    public string Reason { get; set; } = default!;

    [Required]
    public int Status { get; set; } = 1;

    public int? HandledByAdminId { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ResolvedAt { get; set; }

    [ForeignKey(nameof(ReporterId))]
    public ApplicationUser Reporter { get; set; } = default!;

    [ForeignKey(nameof(ReportedId))]
    public ApplicationUser Reported { get; set; } = default!;

    public ApplicationUser? HandledByAdmin { get; set; }
}
