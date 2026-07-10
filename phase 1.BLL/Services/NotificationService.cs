using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using phase_1.BLL.DTOs;
using phase_1.BLL.Hubs;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories.Interfaces;

namespace phase_1.BLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly IHubContext<NotificationHub> _hub;

        public NotificationService(
            INotificationRepository repo,
            IHubContext<NotificationHub> hub)
        {
            _repo = repo;
            _hub = hub;
        }

        public async Task SendAsync(
            int userId, string title, string message,
            int type, int? referenceId = null, string? referenceType = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,
                ReferenceId = referenceId,
                ReferenceType = referenceType,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(notification);
            await _repo.SaveChangesAsync();


            var dto = MapToDTO(notification);
            await _hub.Clients
                .Group($"user-{userId}")
                .SendAsync("ReceiveNotification", dto);
        }

        public async Task<List<NotificationDTO>> GetForUserAsync(int userId)
        {
            var notifications = await _repo.GetByUserIdAsync(userId);
            return notifications.Select(MapToDTO).ToList();
        }

        public async Task<int> GetUnreadCountAsync(int userId)
        {
            return await _repo.GetUnreadCountAsync(userId);
        }

        public async Task MarkAsReadAsync(int notificationId, int userId)
        {
            await _repo.MarkAsReadAsync(notificationId, userId);
            await _repo.SaveChangesAsync();
        }

        public async Task MarkAllAsReadAsync(int userId)
        {
            await _repo.MarkAllAsReadAsync(userId);
            await _repo.SaveChangesAsync();
        }

        private static NotificationDTO MapToDTO(Notification n)
        {
            return new NotificationDTO
            {
                Id = n.Id,
                Title = n.Title,
                Message = n.Message,
                Type = n.Type,
                ReferenceId = n.ReferenceId,
                ReferenceType = n.ReferenceType,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt,
                TimeAgo = GetTimeAgo(n.CreatedAt)
            };
        }

        private static string GetTimeAgo(DateTime createdAt)
        {
            var diff = DateTime.UtcNow - createdAt;
            if (diff.TotalMinutes < 1) return "الآن";
            if (diff.TotalMinutes < 60) return $"منذ {(int)diff.TotalMinutes} دقيقة";
            if (diff.TotalHours < 24) return $"منذ {(int)diff.TotalHours} ساعة";
            return $"منذ {(int)diff.TotalDays} يوم";
        }
    }
}