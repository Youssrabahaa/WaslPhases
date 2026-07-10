using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phase_1.BLL.DTOs
{
    public class CreateReportDTO
    {
        [Required]
        public int SessionId { get; set; }

        [Required]
        public int ReportedUserId { get; set; }

        [Required]
        public int Type { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = null!;
    }
}
