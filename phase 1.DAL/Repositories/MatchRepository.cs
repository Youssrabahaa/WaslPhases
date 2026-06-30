using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using phase_1.DAL.Models;
using phase_1.Data;

namespace phase_1.DAL.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly AppDbContext _context;

        public MatchRepository(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Full include للـ Details page
        public async Task<Match?> GetByIdAsync(int matchId)
        {
            return await _context.Matches
                .Include(x => x.PatientUser)
                .Include(x => x.StudentUser)
                .Include(x => x.Case)
                    .ThenInclude(x => x.TreatmentCategory)
                .Include(x => x.Offer)
                .Include(x => x.Sessions)
                .Include(x => x.Conversation)
                .FirstOrDefaultAsync(x => x.Id == matchId);
        }

        public async Task<List<Match>> GetAllAsync()
        {
            return await _context.Matches.ToListAsync();
        }

        // ✅ إضافة Include(Case) — كانت ناقصة وبتسبب NullReferenceException
        public async Task<List<Match>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Matches
                .Include(x => x.Case)
                .Where(x => x.PatientUserId == patientId)
                .ToListAsync();
        }

        // ✅ إضافة Include(Case) — كانت ناقصة وبتسبب NullReferenceException
        public async Task<List<Match>> GetByStudentIdAsync(int studentId)
        {
            return await _context.Matches
                .Include(x => x.Case)
                .Where(x => x.StudentUserId == studentId)
                .ToListAsync();
        }

        public async Task<Match?> GetByOfferIdAsync(int offerId)
        {
            return await _context.Matches
                .FirstOrDefaultAsync(x => x.OfferId == offerId);
        }

        public async Task AddAsync(Match match)
        {
            await _context.Matches.AddAsync(match);
        }

        public Task UpdateAsync(Match match)
        {
            _context.Matches.Update(match);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Match match)
        {
            _context.Matches.Remove(match);
            return Task.CompletedTask;
        }

        // ✅ Include Offer و Case و Sessions كلها مطلوبة للـ expiry logic
        public async Task<List<Match>> GetActiveMatchesAsync()
        {
            return await _context.Matches
                .Include(m => m.Case)
                .Include(m => m.Offer)
                .Include(m => m.Sessions)
                .Where(m => m.Status == 1)
                .ToListAsync();
        }

        // ✅ إنشاء Conversation تلقائيًا عند قبول الـ Offer
        public async Task AddConversationAsync(Conversation conversation)
        {
            await _context.Conversations.AddAsync(conversation);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}