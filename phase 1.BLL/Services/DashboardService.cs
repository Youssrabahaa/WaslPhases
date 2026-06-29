using phase_1.DTOs;
using phase_1.Models;
using phase_1.Repositories;

namespace phase_1.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IReviewRepository _reviewRepository;

        public DashboardService(
            IDashboardRepository dashboardRepository,
            IReportRepository reportRepository,
            IReviewRepository reviewRepository)
        {
            _dashboardRepository = dashboardRepository;
            _reportRepository = reportRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task<DashboardStatsDTO> GetStatsAsync()
        {
            var totalCases = await _dashboardRepository.GetTotalCasesAsync();
            var activeMatches = await _dashboardRepository.GetActiveMatchesCountAsync();
            var completedMatches = await _dashboardRepository.GetCompletedMatchesCountAsync();
            var cancelledMatches = await _dashboardRepository.GetCancelledMatchesCountAsync();

            var totalSessions = await _dashboardRepository.GetTotalSessionsAsync();
            var completedSessions = await _dashboardRepository.GetCompletedSessionsCountAsync();

            var openReports = await _reportRepository.GetCountByStatusAsync(ReportStatus.Open);
            var underReviewReports = await _reportRepository.GetCountByStatusAsync(ReportStatus.UnderReview);
            var resolvedReports = await _reportRepository.GetCountByStatusAsync(ReportStatus.Resolved);

            var totalReviews = await _reviewRepository.GetTotalReviewsCountAsync();

            var totalStrikes = await _dashboardRepository.GetTotalNoShowStrikesAsync();

            return new DashboardStatsDTO
            {
                TotalCases = totalCases,
                TotalActiveMatches = activeMatches,
                TotalCompletedMatches = completedMatches,
                TotalCancelledMatches = cancelledMatches,
                TotalSessions = totalSessions,
                TotalCompletedSessions = completedSessions,
                OpenReportsCount = openReports,
                UnderReviewReportsCount = underReviewReports,
                ResolvedReportsCount = resolvedReports,
                TotalReviews = totalReviews,
                TotalNoShowStrikes = totalStrikes
            };
        }
    }
}
