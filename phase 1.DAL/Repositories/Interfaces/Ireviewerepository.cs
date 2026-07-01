using phase_1.DAL.Models;

namespace phase_1.DAL.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review?> GetByIdAsync(int id);
        Task<List<Review>> GetByReviewedUserIdAsync(int reviewedUserId);
        Task<List<Review>> GetByMatchIdAsync(int matchId);
        Task<bool> ExistsForMatchAndReviewerAsync(int matchId, int reviewerUserId);
        Task AddAsync(Review review);
        Task<double> GetAverageRatingForUserAsync(int userId);
        Task<int> GetTotalReviewsCountAsync();
        Task SaveChangesAsync();
    }
}