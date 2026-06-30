using System;
using System.Linq;
using System.Threading.Tasks;
using phase_1.BLL.DTOs;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories.Interfaces;

namespace phase_1.BLL.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IMessageRepository _messageRepository;

        public ConversationService(
            IConversationRepository conversationRepository,
            IMessageRepository messageRepository)
        {
            _conversationRepository = conversationRepository;
            _messageRepository = messageRepository;
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

            return true;
        }
    }
}