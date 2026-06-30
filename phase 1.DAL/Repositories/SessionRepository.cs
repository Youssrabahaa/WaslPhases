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
    public class SessionRepository : ISessionRepository
    {
        private readonly AppDbContext _context;

        public SessionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Session>> GetByMatchAsync(int matchId)
        {
            return await _context.Sessions
             .Include(x => x.Match)
                 .ThenInclude(x => x.PatientUser)
             .Include(x => x.Match)
                 .ThenInclude(x => x.StudentUser)
             .Include(x => x.Match)
                 .ThenInclude(x => x.Case)
             .Where(x => x.MatchId == matchId)
             .OrderBy(x => x.Number)
             .ToListAsync();
        }

        public async Task<Session?> GetByIdAsync(int id)
        {
            return await _context.Sessions
                .Include(x => x.Match)
                    .ThenInclude(x => x.PatientUser)
                .Include(x => x.Match)
                    .ThenInclude(x => x.StudentUser)
                .Include(x => x.Match)
                    .ThenInclude(x => x.Case)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Session session)
        {
            await _context.Sessions.AddAsync(session);
        }

        public void Update(Session session)
        {
            _context.Sessions.Update(session);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
