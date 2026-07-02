using System.Collections.Generic;
using System.Threading.Tasks;
using phase_1.BLL.DTOs;
using phase_1.DAL.Models;

namespace phase_1.BLL.Services
{
    public interface IReminderService
    {
        Task<List<ReminderDTO>> GetBySessionAsync(int sessionId);

        Task CreateSessionRemindersAsync(Session session);

        Task<bool> MarkAsSentAsync(int reminderId);

        Task ProcessDueRemindersAsync();

        Task UpdateSessionRemindersAsync(Session session, DateTime oldStartAt);

        Task DeletePendingRemindersAsync(int sessionId);
    }
}