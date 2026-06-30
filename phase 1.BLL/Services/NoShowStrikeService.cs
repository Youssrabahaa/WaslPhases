using System;
using System.Threading.Tasks;
using phase_1.BLL.DTOs;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories.Interfaces;

namespace phase_1.BLL.Services
{
    public class NoShowStrikeService : INoShowStrikeService
    {
        private readonly INoShowStrikeRepository _strikeRepository;
        private readonly ISessionRepository _sessionRepository;

        public NoShowStrikeService(
            INoShowStrikeRepository strikeRepository,
            ISessionRepository sessionRepository)
        {
            _strikeRepository = strikeRepository;
            _sessionRepository = sessionRepository;
        }

        public async Task<bool> CreateStrikeAsync(CreateNoShowStrikeDTO dto)
        {
            var session = await _sessionRepository.GetByIdAsync(dto.SessionId);

            if (session == null)
                return false;

            var exists = await _strikeRepository.GetBySessionAsync(dto.SessionId);
            if (exists != null)
                return false;

            var patientId = session.Match.PatientUserId;
            var studentId = session.Match.StudentUserId;

            if (dto.ReportedByUserId != patientId &&
                dto.ReportedByUserId != studentId)
                return false;

            if (dto.AbsentUserId != patientId &&
                dto.AbsentUserId != studentId)
                return false;

            if (dto.ReportedByUserId == dto.AbsentUserId)
                return false;

            var strike = new NoShowStrike
            {
                SessionId = dto.SessionId,
                UserId = dto.AbsentUserId,
                CreatedAt = DateTime.UtcNow
            };

            await _strikeRepository.AddAsync(strike);
            await _strikeRepository.SaveChangesAsync();

            return true;
        }

        public async Task<NoShowStrikeDTO?> GetBySessionAsync(int sessionId)
        {
            var strike = await _strikeRepository.GetBySessionAsync(sessionId);

            if (strike == null)
                return null;

            return new NoShowStrikeDTO
            {
                Id = strike.Id,
                SessionId = strike.SessionId,
                UserId = strike.UserId,
                UserName = strike.User.FullName,
                WhoMissed = strike.UserId == strike.Session.Match.PatientUserId
                                ? "المريض"
                                : "الطالب",
                CreatedAt = strike.CreatedAt
            };
        }

        public async Task<int> GetUserStrikeCountAsync(int userId)
        {
            return await _strikeRepository.GetCountByUserAsync(userId);
        }
    }
}