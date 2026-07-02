using System;

namespace phase_1.BLL.DTOs
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int Type { get; set; }
        public int? ReferenceId { get; set; }
        public string? ReferenceType { get; set; }
        public bool IsRead { get; set; }
        public string TimeAgo { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}