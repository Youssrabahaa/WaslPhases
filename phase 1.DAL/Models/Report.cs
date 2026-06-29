//using Microsoft.EntityFrameworkCore;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace phase_1.Models;

//[Index(nameof(Status), nameof(CreatedAt))]
//public class Report
//{
//    [Key]
//    public int Id { get; set; }

//    [Required]
//    public int ReporterId { get; set; }

//    [Required]
//    public int ReportedId { get; set; }

//    [Required]
//    public int TargetType { get; set; }

//    [Required]
//    public int TargetId { get; set; }

//    [Required, MaxLength(500)]
//    public string Reason { get; set; } = default!;

//    [Required]
//    public int Status { get; set; } = 1;

//    public int? HandledByAdminId { get; set; }

//    [Required]
//    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

//    public DateTime? ResolvedAt { get; set; }

//    [ForeignKey(nameof(ReporterId))]
//    public ApplicationUser Reporter { get; set; } = default!;

//    [ForeignKey(nameof(ReportedId))]
//    public ApplicationUser Reported { get; set; } = default!;

//    public ApplicationUser? HandledByAdmin { get; set; }
//}


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace phase_1.Models
{
    // بلاغ/شكوى من مستخدم (Patient أو Student) حول جلسة معينة
    // الأدمن هو الوحيد المسؤول عن مراجعته واتخاذ إجراء (AdminAction)
    public class Report
    {
        [Key]
        public int Id { get; set; }

        // الجلسة اللي البلاغ بسببها (مطلوبة دايماً حسب اتفاقنا)
        [Required]
        public int SessionId { get; set; }

        [ForeignKey(nameof(SessionId))]
        public Session Session { get; set; } = null!;

        // مين اللي بلّغ
        [Required]
        public string ReporterUserId { get; set; } = null!;

        [ForeignKey(nameof(ReporterUserId))]
        public ApplicationUser ReporterUser { get; set; } = null!;

        // مين المُبلَّغ عنه
        [Required]
        public string ReportedUserId { get; set; } = null!;

        [ForeignKey(nameof(ReportedUserId))]
        public ApplicationUser ReportedUser { get; set; } = null!;

        [Required]
        public ReportType Type { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = null!;

        [Required]
        public ReportStatus Status { get; set; } = ReportStatus.Open;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // بيتعبّى وقت ما الأدمن يقفل البلاغ (Resolved/Rejected)
        public DateTime? ResolvedAt { get; set; }

    }
}
