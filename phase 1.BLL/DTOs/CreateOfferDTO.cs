namespace phase_1.BLL.DTOs;

public class CreateOfferDTO
{
    public int CaseId { get; set; }
    public int StudentUserId { get; set; }
    public string? Message { get; set; }
    public decimal? ProposedPrice { get; set; }
    public int? EstimatedSessionsCount { get; set; }
}
