using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phase_1.BLL.DTOs
{
    public class MatchDTO
    {
        public int MatchId { get; set; }

        public int CaseId { get; set; }

        public string CaseTitle { get; set; } = string.Empty;

        public int Status { get; set; }

        public DateTime? AcceptedAt { get; set; }
    }
}
