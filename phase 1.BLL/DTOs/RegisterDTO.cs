using System.ComponentModel.DataAnnotations;

namespace phase_1.DTOs;

public class RegisterDTO : IValidatableObject
{
    [Required, MaxLength(150)]
    public string FullName { get; set; } = default!;

    [Required, Phone, MaxLength(30)]
    public string Phone { get; set; } = default!;

    [EmailAddress, MaxLength(200)]
    public string? Email { get; set; }

    [Required, MinLength(8), MaxLength(128)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [Required, Compare(nameof(Password))]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = default!;

    [Required]
    public int Role { get; set; }

    public DateTime? BirthDate { get; set; }

    [MaxLength(20)]
    public string? Gender { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }

    public int? FacultyId { get; set; }

    [Range(1, 10)]
    public int? AcademicYear { get; set; }

    [MaxLength(200)]
    public string? ClinicName { get; set; }

    [MaxLength(100)]
    public string? StudentCode { get; set; }

    [MaxLength(150)]
    public string? SupervisorName { get; set; }

    [Range(0, 100)]
    public int? RequiredCasesCount { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Role == 2)
        {
            if (!FacultyId.HasValue)
                yield return new ValidationResult("FacultyId is required for student registration.", new[] { nameof(FacultyId) });

            if (!AcademicYear.HasValue)
                yield return new ValidationResult("AcademicYear is required for student registration.", new[] { nameof(AcademicYear) });

            if (string.IsNullOrWhiteSpace(StudentCode))
                yield return new ValidationResult("StudentCode is required for student registration.", new[] { nameof(StudentCode) });
        }
    }
}
