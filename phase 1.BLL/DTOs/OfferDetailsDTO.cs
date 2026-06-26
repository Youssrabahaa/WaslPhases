namespace phase_1.BLL.DTOs;

public class OfferDetailsDTO
{
    public int Id { get; set; }
    public int CaseId { get; set; }
    public string? CaseTitle { get; set; } 
    public int StudentUserId { get; set; }
    public string? StudentName { get; set; } 
    public string? Message { get; set; }
    public decimal? ProposedPrice { get; set; }
    public int? EstimatedSessionsCount { get; set; }
    public int Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DecidedAt { get; set; }
}
