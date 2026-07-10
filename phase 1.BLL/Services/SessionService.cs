using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using phase_1.BLL.DTOs;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories.Interfaces;

namespace phase_1.BLL.Services
{
    public class SessionService : ISessionService
    {
        private const int NewSessionNotificationType = 5;

        private readonly ISessionRepository _sessionRepository;
        private readonly INoShowStrikeService _noShowStrikeService;
        private readonly IReminderService _reminderService;
        private readonly IMatchService _matchService;
        private readonly INotificationService _notificationService;

        public SessionService(
            ISessionRepository sessionRepository,
            INoShowStrikeService noShowStrikeService,
            IReminderService reminderService,
            IMatchService matchService,
            INotificationService notificationService)
        {
            _sessionRepository = sessionRepository;
            _noShowStrikeService = noShowStrikeService;
            _reminderService = reminderService;
            _matchService = matchService;
            _notificationService = notificationService;
        }

        public async Task<List<SessionDTO>> GetByMatchAsync(int matchId)
        {
            var sessions = await _sessionRepository.GetByMatchAsync(matchId);

            return sessions.Select(x => new SessionDTO
            {
                Id = x.Id,
                MatchId = x.MatchId,
                Number = x.Number,
                StartAt = x.StartAt,
                EndAt = x.EndAt,
                LocationText = x.LocationText,
                ClinicRoom = x.ClinicRoom,
                Status = x.Status,
                StatusText = x.Status == 1 ? "مجدولة"
                             : x.Status == 2 ? "قيد التنفيذ"
                             : x.Status == 3 ? "مكتملة"
                             : "ملغية",
                HasReport = false
            }).ToList();
        }

        public async Task<SessionDetailsDTO?> GetDetailsAsync(int id)
        {
            var session = await _sessionRepository.GetByIdAsync(id);

            if (session == null)
                return null;

            var dto = new SessionDetailsDTO
            {
                Id = session.Id,
                MatchId = session.MatchId,
                Number = session.Number,
                StartAt = session.StartAt,
                EndAt = session.EndAt,
                LocationText = session.LocationText,
                ClinicRoom = session.ClinicRoom,
                Status = session.Status,
                StatusText = session.Status == 1 ? "مجدولة"
                             : session.Status == 2 ? "قيد التنفيذ"
                             : session.Status == 3 ? "مكتملة"
                             : "ملغية",
                PatientName = session.Match.PatientUser.FullName,
                StudentName = session.Match.StudentUser.FullName,
                CaseTitle = session.Match.Case.Title,
                PatientUserId = session.Match.PatientUserId,
                StudentUserId = session.Match.StudentUserId,
                HasReport = false,
                CanStart = session.Status == 1,
                CanFinish = session.Status == 2
            };

            dto.NoShowStrike = await _noShowStrikeService.GetBySessionAsync(session.Id);

            dto.PatientStrikeCount = await _noShowStrikeService
                .GetUserStrikeCountAsync(session.Match.PatientUserId);

            dto.StudentStrikeCount = await _noShowStrikeService
                .GetUserStrikeCountAsync(session.Match.StudentUserId);

            return dto;
        }

        public async Task<bool> CreateAsync(CreateSessionDTO dto)
        {
            var session = new Session
            {
                MatchId = dto.MatchId,
                Number = dto.Number,
                StartAt = dto.StartAt,
                EndAt = dto.EndAt,
                LocationText = dto.LocationText,
                ClinicRoom = dto.ClinicRoom,
                Status = 1
            };

            await _sessionRepository.AddAsync(session);
            await _sessionRepository.SaveChangesAsync();

            await _reminderService.CreateSessionRemindersAsync(session);

            var match = await _matchService.GetMatchByIdAsync(session.MatchId);
            if (match is not null)
            {
                await _notificationService.SendAsync(
                    match.PatientUserId,
                    "تم جدولة جلسة جديدة",
                    $"تم تحديد موعد جلسة جديدة لحالة \"{match.CaseTitle}\" يوم {session.StartAt:dd MMM} الساعة {session.StartAt:hh:mm tt}.",
                    type: NewSessionNotificationType, referenceId: session.Id, referenceType: "Session");
            }

            return true;
        }

        public async Task<bool> UpdateAsync(UpdateSessionDTO dto)
        {
            var session = await _sessionRepository.GetByIdAsync(dto.Id);

            if (session == null)
                return false;

            if (session.Status != 1)
                return false;

            var oldStartAt = session.StartAt;

            session.StartAt = dto.StartAt;
            session.EndAt = dto.EndAt;
            session.LocationText = dto.LocationText;
            session.ClinicRoom = dto.ClinicRoom;
            session.UpdatedAt = DateTime.UtcNow;

            _sessionRepository.Update(session);
            await _sessionRepository.SaveChangesAsync();

            await _reminderService.UpdateSessionRemindersAsync(session, oldStartAt);

            return true;
        }

        public async Task<bool> StartAsync(int id)
        {
            var session = await _sessionRepository.GetByIdAsync(id);

            if (session == null)
                return false;

            if (session.Status != 1)
                return false;

            session.Status = 2;
            session.UpdatedAt = DateTime.UtcNow;

            _sessionRepository.Update(session);
            await _sessionRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> FinishAsync(int id)
        {
            var session = await _sessionRepository.GetByIdAsync(id);

            if (session == null)
                return false;

            if (session.Status != 2)
                return false;

            session.Status = 3;
            session.UpdatedAt = DateTime.UtcNow;

            _sessionRepository.Update(session);
            await _sessionRepository.SaveChangesAsync();

            await _matchService.CompleteMatchAsync(session.MatchId);

            return true;
        }

        public async Task<bool> CancelAsync(int id, int cancelledByUserId, string reason)
        {
            var session = await _sessionRepository.GetByIdAsync(id);

            if (session == null)
                return false;

            if (session.Status == 3)
                return false;

            session.Status = 4;
            session.CancelledByUserId = cancelledByUserId;
            session.CancelReason = reason;
            session.UpdatedAt = DateTime.UtcNow;

            _sessionRepository.Update(session);
            await _sessionRepository.SaveChangesAsync();

            await _reminderService.DeletePendingRemindersAsync(id);

            return true;
        }
    }
}