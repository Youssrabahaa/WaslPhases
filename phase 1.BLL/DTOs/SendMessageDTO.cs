namespace phase_1.BLL.DTOs
{
    public class SendMessageDTO
    {
        public int MatchId { get; set; }
        public int SenderUserId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}