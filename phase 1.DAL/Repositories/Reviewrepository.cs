using Microsoft.EntityFrameworkCore;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories.Interfaces;
using phase_1.Data;

namespace phase_1.DAL.Repositories
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
                .Include(r => r.ReviewerUser)
                .Include(r => r.ReviewedUser)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<List<Review>> GetByReviewedUserIdAsync(int reviewedUserId)
        {
            return await _context.Reviews
                .Include(r => r.ReviewerUser)
                .Where(r => r.ReviewedUserId == reviewedUserId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Review>> GetByUserInvolvedAsync(int userId)
        {
            return await _context.Reviews
                .Include(r => r.ReviewerUser)
                .Include(r => r.ReviewedUser)
                .Where(r => r.ReviewerUserId == userId || r.ReviewedUserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Review>> GetByMatchIdAsync(int matchId)
        {
            return await _context.Reviews
                .Include(r => r.ReviewerUser)
                .Include(r => r.ReviewedUser)
                .Where(r => r.MatchId == matchId)
                .ToListAsync();
        }

        public async Task<bool> ExistsForMatchAndReviewerAsync(int matchId, int reviewerUserId)
        {
            return await _context.Reviews
                .AnyAsync(r => r.MatchId == matchId && r.ReviewerUserId == reviewerUserId);
        }

        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public async Task<double> GetAverageRatingForUserAsync(int userId)
        {
            var ratings = await _context.Reviews
                .Where(r => r.ReviewedUserId == userId)
                .Select(r => r.Rating)
                .ToListAsync();

            return ratings.Count == 0 ? 0 : ratings.Average();
        }

        public async Task<int> GetTotalReviewsCountAsync()
        {
            return await _context.Reviews.CountAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}