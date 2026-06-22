using System.ComponentModel.DataAnnotations;

namespace phase_1.DTOs;

public class ResetPasswordDTO
{
    [Required, Phone, MaxLength(30)]
    public string Phone { get; set; } = default!;

    [Required, RegularExpression(@"^\d{4,8}$")]
    public string Code { get; set; } = default!;

    [Required, MinLength(8), MaxLength(128)]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = default!;

    [Required, Compare(nameof(NewPassword))]
    [DataType(DataType.Password)]
    public string ConfirmNewPassword { get; set; } = default!;
}
