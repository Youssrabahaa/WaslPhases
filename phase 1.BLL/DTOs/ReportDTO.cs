using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phase_1.BLL.DTOs
{
    public class ReportDTO
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int ReporterUserId { get; set; }
        public string ReporterName { get; set; } = string.Empty;
        public int ReportedUserId { get; set; }
        public string ReportedName { get; set; } = string.Empty;
        public int Type { get; set; }
        public string Description { get; set; } = null!;
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }
}
