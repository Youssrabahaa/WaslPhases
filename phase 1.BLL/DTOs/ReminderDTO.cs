using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phase_1.BLL.DTOs
{
    public class ReminderDTO
    {
        public int Id { get; set; }

        public DateTime ReminderTime { get; set; }

        public bool IsSent { get; set; }
    }
}
