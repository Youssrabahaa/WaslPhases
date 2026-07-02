using Microsoft.EntityFrameworkCore;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories.Interfaces;
using phase_1.Data;

namespace phase_1.DAL.Repositories
{
    public class CaseRepository : ICaseRepository
    {
        private readonly AppDbContext _context;

        public CaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Case?> GetByIdAsync(int id)
        {
            return await _context.Cases
                .Include(c => c.PatientUser)
                .Include(c => c.ServiceType)
                .Include(c => c.TreatmentCategory)
                .Include(c => c.Offers)
                    //  مطلوب عشان CaseDetailsDTO.Offers.StudentName يشتغل
                    .ThenInclude(o => o.StudentUser)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Case>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Cases
                .Include(c => c.ServiceType)
                .Include(c => c.TreatmentCategory)
                .Include(c => c.Offers)
                .Where(c => c.PatientUserId == patientId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Case>> GetAllOpenAsync()
        {
            return await _context.Cases
                .Include(c => c.PatientUser)
                .Include(c => c.ServiceType)
                .Include(c => c.TreatmentCategory)
                .Include(c => c.Offers)
                .Where(c => c.Status == 1)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Case c)
        {
            await _context.Cases.AddAsync(c);
        }

        public Task UpdateAsync(Case c)
        {
            _context.Cases.Update(c);
            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}