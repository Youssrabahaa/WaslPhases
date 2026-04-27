using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace phase_1.Models;

[Index(nameof(Name), IsUnique = true)]
public class TreatmentCategory
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = default!;

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

    public List<Case> Cases { get; set; } = new List<Case>();
}
