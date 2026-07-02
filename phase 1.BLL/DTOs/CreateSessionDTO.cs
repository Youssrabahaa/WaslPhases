namespace phase_1.BLL.DTOs
{
    public class CreateSessionDTO
    {
        public int MatchId { get; set; }

        public int Number { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public string? LocationText { get; set; }

        public string? ClinicRoom { get; set; }
    }
}