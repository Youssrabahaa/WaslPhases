using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phase_1.BLL.DTOs
{
    public class SessionDTO
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

        public bool HasReport { get; set; }
    }
}
