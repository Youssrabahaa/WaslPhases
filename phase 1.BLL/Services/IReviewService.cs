using phase_1.DTOs;

namespace phase_1.Services
{
    public interface IReviewService
    {
        Task<ReviewDTO> CreateReviewAsync(string reviewerId, CreateReviewDTO dto);
        Task<List<ReviewDTO>> GetReviewsForUserAsync(string userId);
        Task<double> GetAverageRatingAsync(string userId);
    }
}
