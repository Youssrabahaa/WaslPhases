using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace phase_1.Models;

[Index(nameof(Name), IsUnique = true)]
public class University
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = default!;

    [Required, MaxLength(100)]
    public string Governorate { get; set; } = default!;

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<Faculty> Faculties { get; set; } = new();
}
