using phase_1.BLL.DTOs;

namespace phase_1.BLL.Services
{
    public interface IOfferService
    {
        Task<OfferDTO> CreateOfferAsync(CreateOfferDTO createOfferDto);
        Task<OfferDetailsDTO?> GetOfferDetailsAsync(int id);
        Task<IEnumerable<OfferDTO>> GetOffersForCaseAsync(int caseId);
        Task<IEnumerable<OfferDTO>> GetOffersByStudentAsync(int studentId);
        // ✅ AcceptOffer محذوف — اتحول لـ MatchService.AcceptOfferAsync حسب الـ business rules
        Task<bool> RejectOfferAsync(int offerId);
    }
}
