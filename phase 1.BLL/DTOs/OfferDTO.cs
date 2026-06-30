using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phase_1.BLL.DTOs
{
    public class OfferDTO
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public int StudentUserId { get; set; }
        public string? Message { get; set; }
        public decimal? ProposedPrice { get; set; }
        public int? EstimatedSessionsCount { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
