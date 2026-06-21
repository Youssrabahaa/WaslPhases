using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace phase_1.Models;

public class PatientProfile
{
    [Key]
    public int UserId { get; set; }

    [Column(TypeName = "date")]
    public DateTime? BirthDate { get; set; }

    [MaxLength(20)]
    public string? Gender { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public User User { get; set; } = default!;
}
