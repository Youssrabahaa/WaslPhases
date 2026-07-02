using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

namespace phase_1.Controllers
{
    public class ConversationController : Controller
    {
        private readonly IConversationService _conversationService;
        private readonly IMatchService _matchService;

        public ConversationController(IConversationService conversationService, IMatchService matchService)
        {
            _conversationService = conversationService;
            _matchService = matchService;
        }

        public async Task<IActionResult> Chat(int matchId)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (currentUserId <= 0)
                return RedirectToAction("Login", "Auth");

            // ✅ التحقق من أن المستخدم طرف في المطابقة
            var match = await _matchService.GetMatchByIdAsync(matchId);
            if (match == null)
                return NotFound();

            if (match.PatientUserId != currentUserId && match.StudentUserId != currentUserId)
                return Forbid();

            var conversation = await _conversationService.GetByMatchIdAsync(matchId, currentUserId);

            if (conversation == null)
                return NotFound();

            return View(conversation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Send(SendMessageDTO dto)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (currentUserId <= 0)
                return Unauthorized(new { message = "يجب تسجيل الدخول." });

            if (string.IsNullOrWhiteSpace(dto.Content))
                return BadRequest(new { message = "اكتب رسالة أولا." });

            // ✅ التحقق من أن المرسل طرف في المطابقة
            var match = await _matchService.GetMatchByIdAsync(dto.MatchId);
            if (match == null)
                return NotFound();

            if (match.PatientUserId != currentUserId && match.StudentUserId != currentUserId)
                return Unauthorized(new { message = "غير مصرح لك بإرسال رسائل في هذه المحادثة." });

            dto.SenderUserId = currentUserId;

            var result = await _conversationService.SendMessageAsync(dto);

            if (!result)
                return BadRequest(new { message = "تعذر إرسال الرسالة." });

            return Ok();
        }
    }
}
