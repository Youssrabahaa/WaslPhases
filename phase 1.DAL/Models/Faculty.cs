using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace phase_1.DAL.Models
{

    [Index(nameof(UniversityId), nameof(Name), IsUnique = true)]
public class Faculty
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = default!;

    [MaxLength(150)]
    public string? Location { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    [Required]
    public int UniversityId { get; set; }

    public University University { get; set; } = default!;

    public List<StudentProfile> StudentProfiles { get; set; } = new();
}
}
