using System.Threading.Tasks;
using phase_1.BLL.DTOs;

namespace phase_1.BLL.Services
{
    public interface IConversationService
    {
        Task<ConversationDTO?> GetByMatchIdAsync(int matchId, int currentUserId);
        Task<bool> SendMessageAsync(SendMessageDTO dto);
    }
}