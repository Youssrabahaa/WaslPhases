using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

namespace phase_1.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<IActionResult> ViewReviews()
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var reviews = await _reviewService.GetReviewsForUserAsync(userId);
            return View(reviews);
        }

        public IActionResult AddReview(int matchId, int revieweeUserId)
        {
            var dto = new CreateReviewDTO
            {
                MatchId = matchId,
                RevieweeUserId = revieweeUserId,
                Rating = 5
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(CreateReviewDTO dto)
        {
            var reviewerUserId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _reviewService.CreateReviewAsync(reviewerUserId, dto);
                TempData["Success"] = "تم إضافة التقييم بنجاح.";
                return RedirectToAction("MatchDetails", "Match", new { id = dto.MatchId });
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("MatchDetails", "Match", new { id = dto.MatchId });
            }
            catch (UnauthorizedAccessException)
            {
                TempData["Error"] = "غير مسموح لك بتقييم هذه المطابقة.";
                return RedirectToAction("MatchDetails", "Match", new { id = dto.MatchId });
            }
        }
    }
}