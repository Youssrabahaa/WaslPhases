using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace phase_1.DAL.Models
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }

        // المستلم
        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; } = null!;

        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(500)]
        public string Message { get; set; } = string.Empty;

        // 1=OfferAccepted 2=OfferRejected 3=MatchCreated
        // 4=NewMessage 5=SessionScheduled 6=Reminder
        public int Type { get; set; }

        // مثلاً: MatchId أو OfferId أو SessionId
        public int? ReferenceId { get; set; }

        // "Match" / "Offer" / "Session"
        [MaxLength(50)]
        public string? ReferenceType { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
