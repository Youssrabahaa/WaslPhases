using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using phase_1.DTOs;
using phase_1.Services;

namespace phase_1.Controllers
{
    [Authorize] // أي مستخدم مسجّل دخول (Patient أو Student)
    [Route("api/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // POST api/reviews
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReviewDTO dto)
        {
            var userId = GetCurrentUserId();

            try
            {
                var result = await _reviewService.CreateReviewAsync(userId, dto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET api/reviews/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetForUser(string userId)
        {
            var reviews = await _reviewService.GetReviewsForUserAsync(userId);
            return Ok(reviews);
        }

        // GET api/reviews/user/{userId}/average
        [HttpGet("user/{userId}/average")]
        public async Task<IActionResult> GetAverage(string userId)
        {
            var average = await _reviewService.GetAverageRatingAsync(userId);
            return Ok(new { userId, averageRating = average });
        }

        private string GetCurrentUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("User not authenticated.");
        }
    }
}
