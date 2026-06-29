using phase_1.Models;

namespace phase_1.Repositories
{
    public interface IReportRepository
    {
        Task<Report?> GetByIdAsync(int id);
        Task<Report?> GetByIdWithDetailsAsync(int id);
        Task<List<Report>> GetAllAsync();
        Task<List<Report>> GetByStatusAsync(ReportStatus status);
        Task<List<Report>> GetBySessionIdAsync(int sessionId);
        Task<List<Report>> GetByReporterIdAsync(string reporterId);
        Task<bool> ExistsForSessionAsync(int sessionId);
        Task AddAsync(Report report);
        Task UpdateAsync(Report report);
        Task<int> GetCountByStatusAsync(ReportStatus status);

    }
}
