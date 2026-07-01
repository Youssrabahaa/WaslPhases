using phase_1.BLL.DTOs;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories;
using phase_1.DAL.Repositories.Interfaces;

namespace phase_1.BLL.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMatchRepository _matchRepository;

        public ReviewService(IReviewRepository reviewRepository, IMatchRepository matchRepository)
        {
            _reviewRepository = reviewRepository;
            _matchRepository = matchRepository;
        }

        public async Task<ReviewDTO> CreateReviewAsync(int reviewerUserId, CreateReviewDTO dto)
        {
            var match = await _matchRepository.GetByIdAsync(dto.MatchId)
                ?? throw new InvalidOperationException("Match not found.");

            if (match.Status != 2)
                throw new InvalidOperationException("Reviews are only allowed after the match is completed.");

            if (match.PatientUserId != reviewerUserId && match.StudentUserId != reviewerUserId)
                throw new UnauthorizedAccessException("You are not a participant of this match.");

            var expectedRevieweeId = match.PatientUserId == reviewerUserId
                ? match.StudentUserId
                : match.PatientUserId;

            if (dto.RevieweeUserId != expectedRevieweeId)
                throw new InvalidOperationException("You can only review the other participant of this match.");

            var alreadyReviewed = await _reviewRepository.ExistsForMatchAndReviewerAsync(dto.MatchId, reviewerUserId);
            if (alreadyReviewed)
                throw new InvalidOperationException("You have already reviewed this match.");

            var review = new Review
            {
                MatchId = dto.MatchId,
                ReviewerUserId = reviewerUserId,
                ReviewedUserId = dto.RevieweeUserId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepository.AddAsync(review);
            await _reviewRepository.SaveChangesAsync();

            return new ReviewDTO
            {
                Id = review.Id,
                MatchId = review.MatchId,
                ReviewerUserId = review.ReviewerUserId,
                ReviewedUserId = review.ReviewedUserId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }

        public async Task<List<ReviewDTO>> GetReviewsForUserAsync(int userId)
        {
            var reviews = await _reviewRepository.GetByReviewedUserIdAsync(userId);

            return reviews.Select(r => new ReviewDTO
            {
                Id = r.Id,
                MatchId = r.MatchId,
                ReviewerUserId = r.ReviewerUserId,
                ReviewerName = r.ReviewerUser?.FullName ?? string.Empty,
                ReviewedUserId = r.ReviewedUserId,
                ReviewedName = r.ReviewedUser?.FullName ?? string.Empty,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task<double> GetAverageRatingAsync(int userId)
        {
            return await _reviewRepository.GetAverageRatingForUserAsync(userId);
        }
    }
}