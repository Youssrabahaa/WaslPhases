using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

namespace phase_1.Controllers
{
    public class ConversationController : Controller
    {
        private readonly IConversationService _conversationService;

        public ConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        public async Task<IActionResult> Chat(int matchId)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var conversation = await _conversationService.GetByMatchIdAsync(matchId, currentUserId);

            if (conversation == null)
                return NotFound();
            return View(conversation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(SendMessageDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
                return BadRequest(new { message = "??????? ?????." });

            var result = await _conversationService.SendMessageAsync(dto);

            if (!result)
                return BadRequest(new { message = "????? ????? ???????." });

            return Ok();
        }
    }
}