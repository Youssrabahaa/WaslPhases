using System.Threading.Tasks;
using phase_1.DAL.Models;

namespace phase_1.DAL.Repositories.Interfaces
{
    public interface IConversationRepository
    {
        Task<Conversation?> GetByMatchIdAsync(int matchId);
        Task AddAsync(Conversation conversation);
        Task SaveChangesAsync();
    }
}