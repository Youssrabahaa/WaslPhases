using phase_1.BLL.DTOs;

namespace phase_1.BLL.Services
{
    public interface IOfferService
    {
        Task<OfferDTO> CreateOfferAsync(CreateOfferDTO createOfferDto);
        Task<OfferDetailsDTO?> GetOfferDetailsAsync(int id);
        Task<IEnumerable<OfferDTO>> GetOffersForCaseAsync(int caseId);
        Task<IEnumerable<OfferDTO>> GetOffersByStudentAsync(int studentId);
        Task<bool> RejectOfferAsync(int offerId);

        //  method جديدة — بترجع الـ offer الموجود لو الطالب عنده offer على نفس الحالة
        Task<OfferDTO?> GetExistingOfferAsync(int studentId, int caseId);
    }
}