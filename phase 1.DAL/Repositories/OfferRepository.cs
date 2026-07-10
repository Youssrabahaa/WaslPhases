using Microsoft.EntityFrameworkCore;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories.Interfaces;
using phase_1.Data;

namespace phase_1.DAL.Repositories
{
    public class OfferRepository : IOfferRepository
    {
        private readonly AppDbContext _context;

        public OfferRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Offer> AddAsync(Offer offer)
        {
            await _context.Offers.AddAsync(offer);
            await _context.SaveChangesAsync();
            return offer;
        }

        public async Task<Offer?> GetByIdAsync(int id)
        {
            return await _context.Offers
                .Include(o => o.Case)
                    .ThenInclude(c => c.PatientUser)
                .Include(o => o.StudentUser)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Offer>> GetOffersByCaseIdAsync(int caseId)
        {
            return await _context.Offers
                .Include(o => o.StudentUser)
                .Where(o => o.CaseId == caseId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Offer>> GetOffersByStudentIdAsync(int studentId)
        {
            return await _context.Offers
                .Include(o => o.Case)
                .Where(o => o.StudentUserId == studentId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Offer>> GetCaseOffersAsync(int caseId)
        {
            return await _context.Offers
                .Where(o => o.CaseId == caseId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<Offer?> GetByStudentAndCaseAsync(
            int studentId, int caseId, bool pendingOnly = false)
        {
            var query = _context.Offers
                .Where(o => o.StudentUserId == studentId && o.CaseId == caseId);

            if (pendingOnly)
                query = query.Where(o => o.Status == 1);

            return await query.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Offer offer)
        {
            _context.Offers.Update(offer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Offer offer)
        {
            _context.Offers.Remove(offer);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}