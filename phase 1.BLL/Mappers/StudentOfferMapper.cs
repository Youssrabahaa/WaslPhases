using phase_1.Models;
using phase_1.BLL.DTOs;
using System.Linq;

namespace phase_1.BLL.Mappers;

public static class StudentOfferMapper
{
    public static Offer ToEntity(this CreateOfferDTO dto)
    {
        return new Offer
        {
            CaseId = dto.CaseId,
            StudentUserId = dto.StudentUserId,
            Message = dto.Message,
            ProposedPrice = dto.ProposedPrice,
            EstimatedSessionsCount = dto.EstimatedSessionsCount,
            Status = 1,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static OfferDTO ToDTO(this Offer entity)
    {
        return new OfferDTO
        {
            Id = entity.Id,
            CaseId = entity.CaseId,
            StudentUserId = entity.StudentUserId,
            Message = entity.Message,
            ProposedPrice = entity.ProposedPrice,
            EstimatedSessionsCount = entity.EstimatedSessionsCount,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt
        };
    }

    public static OfferDetailsDTO ToDetailsDTO(this Offer entity)
    {
        return new OfferDetailsDTO
        {
            Id = entity.Id,
            CaseId = entity.CaseId,
            CaseTitle = entity.Case?.Title, 
            StudentName = entity.StudentUser?.FullName,
            StudentUserId = entity.StudentUserId,
            Message = entity.Message,
            ProposedPrice = entity.ProposedPrice,
            EstimatedSessionsCount = entity.EstimatedSessionsCount,
            Status = entity.Status,
            CreatedAt = entity.CreatedAt,
            DecidedAt = entity.DecidedAt
        };
    }

}
