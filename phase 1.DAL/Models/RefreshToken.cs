using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace phase_1.DAL.Models
{

    [Index(nameof(TokenHash), IsUnique = true)]
    [Index(nameof(UserId), nameof(ExpiresAt))]
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required, MaxLength(500)]
        public string TokenHash { get; set; } = default!;

        [Required]
        public DateTime ExpiresAt { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? CreatedByIp { get; set; }

        public DateTime? RevokedAt { get; set; }

        [MaxLength(100)]
        public string? RevokedByIp { get; set; }

        [MaxLength(500)]
        public string? ReplacedByTokenHash { get; set; }

        [MaxLength(250)]
        public string? RevocationReason { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        public bool IsRevoked => RevokedAt.HasValue;

        public bool IsActive => !IsExpired && !IsRevoked;

        public ApplicationUser User { get; set; } = default!;
    }
}
