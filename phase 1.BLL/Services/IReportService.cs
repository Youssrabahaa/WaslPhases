using phase_1.DTOs;

namespace phase_1.Services
{
    public interface IReportService
    {
        Task<ReportDTO> CreateReportAsync(string reporterId, CreateReportDTO dto);
        Task<ReportDTO> GetByIdAsync(int reportId);
        Task<List<ReportDTO>> GetAllAsync();
        Task<List<ReportDTO>> GetByStatusAsync(Models.ReportStatus status);
        Task<List<ReportDTO>> GetMyReportsAsync(string userId);

        // عمليات الأدمن فقط
        Task<ReportDTO> ResolveReportAsync(string adminUserId, int reportId, ResolveReportDTO dto);
    }
}
