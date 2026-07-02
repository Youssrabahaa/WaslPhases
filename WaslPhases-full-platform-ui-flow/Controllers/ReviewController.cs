using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

namespace phase_1.PL.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;
        private readonly IMatchService _matchService;

        public ReviewController(IReviewService reviewService, IMatchService matchService)
        {
            _reviewService = reviewService;
            _matchService = matchService;
        }

        public async Task<IActionResult> ViewReviews()
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (userId <= 0)
                return RedirectToAction("Login", "Auth");

            var reviews = await _reviewService.GetReviewsForUserAsync(userId);
            
            // حساب المتوسط
            double average = 0;
            if (reviews.Any())
            {
                average = reviews.Average(r => r.Rating);
            }
            
            ViewBag.AverageRating = average.ToString("F1");
            ViewBag.ReviewsCount = reviews.Count;
            ViewBag.PositivePercentage = reviews.Any() 
                ? (reviews.Count(r => r.Rating >= 4) * 100 / reviews.Count) 
                : 100;

            return View(reviews);
        }

        public async Task<IActionResult> AddReview(int matchId)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (currentUserId <= 0)
                return RedirectToAction("Login", "Auth");

            var match = await _matchService.GetMatchByIdAsync(matchId);
            if (match == null)
                return NotFound();

            if (match.PatientUserId != currentUserId && match.StudentUserId != currentUserId)
                return Forbid();

            var revieweeUserId = match.PatientUserId == currentUserId
                ? match.StudentUserId
                : match.PatientUserId;

            var model = new CreateReviewDTO
            {
                MatchId = matchId,
                RevieweeUserId = revieweeUserId
            };

            ViewBag.MatchTitle = match.CaseTitle;
            ViewBag.RevieweeName = match.PatientUserId == currentUserId ? match.StudentName : match.PatientName;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReview(CreateReviewDTO dto)
        {
            var reviewerUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (reviewerUserId <= 0)
                return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
            {
                var match = await _matchService.GetMatchByIdAsync(dto.MatchId);
                if (match != null)
                {
                    ViewBag.MatchTitle = match.CaseTitle;
                    ViewBag.RevieweeName = match.PatientUserId == reviewerUserId ? match.StudentName : match.PatientName;
                }
                return View(dto);
            }

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