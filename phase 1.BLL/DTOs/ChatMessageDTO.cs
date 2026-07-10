using System;

namespace phase_1.BLL.DTOs
{
    public class ChatMessageDTO
    {
        public int Id { get; set; }
        public int SenderUserId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public bool IsOwn { get; set; }
    }
}