using phase_1.DAL.Models;

namespace phase_1.DAL.Repositories.Interfaces
{
    public interface IOfferRepository
    {
        Task<Offer> AddAsync(Offer offer);
        Task<Offer?> GetByIdAsync(int id);
        Task<IEnumerable<Offer>> GetOffersByCaseIdAsync(int caseId);
        Task<IEnumerable<Offer>> GetOffersByStudentIdAsync(int studentId);
        Task<List<Offer>> GetCaseOffersAsync(int caseId);

        Task<Offer?> GetByStudentAndCaseAsync(int studentId, int caseId, bool pendingOnly = false);

        Task UpdateAsync(Offer offer);
        Task DeleteAsync(Offer offer);
        Task SaveChangesAsync();
    }
}