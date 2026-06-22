using System.ComponentModel.DataAnnotations;

namespace phase_1.DTOs;

public class RefreshTokenDTO
{
    [Required]
    public string RefreshToken { get; set; } = default!;
}
