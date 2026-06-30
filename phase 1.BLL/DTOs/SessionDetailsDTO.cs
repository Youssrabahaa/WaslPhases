using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phase_1.BLL.DTOs
{
    public class SessionDetailsDTO
    {
        public int Id { get; set; }

        public int MatchId { get; set; }

        public int Number { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public string? LocationText { get; set; }

        public string? ClinicRoom { get; set; }

        public int Status { get; set; }

        public string StatusText { get; set; } = "";

        public string PatientName { get; set; } = "";

        public string StudentName { get; set; } = "";

        public string CaseTitle { get; set; } = "";

        public bool HasReport { get; set; }

        public bool CanStart { get; set; }

        public bool CanFinish { get; set; }
        public NoShowStrikeDTO? NoShowStrike { get; set; }

        public int PatientStrikeCount { get; set; }

        public int StudentStrikeCount { get; set; }
        public int PatientUserId { get; set; }

        public int StudentUserId { get; set; }
    }
}
