using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace phase_1.DAL.Models
{
    [Index(nameof(Status), nameof(CreatedAt))]
    public class Report
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SessionId { get; set; }

        [ForeignKey(nameof(SessionId))]
        public Session Session { get; set; } = null!;

        [Required]
        public int ReporterUserId { get; set; }

        [ForeignKey(nameof(ReporterUserId))]
        public ApplicationUser ReporterUser { get; set; } = null!;

        [Required]
        public int ReportedUserId { get; set; }

        [ForeignKey(nameof(ReportedUserId))]
        public ApplicationUser ReportedUser { get; set; } = null!;

        [Required]
        public int Type { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = null!;

        [Required]
        public int Status { get; set; } = 1;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ResolvedAt { get; set; }
    }
}