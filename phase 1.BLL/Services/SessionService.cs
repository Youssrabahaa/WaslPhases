using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using phase_1.BLL.DTOs;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories;
using phase_1.DAL.Repositories.Interfaces;

namespace phase_1.BLL.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IMatchRepository _matchRepository;
        private readonly INoShowStrikeService _noShowStrikeService;
        private readonly IReminderService _reminderService;

        public SessionService(
            ISessionRepository sessionRepository,
            IMatchRepository matchRepository,
            INoShowStrikeService noShowStrikeService,
            IReminderService reminderService)
        {
            _sessionRepository = sessionRepository;
            _matchRepository = matchRepository;
            _noShowStrikeService = noShowStrikeService;
            _reminderService = reminderService;
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
            if (dto.MatchId <= 0 || dto.StartAt == default || dto.EndAt == default || dto.EndAt <= dto.StartAt)
                return false;

            var match = await _matchRepository.GetByIdAsync(dto.MatchId);
            if (match == null || match.Status != 1)
                return false;

            var session = new Session
            {
                MatchId = dto.MatchId,
                Number = await _sessionRepository.GetNextNumberAsync(dto.MatchId),
                StartAt = dto.StartAt,
                EndAt = dto.EndAt,
                LocationText = dto.LocationText,
                ClinicRoom = dto.ClinicRoom,
                Status = 1
            };

            await _sessionRepository.AddAsync(session);
            await _sessionRepository.SaveChangesAsync();

            // ✅ Reminders تتنشأ تلقائيًا بعد الحفظ
            await _reminderService.CreateSessionRemindersAsync(session);

            return true;
        }

        public async Task<bool> UpdateAsync(UpdateSessionDTO dto)
        {
            var session = await _sessionRepository.GetByIdAsync(dto.Id);

            if (session == null)
                return false;

            // ✅ إصلاح: التعديل مسموح فقط لو Scheduled
            if (session.Status != 1)
                return false;

            // نحفظ القديم قبل التغيير عشان نقارن في الـ Reminder
            var oldStartAt = session.StartAt;

            session.StartAt = dto.StartAt;
            session.EndAt = dto.EndAt;
            session.LocationText = dto.LocationText;
            session.ClinicRoom = dto.ClinicRoom;
            session.UpdatedAt = DateTime.UtcNow;

            _sessionRepository.Update(session);
            await _sessionRepository.SaveChangesAsync();

            // ✅ إصلاح: بنمرر oldStartAt — الـ Service يقارن ويقرر
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

            return true;
        }

        public async Task<bool> CancelAsync(int id, int cancelledByUserId, string reason)
        {
            var session = await _sessionRepository.GetByIdAsync(id);

            if (session == null)
                return false;

            // مش مسموح تكنسل Completed
            if (session.Status == 3)
                return false;

            session.Status = 4;
            session.CancelledByUserId = cancelledByUserId;
            session.CancelReason = reason;
            session.UpdatedAt = DateTime.UtcNow;

            _sessionRepository.Update(session);
            await _sessionRepository.SaveChangesAsync();

            // ✅ إصلاح: احذف Pending reminders بعد الكنسلة
            await _reminderService.DeletePendingRemindersAsync(id);

            return true;
        }
    }
}
