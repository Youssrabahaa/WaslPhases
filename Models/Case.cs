using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace phase_1.Models;

[Index(nameof(Status), nameof(Governorate), nameof(City), nameof(ServiceTypeId), nameof(CreatedAt))]
public class Case
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int PatientUserId { get; set; }

    [Required]
    public int ServiceTypeId { get; set; }

    [Required]
    public int TreatmentCategoryId { get; set; }

    [Required, MaxLength(120)]
    public string Title { get; set; } = default!;

    [Required, MaxLength(2000)]
    public string Description { get; set; } = default!;

    [Required]
    public int Urgency { get; set; } = 2;

    [Required]
    public int Status { get; set; } = 1;

    public decimal? EstimatedPriceMin { get; set; }
    public decimal? EstimatedPriceMax { get; set; }

    [Required]
    public bool NeedsSupervisorApproval { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ClosedAt { get; set; }

    [Required, MaxLength(100)]
    public string Governorate { get; set; } = default!;

    [Required, MaxLength(100)]
    public string City { get; set; } = default!;

    [Required, MaxLength(100)]
    public string Area { get; set; } = default!;

    public User PatientUser { get; set; } = default!;
    public ServiceType ServiceType { get; set; } = default!;
    public TreatmentCategory TreatmentCategory { get; set; } = default!;
    public List<Offer> Offers { get; set; } = new();
    public List<Match> Matches { get; set; } = new();
}
