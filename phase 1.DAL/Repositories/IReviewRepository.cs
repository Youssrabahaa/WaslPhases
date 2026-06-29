using phase_1.Models;

namespace phase_1.Repositories
{
    public interface IReviewRepository
    {
        Task<Review?> GetByIdAsync(int id);
        Task<List<Review>> GetByRevieweeIdAsync(string revieweeId);
        Task<List<Review>> GetByMatchIdAsync(int matchId);
        Task<bool> ExistsForMatchAndReviewerAsync(int matchId, string reviewerId);
        Task AddAsync(Review review);
        Task<double> GetAverageRatingForUserAsync(string userId);
        Task<int> GetTotalReviewsCountAsync();
    }
}
