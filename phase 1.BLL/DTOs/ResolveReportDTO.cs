using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phase_1.BLL.DTOs
{
    public class ResolveReportDTO
    {
        [Required]
        public int NewStatus { get; set; }   // 2=Resolved, 3=Rejected

        [MaxLength(1000)]
        public string? Notes { get; set; }
    }
}
