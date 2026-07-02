using System.Collections.Generic;
using System.Threading.Tasks;
using phase_1.BLL.DTOs;

namespace phase_1.BLL.Services
{
    public interface INotificationService
    {
        Task SendAsync(int userId, string title, string message,
                       int type, int? referenceId = null, string? referenceType = null);

        Task<List<NotificationDTO>> GetForUserAsync(int userId);

        Task<int> GetUnreadCountAsync(int userId);

        Task MarkAsReadAsync(int notificationId, int userId);
        Task MarkAllAsReadAsync(int userId);
    }
}