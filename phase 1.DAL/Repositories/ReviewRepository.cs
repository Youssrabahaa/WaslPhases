using Microsoft.EntityFrameworkCore;
using phase_1.Data;
using phase_1.Models;

namespace phase_1.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.Reviewee)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Review>> GetByRevieweeIdAsync(string revieweeId)
        {
            return await _context.Reviews
                .Include(r => r.Reviewer)
                .Where(r => r.RevieweeId == revieweeId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Review>> GetByMatchIdAsync(int matchId)
        {
            return await _context.Reviews
                .Include(r => r.Reviewer)
                .Include(r => r.Reviewee)
                .Where(r => r.MatchId == matchId)
                .ToListAsync();
        }

        public async Task<bool> ExistsForMatchAndReviewerAsync(int matchId, string reviewerId)
        {
            return await _context.Reviews
                .AnyAsync(r => r.MatchId == matchId && r.ReviewerId == reviewerId);
        }

        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();
        }

        public async Task<double> GetAverageRatingForUserAsync(string userId)
        {
            var ratings = await _context.Reviews
                .Where(r => r.RevieweeId == userId)
                .Select(r => r.Rating)
                .ToListAsync();

            return ratings.Count == 0 ? 0 : ratings.Average();
        }

        public async Task<int> GetTotalReviewsCountAsync()
        {
            return await _context.Reviews.CountAsync();
        }
    }
}
