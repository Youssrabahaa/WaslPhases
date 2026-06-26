using phase_1.Models;

namespace phase_1.DAL.Repositories;

public interface IOfferRepository
{
    Task<Offer> AddAsync(Offer offer);
    Task<Offer?> GetByIdAsync(int id);
    Task<IEnumerable<Offer>> GetOffersByCaseIdAsync(int caseId);
    Task<IEnumerable<Offer>> GetOffersByStudentIdAsync(int studentId);
    Task UpdateAsync(Offer offer);
    Task DeleteAsync(Offer offer);
}
