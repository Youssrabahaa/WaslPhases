//using System.ComponentModel.DataAnnotations;
//using Microsoft.EntityFrameworkCore;

//namespace phase_1.Models;

//[Index(nameof(MatchId), nameof(ReviewerUserId), nameof(ReviewedUserId), IsUnique = true)]
//public class Review
//{
//    [Key]
//    public int Id { get; set; }

//    [Required]
//    public int MatchId { get; set; }

//    [Required]
//    public int ReviewerUserId { get; set; }

//    [Required]
//    public int ReviewedUserId { get; set; }

//    [Range(1, 5)]
//    public int Rating { get; set; }

//    [MaxLength(2000)]
//    public string? Comment { get; set; }

//    [Required]
//    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

//    public Match Match { get; set; } = default!;

//    public ApplicationUser ReviewerUser { get; set; } = default!;

//    public ApplicationUser ReviewedUser { get; set; } = default!;
//}


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace phase_1.Models
{
    // مراجعة/تقييم بيتعمل بعد ما الـ Match يخلص (Completed)
    // الـ Rating هنا حقل جوه الريفيو نفسه (مفيش Model مستقل للـ Rating)
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MatchId { get; set; }

        [ForeignKey(nameof(MatchId))]
        public Match Match { get; set; } = null!;

        // الشخص اللي كتب الريفيو (Patient أو Student)
        [Required]
        public string ReviewerId { get; set; } = null!;

        [ForeignKey(nameof(ReviewerId))]
        public ApplicationUser Reviewer { get; set; } = null!;

        // الشخص اللي بيتم تقييمه
        [Required]
        public string RevieweeId { get; set; } = null!;

        [ForeignKey(nameof(RevieweeId))]
        public ApplicationUser Reviewee { get; set; } = null!;

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
