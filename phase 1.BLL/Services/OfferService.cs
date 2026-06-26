using phase_1.BLL.DTOs;
using phase_1.BLL.Mappers;
using phase_1.DAL.Repositories;

namespace phase_1.BLL.Services;

public class OfferService : IOfferService
{
    private readonly IOfferRepository _offerRepository;

    public OfferService(IOfferRepository offerRepository)
    {
        _offerRepository = offerRepository;
    }

    public async Task<OfferDTO> CreateOfferAsync(CreateOfferDTO createOfferDto)
    {
        var entity = createOfferDto.ToEntity();
        var createdEntity = await _offerRepository.AddAsync(entity);
        return createdEntity.ToDTO();
    }

    public async Task<OfferDetailsDTO?> GetOfferDetailsAsync(int id)
    {
        var offer = await _offerRepository.GetByIdAsync(id);
        if (offer == null) return null;

        return offer.ToDetailsDTO();
    }

    public async Task<IEnumerable<OfferDTO>> GetOffersForCaseAsync(int caseId)
    {
        var offers = await _offerRepository.GetOffersByCaseIdAsync(caseId);
        return offers.Select(o => o.ToDTO());
    }

    public async Task<IEnumerable<OfferDTO>> GetOffersByStudentAsync(int studentId)
    {
        var offers = await _offerRepository.GetOffersByStudentIdAsync(studentId);
        return offers.Select(o => o.ToDTO());
    }

    public async Task<bool> AcceptOfferAsync(int offerId)
    {
        var offer = await _offerRepository.GetByIdAsync(offerId);
        if (offer == null) return false;

        offer.Status = 2;
        offer.DecidedAt = DateTime.UtcNow;
        await _offerRepository.UpdateAsync(offer);
        return true;
    }

    public async Task<bool> RejectOfferAsync(int offerId)
    {
        var offer = await _offerRepository.GetByIdAsync(offerId);
        if (offer == null) return false;

        offer.Status = 3;
        offer.DecidedAt = DateTime.UtcNow;
        await _offerRepository.UpdateAsync(offer);
        return true;
    }
}
