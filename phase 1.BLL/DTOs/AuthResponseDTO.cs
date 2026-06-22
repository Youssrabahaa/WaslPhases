namespace phase_1.DTOs;

public class AuthResponseDTO
{
    public string AccessToken { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;

    public ProfileDTO Profile { get; set; } = default!;
}
