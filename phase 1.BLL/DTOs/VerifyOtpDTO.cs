using System.ComponentModel.DataAnnotations;

namespace phase_1.DTOs;

public class VerifyOtpDTO
{
    [Required, Phone, MaxLength(30)]
    public string Phone { get; set; } = default!;

    [Required, RegularExpression(@"^\d{4,8}$")]
    public string Code { get; set; } = default!;

    [Required]
    public int Purpose { get; set; }
}
