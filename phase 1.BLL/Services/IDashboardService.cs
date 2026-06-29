using phase_1.DTOs;

namespace phase_1.Services
{
    public interface IDashboardService
    {
        Task<DashboardStatsDTO> GetStatsAsync();
    }
}
