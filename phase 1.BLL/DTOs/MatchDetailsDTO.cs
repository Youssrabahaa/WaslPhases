using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phase_1.BLL.DTOs
{
    public class MatchDetailsDTO : MatchDTO
    {
        public string PatientName { get; set; } = string.Empty;
        public string PatientPhone { get; set; } = string.Empty;

        public string StudentName { get; set; } = string.Empty;
        public string StudentPhone { get; set; } = string.Empty;

        public string CaseDescription { get; set; } = string.Empty;

        public string TreatmentCategory { get; set; } = string.Empty;

        public string Governorate { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;

        public decimal? AgreedPrice { get; set; }

        public int? SessionsCount { get; set; }

        public string? OfferMessage { get; set; }

        public int CompletedSessions { get; set; }

        public int RemainingSessions { get; set; }

        public List<MatchTimelineDto> Timeline { get; set; } = new();
    }
}
