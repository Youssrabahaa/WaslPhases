using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phase_1.BLL.DTOs
{
    public class NoShowStrikeDTO
    {
        public int Id { get; set; }

        public int SessionId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; } = "";

        public string WhoMissed { get; set; } = "";

        public DateTime CreatedAt { get; set; }
    }
}
