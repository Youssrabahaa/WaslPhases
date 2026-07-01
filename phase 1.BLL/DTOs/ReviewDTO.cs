using System.ComponentModel.DataAnnotations;

namespace phase_1.BLL.DTOs
{
   
    public class ReviewDTO
    {
        public int Id { get; set; }
        public int MatchId { get; set; }
        public int ReviewerUserId { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public int ReviewedUserId { get; set; }
        public string ReviewedName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}