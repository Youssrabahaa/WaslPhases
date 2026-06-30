using System;
using System.Collections.Generic;

namespace phase_1.BLL.DTOs
{
    public class ConversationDTO
    {
        public int ConversationId { get; set; }
        public int MatchId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public string StudentName { get; set; } = string.Empty;
        public int CurrentUserId { get; set; }
        public List<ChatMessageDTO> Messages { get; set; } = new();
    }
}
