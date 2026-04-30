using phase_1.ViewModels.PatientPortal;

namespace phase_1.Services;

public interface IPatientPortalService
{
    PatientPortalDashboardViewModel GetDashboard();
    PatientCasesPageViewModel GetCases();
    PatientCaseDetailsViewModel? GetCaseDetails(int id);
    PatientMatchDetailsViewModel? GetMatchDetails(int id);
    PatientSessionsPageViewModel GetSessions();
    PatientSessionDetailsViewModel? GetSessionDetails(int id);
    PatientReviewsPageViewModel GetReviews();
    PatientAddReviewViewModel? GetAddReviewModel(int? matchId);
}
