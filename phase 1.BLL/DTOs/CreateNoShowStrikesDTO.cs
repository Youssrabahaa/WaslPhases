using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phase_1.BLL.DTOs
{
    public class CreateNoShowStrikeDTO
    {
        public int SessionId { get; set; }

        public int ReportedByUserId { get; set; }
        public int AbsentUserId { get; set; }

    }

}
