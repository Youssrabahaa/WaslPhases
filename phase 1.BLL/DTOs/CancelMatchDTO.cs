using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phase_1.BLL.DTOs
{
    public class CancelMatchDto
    {
        public int UserId { get; set; }

        public string? Reason { get; set; }
    }
}
