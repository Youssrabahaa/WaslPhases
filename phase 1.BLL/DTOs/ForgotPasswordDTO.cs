using System.ComponentModel.DataAnnotations;

namespace phase_1.DTOs;

public class ForgotPasswordDTO
{
    [Required, Phone, MaxLength(30)]
    public string Phone { get; set; } = default!;
}
