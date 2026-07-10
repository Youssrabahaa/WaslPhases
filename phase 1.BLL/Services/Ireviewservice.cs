using phase_1.BLL.DTOs;

namespace phase_1.BLL.Services
{
    public interface IReviewService
    {
        Task<ReviewDTO> CreateReviewAsync(int reviewerUserId, CreateReviewDTO dto);
        Task<List<ReviewDTO>> GetReviewsForUserAsync(int userId);
        Task<double> GetAverageRatingAsync(int userId);
        Task<(double Average, int Count)> GetRatingSummaryAsync(int userId);
    }
}