using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        Task UpdateAsync(Offer offer);
        Task DeleteAsync(Offer offer);
    }
}
