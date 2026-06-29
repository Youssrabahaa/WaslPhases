using phase_1.DTOs;
using phase_1.Models;
using phase_1.Repositories;

namespace phase_1.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMatchRepository _matchRepository; // موجود عند يوسرا - بنستخدمه للتحقق فقط (قراءة)

        public ReviewService(IReviewRepository reviewRepository, IMatchRepository matchRepository)
        {
            _reviewRepository = reviewRepository;
            _matchRepository = matchRepository;
        }

        public async Task<ReviewDTO> CreateReviewAsync(string reviewerId, CreateReviewDTO dto)
        {
            var match = await _matchRepository.GetByIdAsync(dto.MatchId)
                ?? throw new InvalidOperationException("Match not found.");

            // Business Rule: الريفيو يُسمح بيه فقط بعد ما الـ Match يكون Completed
            // (الجلسات خلصت كلها فعلاً، يعني فيه تجربة فعلية تُقيَّم)
            if (match.Status != MatchStatus.Completed)
                throw new InvalidOperationException("Reviews are only allowed after the match is completed.");

            // الريفيور لازم يكون طرف فعلي في الـ Match (Patient أو Student بتاع الماتش)
            var isParticipant = match.PatientId == reviewerId || match.StudentId == reviewerId;
            if (!isParticipant)
                throw new UnauthorizedAccessException("You are not a participant of this match.");

            // الـ Reviewee لازم يكون الطرف الآخر في نفس الماتش (مينفعش تقيّم حد خارج الماتش)
            var expectedRevieweeId = match.PatientId == reviewerId ? match.StudentId : match.PatientId;
            if (dto.RevieweeId != expectedRevieweeId)
                throw new InvalidOperationException("You can only review the other participant of this match.");

            // مينفعش المستخدم يعمل أكتر من ريفيو لنفس الماتش
            var alreadyReviewed = await _reviewRepository.ExistsForMatchAndReviewerAsync(dto.MatchId, reviewerId);
            if (alreadyReviewed)
                throw new InvalidOperationException("You have already reviewed this match.");

            var review = new Review
            {
                MatchId = dto.MatchId,
                ReviewerId = reviewerId,
                RevieweeId = dto.RevieweeId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepository.AddAsync(review);

            return new ReviewDTO
            {
                Id = review.Id,
                MatchId = review.MatchId,
                ReviewerId = review.ReviewerId,
                RevieweeId = review.RevieweeId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }

        public async Task<List<ReviewDTO>> GetReviewsForUserAsync(string userId)
        {
            var reviews = await _reviewRepository.GetByRevieweeIdAsync(userId);

            return reviews.Select(r => new ReviewDTO
            {
                Id = r.Id,
                MatchId = r.MatchId,
                ReviewerId = r.ReviewerId,
                ReviewerName = r.Reviewer?.UserName ?? string.Empty,
                RevieweeId = r.RevieweeId,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task<double> GetAverageRatingAsync(string userId)
        {
            return await _reviewRepository.GetAverageRatingForUserAsync(userId);
        }
    }
}
