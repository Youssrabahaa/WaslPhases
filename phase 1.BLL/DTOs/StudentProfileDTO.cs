using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phase_1.BLL.DTOs
{
    public class StudentProfileDTO
    {
        public int UserId { get; set; }
        public int FacultyId { get; set; }
        public int AcademicYear { get; set; }
        public string? ClinicName { get; set; }
        public string? StudentCode { get; set; }
        public string? SupervisorName { get; set; }
        public int RequiredCasesCount { get; set; }
        public int CompletedCasesCount { get; set; }
        public bool IsVerified { get; set; }

        public List<string> Skills { get; set; } = new List<string>();
    }
}
