using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using phase_1.BLL.DTOs;
using phase_1.BLL.Hubs;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories.Interfaces;

namespace phase_1.BLL.Services
{
    public class ConversationService : IConversationService
    {
        private const int NewMessageNotificationType = 4;
        private const int NotificationMessageMaxLength = 400;

        private readonly IConversationRepository _conversationRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly INotificationService _notificationService;

        public ConversationService(
            IConversationRepository conversationRepository,
            IMessageRepository messageRepository,
            IHubContext<ChatHub> hubContext,
            INotificationService notificationService)
        {
            _conversationRepository = conversationRepository;
            _messageRepository = messageRepository;
            _hubContext = hubContext;
            _notificationService = notificationService;
        }

        public async Task<ConversationDTO?> GetByMatchIdAsync(int matchId, int currentUserId)
        {
            var conversation = await _conversationRepository.GetByMatchIdAsync(matchId);

            if (conversation == null)
                return null;

            return new ConversationDTO
            {
                ConversationId = conversation.Id,
                MatchId = conversation.MatchId,
                PatientName = conversation.Match.PatientUser.FullName,
                StudentName = conversation.Match.StudentUser.FullName,
                CurrentUserId = currentUserId,
                Messages = conversation.Messages
                    .OrderBy(m => m.SentAt)
                    .Select(m => new ChatMessageDTO
                    {
                        Id = m.Id,
                        SenderUserId = m.SenderUserId,
                        SenderName = m.SenderUser.FullName,
                        Content = m.Content,
                        SentAt = m.SentAt,
                        IsRead = m.IsRead,
                        IsOwn = m.SenderUserId == currentUserId
                    }).ToList()
            };
        }

        public async Task<bool> SendMessageAsync(SendMessageDTO dto)
        {
            var conversation = await _conversationRepository.GetByMatchIdAsync(dto.MatchId);

            if (conversation == null)
                return false;

            var patientId = conversation.Match.PatientUserId;
            var studentId = conversation.Match.StudentUserId;

            if (dto.SenderUserId != patientId && dto.SenderUserId != studentId)
                return false;

            var receiverId = dto.SenderUserId == patientId ? studentId : patientId;

            var message = new Message
            {
                ConversationId = conversation.Id,
                SenderUserId = dto.SenderUserId,
                ReceiverUserId = receiverId,
                Content = dto.Content.Trim(),
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            await _messageRepository.AddAsync(message);

            conversation.LastMessageAt = message.SentAt;

            await _messageRepository.SaveChangesAsync();

            var senderName = dto.SenderUserId == patientId
                ? conversation.Match.PatientUser.FullName
                : conversation.Match.StudentUser.FullName;

            await _hubContext.Clients
                .Group($"match-{dto.MatchId}")
                .SendAsync("ReceiveMessage", new
                {
                    id = message.Id,
                    senderUserId = message.SenderUserId,
                    senderName = senderName,
                    content = message.Content,
                    sentAt = message.SentAt.ToString("hh:mm tt")
                });

            var truncatedContent = message.Content.Length > NotificationMessageMaxLength
                ? message.Content.Substring(0, NotificationMessageMaxLength) + "..."
                : message.Content;

            await _notificationService.SendAsync(
                receiverId,
                "رسالة جديدة",
                $"{senderName}: {truncatedContent}",
                type: NewMessageNotificationType, referenceId: dto.MatchId, referenceType: "Match");

            return true;
        }
    }
}