using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace phase_1.DAL.Models
{
    [Index(nameof(MatchId), nameof(ReviewerUserId), nameof(ReviewedUserId), IsUnique = true)]
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MatchId { get; set; }
        public Match Match { get; set; } = null!;

        [Required]
        public int ReviewerUserId { get; set; }
        public ApplicationUser ReviewerUser { get; set; } = null!;

        [Required]
        public int ReviewedUserId { get; set; }
        public ApplicationUser ReviewedUser { get; set; } = null!;

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}