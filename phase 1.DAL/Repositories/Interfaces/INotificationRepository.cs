using System.Collections.Generic;
using System.Threading.Tasks;
using phase_1.DAL.Models;

namespace phase_1.DAL.Repositories.Interfaces
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);
        Task<List<Notification>> GetByUserIdAsync(int userId);
        Task<int> GetUnreadCountAsync(int userId);
        Task MarkAsReadAsync(int notificationId, int userId);
        Task MarkAllAsReadAsync(int userId);
        Task SaveChangesAsync();
    }
}