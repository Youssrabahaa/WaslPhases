using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories.Interfaces;
using phase_1.Data;

namespace phase_1.DAL.Repositories
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly AppDbContext _context;

        public ConversationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Conversation?> GetByMatchIdAsync(int matchId)
        {
            return await _context.Conversations
                .Include(c => c.Messages)
                    .ThenInclude(m => m.SenderUser)
                .Include(c => c.Match)
                    .ThenInclude(m => m.PatientUser)
                .Include(c => c.Match)
                    .ThenInclude(m => m.StudentUser)
                .FirstOrDefaultAsync(c => c.MatchId == matchId);
        }

        public async Task AddAsync(Conversation conversation)
        {
            await _context.Conversations.AddAsync(conversation);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
