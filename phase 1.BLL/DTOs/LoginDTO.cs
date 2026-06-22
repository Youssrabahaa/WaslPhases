using System.ComponentModel.DataAnnotations;

namespace phase_1.DTOs;

public class LoginDTO
{
    [Required, Phone, MaxLength(30)]
    public string Phone { get; set; } = default!;

    [Required, MinLength(8), MaxLength(128)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    public bool RememberMe { get; set; }
}
