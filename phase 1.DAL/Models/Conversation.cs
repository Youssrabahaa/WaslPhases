using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace phase_1.DAL.Models
{
    public class Conversation
    {
        public int Id { get; set; }

        public int MatchId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? LastMessageAt { get; set; }

        public Match Match { get; set; } = default!;

        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public bool IsClosed { get; set; } = false;
    }
}
