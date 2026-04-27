using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace phase_1.Models;

[Index(nameof(StudentCode), IsUnique = true)]
public class StudentProfile
{
    [Key]
    public int UserId { get; set; }

    [Required]
    public int FacultyId { get; set; }

    [Required]
    public int AcademicYear { get; set; }

    [MaxLength(200)]
    public string? ClinicName { get; set; }

    [MaxLength(100)]
    public string? StudentCode { get; set; }

    [MaxLength(150)]
    public string? SupervisorName { get; set; }

    [Required]
    public int RequiredCasesCount { get; set; }

    [Required]
    public int CompletedCasesCount { get; set; }

    [Required]
    public bool IsVerified { get; set; }

    public DateTime? VerifiedAt { get; set; }

    public User User { get; set; } = default!;
    public Faculty Faculty { get; set; } = default!;
}
