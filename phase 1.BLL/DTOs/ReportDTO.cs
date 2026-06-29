using System.ComponentModel.DataAnnotations;
using phase_1.Models;

namespace phase_1.DTOs
{
    // الداتا اللي بتيجي من المستخدم عشان يعمل بلاغ على جلسة
    public class CreateReportDTO
    {
        [Required]
        public int SessionId { get; set; }

        [Required]
        public string ReportedUserId { get; set; } = null!;

        [Required]
        public ReportType Type { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = null!;
    }

    // الداتا اللي بترجع للعرض (للمستخدم العادي أو للأدمن)
    public class ReportDTO
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public string ReporterUserId { get; set; } = null!;
        public string ReporterName { get; set; } = null!;
        public string ReportedUserId { get; set; } = null!;
        public string ReportedName { get; set; } = null!;
        public ReportType Type { get; set; }
        public string Description { get; set; } = null!;
        public ReportStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }

    // الداتا اللي الأدمن بيبعتها عشان يقفل/يتخذ إجراء على بلاغ
    public class ResolveReportDTO
    {
        [Required]
        public AdminActionType ActionType { get; set; }

        [Required]
        public ReportStatus NewStatus { get; set; } // المتوقع: Resolved أو Rejected

        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
}
