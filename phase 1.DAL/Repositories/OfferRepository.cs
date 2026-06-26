using Microsoft.EntityFrameworkCore;
using phase_1.Data;
using phase_1.Models;

namespace phase_1.DAL.Repositories;

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
        // Eager loading example
        return await _context.Offers
            .Include(o => o.Case)
            .Include(o => o.StudentUser)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Offer>> GetOffersByCaseIdAsync(int caseId)
    {
        return await _context.Offers
            .Include(o => o.StudentUser)
            .Where(o => o.CaseId == caseId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Offer>> GetOffersByStudentIdAsync(int studentId)
    {
        return await _context.Offers
            .Include(o => o.Case)
            .Where(o => o.StudentUserId == studentId)
            .ToListAsync();
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
}
