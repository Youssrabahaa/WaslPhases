using phase_1.BLL.DTOs;

namespace phase_1.BLL.Services
{
    public interface IReportService
    {
        Task<ReportDTO> CreateReportAsync(int reporterUserId, CreateReportDTO dto);
        Task<ReportDTO> GetByIdAsync(int reportId);
        Task<List<ReportDTO>> GetAllAsync();
        Task<List<ReportDTO>> GetByStatusAsync(int status);
        Task<List<ReportDTO>> GetMyReportsAsync(int userId);
        Task<ReportDTO> ResolveReportAsync(int reportId, ResolveReportDTO dto);
    }
}