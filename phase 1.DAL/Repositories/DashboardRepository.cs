using Microsoft.EntityFrameworkCore;
using phase_1.Data;
using phase_1.Models;

namespace phase_1.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalCasesAsync()
        {
            return await _context.Cases.CountAsync();
        }

        public async Task<int> GetActiveMatchesCountAsync()
        {
            return await _context.Matches.CountAsync(m => m.Status == MatchStatus.Active);
        }

        public async Task<int> GetCompletedMatchesCountAsync()
        {
            return await _context.Matches.CountAsync(m => m.Status == MatchStatus.Completed);
        }

        public async Task<int> GetCancelledMatchesCountAsync()
        {
            return await _context.Matches.CountAsync(m => m.Status == MatchStatus.Cancelled);
        }

        public async Task<int> GetTotalSessionsAsync()
        {
            return await _context.Sessions.CountAsync();
        }

        public async Task<int> GetCompletedSessionsCountAsync()
        {
            return await _context.Sessions.CountAsync(s => s.Status == SessionStatus.Completed);
        }

        public async Task<int> GetTotalNoShowStrikesAsync()
        {
            return await _context.NoShowStrikes.CountAsync();
        }
    }
}
