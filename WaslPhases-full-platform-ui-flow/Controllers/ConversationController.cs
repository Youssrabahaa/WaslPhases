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

        // GET: /student/Conversation/Chat?matchId=1
        public async Task<IActionResult> Chat(int matchId)
        {
            //var userIdStr = Request.Cookies["UserId"];
            //int.TryParse(userIdStr, out int currentUserId);
            int currentUserId = 3;

            var conversation = await _conversationService.GetByMatchIdAsync(matchId, currentUserId);

            if (conversation == null)
                //return NotFound();
                return Content($"Conversation not found for matchId={matchId}, userId={currentUserId}");
            return View(conversation);
        }

        // ✅ POST عبر AJAX — يرجع JSON بدل Redirect عشان SignalR يبقى متزامن
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(SendMessageDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Content))
                return BadRequest(new { message = "الرسالة فارغة." });

            var result = await _conversationService.SendMessageAsync(dto);

            if (!result)
                return BadRequest(new { message = "تعذّر إرسال الرسالة." });

            return Ok();
        }
    }
}