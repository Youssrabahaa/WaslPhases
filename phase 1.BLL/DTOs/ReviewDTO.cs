using System.ComponentModel.DataAnnotations;

namespace phase_1.DTOs
{
    // الداتا اللي بتيجي من المستخدم عشان يعمل ريفيو
    public class CreateReviewDTO
    {
        [Required]
        public int MatchId { get; set; }

        [Required]
        public string RevieweeId { get; set; } = null!;

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }
    }

    // الداتا اللي بترجع للعرض
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public string ReviewerId { get; set; } = null!;
        public string ReviewerName { get; set; } = null!;
        public string RevieweeId { get; set; } = null!;
        public string RevieweeName { get; set; } = null!;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
