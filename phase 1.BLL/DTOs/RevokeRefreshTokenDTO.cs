using System.ComponentModel.DataAnnotations;

namespace phase_1.DTOs;

public class RevokeRefreshTokenDTO
{
    [Required]
    public string RefreshToken { get; set; } = default!;

    [MaxLength(250)]
    public string? Reason { get; set; }
}
