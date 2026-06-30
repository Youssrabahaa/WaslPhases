using Microsoft.AspNetCore.Mvc;
using phase_1.BLL.DTOs;
using phase_1.BLL.Services;

namespace phase_1.Controllers;

public class ConversationController : Controller
{
    private readonly IConversationService _conversationService;

    public ConversationController(IConversationService conversationService)
    {
        _conversationService = conversationService;
    }

    public async Task<IActionResult> Chat(int matchId)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
            return RedirectToAction("Login", "Auth");

        if (matchId <= 0)
            return RedirectToAction("Index", "Match");

        var conversation = await _conversationService.GetByMatchIdAsync(matchId, userId.Value);
        if (conversation == null)
            return NotFound();

        return View(conversation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Send(SendMessageDTO dto)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (!userId.HasValue)
            return RedirectToAction("Login", "Auth");

        dto.SenderUserId = userId.Value;

        if (!string.IsNullOrWhiteSpace(dto.Content))
            await _conversationService.SendMessageAsync(dto);

        return RedirectToAction(nameof(Chat), new { matchId = dto.MatchId });
    }
}
