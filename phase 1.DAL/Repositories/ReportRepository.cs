using Microsoft.EntityFrameworkCore;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories.Interfaces;
using phase_1.Data;

namespace phase_1.DAL.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Report?> GetByIdAsync(int id)
        {
            return await _context.Reports.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Report?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Reports
                .Include(r => r.ReporterUser)
                .Include(r => r.ReportedUser)
                .Include(r => r.Session)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Report>> GetAllAsync()
        {
            return await _context.Reports
                .Include(r => r.ReporterUser)
                .Include(r => r.ReportedUser)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Report>> GetByStatusAsync(int status)
        {
            return await _context.Reports
                .Include(r => r.ReporterUser)
                .Include(r => r.ReportedUser)
                .Where(r => r.Status == status)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Report>> GetBySessionIdAsync(int sessionId)
        {
            return await _context.Reports
                .Where(r => r.SessionId == sessionId)
                .ToListAsync();
        }

        public async Task<List<Report>> GetByReporterIdAsync(int reporterId)
        {
            return await _context.Reports
                .Where(r => r.ReporterUserId == reporterId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
        }

        public async Task UpdateAsync(Report report)
        {
            _context.Reports.Update(report);
        }

        public async Task<int> GetCountByStatusAsync(int status)
        {
            return await _context.Reports.CountAsync(r => r.Status == status);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}