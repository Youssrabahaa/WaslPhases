using System.ComponentModel.DataAnnotations;

namespace phase_1.DTOs;

public class RegisterDTO : IValidatableObject
{
    [Required(ErrorMessage = "\u0627\u0643\u062a\u0628 \u0627\u0644\u0627\u0633\u0645 \u0628\u0627\u0644\u0643\u0627\u0645\u0644.")]
    [MaxLength(150, ErrorMessage = "\u0627\u0644\u0627\u0633\u0645 \u0644\u0627 \u064a\u0632\u064a\u062f \u0639\u0646 150 \u062d\u0631\u0641.")]
    public string FullName { get; set; } = default!;

    [Required(ErrorMessage = "\u0627\u0643\u062a\u0628 \u0631\u0642\u0645 \u0627\u0644\u0647\u0627\u062a\u0641.")]
    [Phone(ErrorMessage = "\u0627\u0643\u062a\u0628 \u0631\u0642\u0645 \u0647\u0627\u062a\u0641 \u0635\u062d\u064a\u062d.")]
    [MaxLength(30, ErrorMessage = "\u0631\u0642\u0645 \u0627\u0644\u0647\u0627\u062a\u0641 \u0644\u0627 \u064a\u0632\u064a\u062f \u0639\u0646 30 \u0631\u0642\u0645.")]
    public string Phone { get; set; } = default!;

    [EmailAddress(ErrorMessage = "\u0627\u0643\u062a\u0628 \u0628\u0631\u064a\u062f \u0625\u0644\u0643\u062a\u0631\u0648\u0646\u064a \u0635\u062d\u064a\u062d.")]
    [MaxLength(200, ErrorMessage = "\u0627\u0644\u0628\u0631\u064a\u062f \u0627\u0644\u0625\u0644\u0643\u062a\u0631\u0648\u0646\u064a \u0644\u0627 \u064a\u0632\u064a\u062f \u0639\u0646 200 \u062d\u0631\u0641.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "\u0627\u0643\u062a\u0628 \u0643\u0644\u0645\u0629 \u0627\u0644\u0645\u0631\u0648\u0631.")]
    [MinLength(8, ErrorMessage = "\u0643\u0644\u0645\u0629 \u0627\u0644\u0645\u0631\u0648\u0631 \u0644\u0627\u0632\u0645 \u062a\u0643\u0648\u0646 8 \u0623\u062d\u0631\u0641 \u0639\u0644\u0649 \u0627\u0644\u0623\u0642\u0644.")]
    [MaxLength(128, ErrorMessage = "\u0643\u0644\u0645\u0629 \u0627\u0644\u0645\u0631\u0648\u0631 \u0644\u0627 \u062a\u0632\u064a\u062f \u0639\u0646 128 \u062d\u0631\u0641.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [Required(ErrorMessage = "\u0623\u0639\u062f \u0643\u062a\u0627\u0628\u0629 \u0643\u0644\u0645\u0629 \u0627\u0644\u0645\u0631\u0648\u0631 \u0644\u0644\u062a\u0623\u0643\u064a\u062f.")]
    [Compare(nameof(Password), ErrorMessage = "\u062a\u0623\u0643\u064a\u062f \u0643\u0644\u0645\u0629 \u0627\u0644\u0645\u0631\u0648\u0631 \u063a\u064a\u0631 \u0645\u0637\u0627\u0628\u0642.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = default!;

    [Range(1, 2, ErrorMessage = "\u0627\u062e\u062a\u0631 \u0646\u0648\u0639 \u0627\u0644\u062d\u0633\u0627\u0628.")]
    public int Role { get; set; }

    public DateTime? BirthDate { get; set; }

    [MaxLength(20, ErrorMessage = "\u0627\u0644\u0646\u0648\u0639 \u0644\u0627 \u064a\u0632\u064a\u062f \u0639\u0646 20 \u062d\u0631\u0641.")]
    public string? Gender { get; set; }

    [MaxLength(500, ErrorMessage = "\u0627\u0644\u0645\u0644\u0627\u062d\u0638\u0627\u062a \u0644\u0627 \u062a\u0632\u064a\u062f \u0639\u0646 500 \u062d\u0631\u0641.")]
    public string? Notes { get; set; }

    public int? FacultyId { get; set; }

    [Range(1, 10, ErrorMessage = "\u0627\u0644\u0633\u0646\u0629 \u0627\u0644\u062f\u0631\u0627\u0633\u064a\u0629 \u0644\u0627\u0632\u0645 \u062a\u0643\u0648\u0646 \u0628\u064a\u0646 1 \u0648 10.")]
    public int? AcademicYear { get; set; }

    [MaxLength(200, ErrorMessage = "\u0627\u0633\u0645 \u0627\u0644\u0639\u064a\u0627\u062f\u0629 \u0644\u0627 \u064a\u0632\u064a\u062f \u0639\u0646 200 \u062d\u0631\u0641.")]
    public string? ClinicName { get; set; }

    [MaxLength(100, ErrorMessage = "\u0643\u0648\u062f \u0627\u0644\u0637\u0627\u0644\u0628 \u0644\u0627 \u064a\u0632\u064a\u062f \u0639\u0646 100 \u062d\u0631\u0641.")]
    public string? StudentCode { get; set; }

    [MaxLength(150, ErrorMessage = "\u0627\u0633\u0645 \u0627\u0644\u0645\u0634\u0631\u0641 \u0644\u0627 \u064a\u0632\u064a\u062f \u0639\u0646 150 \u062d\u0631\u0641.")]
    public string? SupervisorName { get; set; }

    [Range(0, 100, ErrorMessage = "\u0639\u062f\u062f \u0627\u0644\u062d\u0627\u0644\u0627\u062a \u0627\u0644\u0645\u0637\u0644\u0648\u0628\u0629 \u0644\u0627\u0632\u0645 \u064a\u0643\u0648\u0646 \u0628\u064a\u0646 0 \u0648 100.")]
    public int? RequiredCasesCount { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Role == 2)
        {
            if (!FacultyId.HasValue)
                yield return new ValidationResult("\u0627\u062e\u062a\u0631 \u0627\u0644\u0643\u0644\u064a\u0629.", new[] { nameof(FacultyId) });

            if (!AcademicYear.HasValue)
                yield return new ValidationResult("\u0627\u0643\u062a\u0628 \u0627\u0644\u0633\u0646\u0629 \u0627\u0644\u062f\u0631\u0627\u0633\u064a\u0629.", new[] { nameof(AcademicYear) });

            if (string.IsNullOrWhiteSpace(StudentCode))
                yield return new ValidationResult("\u0627\u0643\u062a\u0628 \u0643\u0648\u062f \u0627\u0644\u0637\u0627\u0644\u0628.", new[] { nameof(StudentCode) });
        }
    }
}
