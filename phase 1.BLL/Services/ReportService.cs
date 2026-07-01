using phase_1.BLL.DTOs;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories.Interfaces;

namespace phase_1.BLL.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly ISessionRepository _sessionRepository;

        public ReportService(
            IReportRepository reportRepository,
            ISessionRepository sessionRepository)
        {
            _reportRepository = reportRepository;
            _sessionRepository = sessionRepository;
        }

        public async Task<ReportDTO> CreateReportAsync(int reporterUserId, CreateReportDTO dto)
        {
            var session = await _sessionRepository.GetByIdAsync(dto.SessionId)
                ?? throw new InvalidOperationException("Session not found.");

            var patientId = session.Match.PatientUserId;
            var studentId = session.Match.StudentUserId;

            if (reporterUserId != patientId && reporterUserId != studentId)
                throw new UnauthorizedAccessException("You are not a participant of this session.");

            if (dto.ReportedUserId == reporterUserId)
                throw new InvalidOperationException("You cannot report yourself.");

            if (dto.ReportedUserId != patientId && dto.ReportedUserId != studentId)
                throw new InvalidOperationException("The reported user is not a participant of this session.");

            var report = new Report
            {
                SessionId = dto.SessionId,
                ReporterUserId = reporterUserId,
                ReportedUserId = dto.ReportedUserId,
                Type = dto.Type,
                Description = dto.Description,
                Status = 1,
                CreatedAt = DateTime.UtcNow
            };

            await _reportRepository.AddAsync(report);
            await _reportRepository.SaveChangesAsync();

            return new ReportDTO
            {
                Id = report.Id,
                SessionId = report.SessionId,
                ReporterUserId = report.ReporterUserId,
                ReportedUserId = report.ReportedUserId,
                Type = report.Type,
                Description = report.Description,
                Status = report.Status,
                CreatedAt = report.CreatedAt
            };
        }

        public async Task<ReportDTO> GetByIdAsync(int reportId)
        {
            var report = await _reportRepository.GetByIdWithDetailsAsync(reportId)
                ?? throw new InvalidOperationException("Report not found.");

            return new ReportDTO
            {
                Id = report.Id,
                SessionId = report.SessionId,
                ReporterUserId = report.ReporterUserId,
                ReporterName = report.ReporterUser?.FullName ?? string.Empty,
                ReportedUserId = report.ReportedUserId,
                ReportedName = report.ReportedUser?.FullName ?? string.Empty,
                Type = report.Type,
                Description = report.Description,
                Status = report.Status,
                CreatedAt = report.CreatedAt,
                ResolvedAt = report.ResolvedAt
            };
        }

        public async Task<List<ReportDTO>> GetAllAsync()
        {
            var reports = await _reportRepository.GetAllAsync();

            return reports.Select(r => new ReportDTO
            {
                Id = r.Id,
                SessionId = r.SessionId,
                ReporterUserId = r.ReporterUserId,
                ReporterName = r.ReporterUser?.FullName ?? string.Empty,
                ReportedUserId = r.ReportedUserId,
                ReportedName = r.ReportedUser?.FullName ?? string.Empty,
                Type = r.Type,
                Description = r.Description,
                Status = r.Status,
                CreatedAt = r.CreatedAt,
                ResolvedAt = r.ResolvedAt
            }).ToList();
        }

        public async Task<List<ReportDTO>> GetByStatusAsync(int status)
        {
            var reports = await _reportRepository.GetByStatusAsync(status);

            return reports.Select(r => new ReportDTO
            {
                Id = r.Id,
                SessionId = r.SessionId,
                ReporterUserId = r.ReporterUserId,
                ReporterName = r.ReporterUser?.FullName ?? string.Empty,
                ReportedUserId = r.ReportedUserId,
                ReportedName = r.ReportedUser?.FullName ?? string.Empty,
                Type = r.Type,
                Description = r.Description,
                Status = r.Status,
                CreatedAt = r.CreatedAt,
                ResolvedAt = r.ResolvedAt
            }).ToList();
        }

        public async Task<List<ReportDTO>> GetMyReportsAsync(int userId)
        {
            var reports = await _reportRepository.GetByReporterIdAsync(userId);

            return reports.Select(r => new ReportDTO
            {
                Id = r.Id,
                SessionId = r.SessionId,
                ReporterUserId = r.ReporterUserId,
                ReportedUserId = r.ReportedUserId,
                Type = r.Type,
                Description = r.Description,
                Status = r.Status,
                CreatedAt = r.CreatedAt,
                ResolvedAt = r.ResolvedAt
            }).ToList();
        }

        public async Task<ReportDTO> ResolveReportAsync(int reportId, ResolveReportDTO dto)
        {
            var report = await _reportRepository.GetByIdAsync(reportId)
                ?? throw new InvalidOperationException("Report not found.");

            if (report.Status == 2 || report.Status == 3)
                throw new InvalidOperationException("This report has already been closed.");

            if (dto.NewStatus != 2 && dto.NewStatus != 3)
                throw new InvalidOperationException("Invalid status. Must be Resolved(2) or Rejected(3).");

            report.Status = dto.NewStatus;
            report.ResolvedAt = DateTime.UtcNow;

            await _reportRepository.UpdateAsync(report);
            await _reportRepository.SaveChangesAsync();

            return new ReportDTO
            {
                Id = report.Id,
                SessionId = report.SessionId,
                ReporterUserId = report.ReporterUserId,
                ReportedUserId = report.ReportedUserId,
                Type = report.Type,
                Description = report.Description,
                Status = report.Status,
                CreatedAt = report.CreatedAt,
                ResolvedAt = report.ResolvedAt
            };
        }
    }
}