using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories.Interfaces;
using phase_1.Data;

namespace phase_1.DAL.Repositories
{
    public class NoShowStrikeRepository : INoShowStrikeRepository
    {
        private readonly AppDbContext _context;

        public NoShowStrikeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<NoShowStrike?> GetBySessionAsync(int sessionId)
        {
            return await _context.NoShowStrikes
                .Include(x => x.User)
                .Include(x => x.Session)
                    .ThenInclude(x => x.Match)
                .FirstOrDefaultAsync(x => x.SessionId == sessionId);
        }

        public async Task AddAsync(NoShowStrike strike)
        {
            await _context.NoShowStrikes.AddAsync(strike);
        }

        public async Task<int> GetCountByUserAsync(int userId)
        {
            return await _context.NoShowStrikes
                .CountAsync(x => x.UserId == userId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
