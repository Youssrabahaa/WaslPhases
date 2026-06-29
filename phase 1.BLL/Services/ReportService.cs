using phase_1.DTOs;
using phase_1.Models;
using phase_1.Repositories;

namespace phase_1.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly ISessionRepository _sessionRepository; // موجود عند يوسرا - قراءة فقط للتحقق

        public ReportService(
            IReportRepository reportRepository,
            ISessionRepository sessionRepository,
        {
            _reportRepository = reportRepository;
            _sessionRepository = sessionRepository;
        }

        public async Task<ReportDTO> CreateReportAsync(string reporterId, CreateReportDTO dto)
        {
            var session = await _sessionRepository.GetByIdAsync(dto.SessionId)
                ?? throw new InvalidOperationException("Session not found.");

            // لازم نجيب الماتش بتاع الجلسة عشان نتأكد إن الريبورتر والمُبلَّغ عنه طرفين فعليين فيه
            var match = await _sessionRepository.GetMatchForSessionAsync(dto.SessionId)
                ?? throw new InvalidOperationException("Match not found for this session.");

            var isReporterParticipant = match.PatientId == reporterId || match.StudentId == reporterId;
            if (!isReporterParticipant)
                throw new UnauthorizedAccessException("You are not a participant of this session.");

            if (dto.ReportedUserId == reporterId)
                throw new InvalidOperationException("You cannot report yourself.");

            var isReportedParticipant = match.PatientId == dto.ReportedUserId || match.StudentId == dto.ReportedUserId;
            if (!isReportedParticipant)
                throw new InvalidOperationException("The reported user is not a participant of this session.");

            var report = new Report
            {
                SessionId = dto.SessionId,
                ReporterUserId = reporterId,
                ReportedUserId = dto.ReportedUserId,
                Type = dto.Type,
                Description = dto.Description,
                Status = ReportStatus.Open,
                CreatedAt = DateTime.UtcNow
            };

            await _reportRepository.AddAsync(report);

            return MapToDTO(report, session: null);
        }

        public async Task<ReportDTO> GetByIdAsync(int reportId)
        {
            var report = await _reportRepository.GetByIdWithDetailsAsync(reportId)
                ?? throw new InvalidOperationException("Report not found.");

            return MapToDTO(report, null);
        }

        public async Task<List<ReportDTO>> GetAllAsync()
        {
            var reports = await _reportRepository.GetAllAsync();
            return reports.Select(r => MapToDTO(r, null)).ToList();
        }

        public async Task<List<ReportDTO>> GetByStatusAsync(ReportStatus status)
        {
            var reports = await _reportRepository.GetByStatusAsync(status);
            return reports.Select(r => MapToDTO(r, null)).ToList();
        }

        public async Task<List<ReportDTO>> GetMyReportsAsync(string userId)
        {
            var reports = await _reportRepository.GetByReporterIdAsync(userId);
            return reports.Select(r => MapToDTO(r, null)).ToList();
        }

        // الأدمن بس هو اللي يقدر يستخدم الميثود دي (الـ Controller هو اللي بيتحقق من الـ Role
        // لكن منطقياً تأكيد ثاني هنا في الـ Service يحمي ضد أي استدعاء مباشر غلط)
        public async Task<ReportDTO> ResolveReportAsync(string adminUserId, int reportId, ResolveReportDTO dto)
        {
            var report = await _reportRepository.GetByIdAsync(reportId)
                ?? throw new InvalidOperationException("Report not found.");

            // مينفعش تتقفل بلاغ مقفول قبل كده
            if (report.Status == ReportStatus.Resolved || report.Status == ReportStatus.Rejected)
                throw new InvalidOperationException("This report has already been closed.");

            // الـ NewStatus المسموح بيه فقط Resolved أو Rejected عند اتخاذ إجراء
            if (dto.NewStatus != ReportStatus.Resolved && dto.NewStatus != ReportStatus.Rejected)
                throw new InvalidOperationException("Invalid resolution status. Must be Resolved or Rejected.");

            report.Status = dto.NewStatus;
            report.ResolvedAt = DateTime.UtcNow;

            await _reportRepository.UpdateAsync(report);

            var adminAction = new AdminAction
            {
                ReportId = report.Id,
                AdminUserId = adminUserId,
                ActionType = dto.ActionType,
                Notes = dto.Notes,
                CreatedAt = DateTime.UtcNow
            };

            await _reportRepository.AddAdminActionAsync(adminAction);

            // إشعار المُبلِّغ إن بلاغه تم النظر فيه
            await _notificationService.CreateAsync(new CreateNotificationDTO
            {
                UserId = report.ReporterUserId,
                Type = NotificationType.ReportStatusChanged,
                Title = "تم تحديث حالة بلاغك",
                Body = $"بلاغك أصبح: {dto.NewStatus}",
                RelatedEntityId = report.Id.ToString()
            });

            return MapToDTO(report, null);
        }

        private static ReportDTO MapToDTO(Report r, object? session)
        {
            return new ReportDTO
            {
                Id = r.Id,
                SessionId = r.SessionId,
                ReporterUserId = r.ReporterUserId,
                ReporterName = r.ReporterUser?.UserName ?? string.Empty,
                ReportedUserId = r.ReportedUserId,
                ReportedName = r.ReportedUser?.UserName ?? string.Empty,
                Type = r.Type,
                Description = r.Description,
                Status = r.Status,
                CreatedAt = r.CreatedAt,
                ResolvedAt = r.ResolvedAt
            };
        }
    }
}
