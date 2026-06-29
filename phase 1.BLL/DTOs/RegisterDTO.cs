using System.ComponentModel.DataAnnotations;

namespace phase_1.DTOs;

public class RegisterDTO : IValidatableObject
{
    [Required(ErrorMessage = "اكتب الاسم بالكامل.")]
    [MaxLength(150, ErrorMessage = "الاسم لا يزيد عن 150 حرف.")]
    public string FullName { get; set; } = default!;

    [Required(ErrorMessage = "اكتب رقم الهاتف.")]
    [Phone(ErrorMessage = "اكتب رقم هاتف صحيح.")]
    [MaxLength(30, ErrorMessage = "رقم الهاتف لا يزيد عن 30 رقم.")]
    public string Phone { get; set; } = default!;

    [EmailAddress(ErrorMessage = "اكتب بريد إلكتروني صحيح.")]
    [MaxLength(200, ErrorMessage = "البريد الإلكتروني لا يزيد عن 200 حرف.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "اكتب كلمة المرور.")]
    [MinLength(8, ErrorMessage = "كلمة المرور لازم تكون 8 أحرف على الأقل.")]
    [MaxLength(128, ErrorMessage = "كلمة المرور لا تزيد عن 128 حرف.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [Required(ErrorMessage = "أعد كتابة كلمة المرور للتأكيد.")]
    [Compare(nameof(Password), ErrorMessage = "تأكيد كلمة المرور غير مطابق.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = default!;

    [Range(1, 2, ErrorMessage = "اختر نوع الحساب.")]
    public int Role { get; set; }

    public DateTime? BirthDate { get; set; }

    [MaxLength(20, ErrorMessage = "النوع لا يزيد عن 20 حرف.")]
    public string? Gender { get; set; }

    [MaxLength(500, ErrorMessage = "الملاحظات لا تزيد عن 500 حرف.")]
    public string? Notes { get; set; }

    public int? FacultyId { get; set; }

    [Range(1, 10, ErrorMessage = "السنة الدراسية لازم تكون بين 1 و 10.")]
    public int? AcademicYear { get; set; }

    [MaxLength(200, ErrorMessage = "اسم العيادة لا يزيد عن 200 حرف.")]
    public string? ClinicName { get; set; }

    [MaxLength(100, ErrorMessage = "كود الطالب لا يزيد عن 100 حرف.")]
    public string? StudentCode { get; set; }

    [MaxLength(150, ErrorMessage = "اسم المشرف لا يزيد عن 150 حرف.")]
    public string? SupervisorName { get; set; }

    [Range(0, 100, ErrorMessage = "عدد الحالات المطلوبة لازم يكون بين 0 و 100.")]
    public int? RequiredCasesCount { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Role == 2)
        {
            if (!FacultyId.HasValue)
                yield return new ValidationResult("اختر الكلية.", new[] { nameof(FacultyId) });

            if (!AcademicYear.HasValue)
                yield return new ValidationResult("اكتب السنة الدراسية.", new[] { nameof(AcademicYear) });

            if (string.IsNullOrWhiteSpace(StudentCode))
                yield return new ValidationResult("اكتب كود الطالب.", new[] { nameof(StudentCode) });
        }
    }
}
