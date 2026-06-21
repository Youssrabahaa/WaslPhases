using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace phase_1.Models;

[Index(nameof(Name), IsUnique = true)]
public class ServiceType
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(80)]
    public string Name { get; set; } = default!;

    [Required]
    public bool IsActive { get; set; } = true;

    public List<Case> Cases { get; set; } = new List<Case>();
}
