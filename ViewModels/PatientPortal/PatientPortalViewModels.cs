namespace phase_1.ViewModels.PatientPortal;

public class PatientPortalDashboardViewModel
{
    public string PatientName { get; set; } = string.Empty;
    public int TotalCases { get; set; }
    public int OpenCases { get; set; }
    public int ActiveMatches { get; set; }
    public int UpcomingSessions { get; set; }
    public List<PatientCaseCardViewModel> RecentCases { get; set; } = new();
    public List<PatientSessionCardViewModel> NextSessions { get; set; } = new();
}

public class PatientCasesPageViewModel
{
    public List<PatientCaseCardViewModel> Cases { get; set; } = new();
}

public class PatientCaseDetailsViewModel
{
    public PatientCaseCardViewModel Case { get; set; } = new();
    public PatientMatchSummaryViewModel? Match { get; set; }
    public List<TimelineEventViewModel> Timeline { get; set; } = new();
}

public class PatientSessionsPageViewModel
{
    public int UpcomingCount { get; set; }
    public int CompletedCount { get; set; }
    public int CancelledCount { get; set; }
    public List<PatientSessionCardViewModel> Sessions { get; set; } = new();
}

public class PatientSessionDetailsViewModel
{
    public PatientSessionCardViewModel Session { get; set; } = new();
    public PatientCaseCardViewModel Case { get; set; } = new();
    public PatientMatchSummaryViewModel Match { get; set; } = new();
}

public class PatientReviewsPageViewModel
{
    public double AverageRating { get; set; }
    public int ReceivedCount { get; set; }
    public int SubmittedCount { get; set; }
    public List<PatientReviewCardViewModel> ReceivedReviews { get; set; } = new();
    public List<PatientReviewCardViewModel> SubmittedReviews { get; set; } = new();
    public PatientReviewPromptViewModel? PendingReview { get; set; }
}

public class PatientAddReviewViewModel
{
    public PatientReviewPromptViewModel ReviewTarget { get; set; } = new();
}

public class PatientMatchDetailsViewModel
{
    public PatientMatchSummaryViewModel Match { get; set; } = new();
    public PatientCaseCardViewModel Case { get; set; } = new();
}

public class PatientCaseCardViewModel
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string StatusLabel { get; set; } = string.Empty;
    public string StatusCssClass { get; set; } = string.Empty;
    public string UrgencyLabel { get; set; } = string.Empty;
    public string UrgencyCssClass { get; set; } = string.Empty;
    public int OffersCount { get; set; }
    public string CreatedAtLabel { get; set; } = string.Empty;
    public bool HasMatch { get; set; }
    public int? MatchId { get; set; }
}

public class PatientMatchSummaryViewModel
{
    public int Id { get; set; }
    public int CaseId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentRole { get; set; } = "طالب";
    public string StatusLabel { get; set; } = string.Empty;
    public string StatusCssClass { get; set; } = string.Empty;
    public decimal AgreedPrice { get; set; }
    public int SessionsCount { get; set; }
    public string OfferMessage { get; set; } = string.Empty;
}

public class PatientSessionCardViewModel
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public int CaseId { get; set; }
    public int Number { get; set; }
    public string Title { get; set; } = string.Empty;
    public string StatusLabel { get; set; } = string.Empty;
    public string StatusCssClass { get; set; } = string.Empty;
    public string DateLabel { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Room { get; set; } = string.Empty;
    public string SupervisorName { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public bool CanAddReview { get; set; }
}

public class PatientReviewCardViewModel
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public int CaseId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string AuthorRoleLabel { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string CaseTitle { get; set; } = string.Empty;
    public string CreatedAtLabel { get; set; } = string.Empty;
}

public class PatientReviewPromptViewModel
{
    public int MatchId { get; set; }
    public int CaseId { get; set; }
    public string CaseTitle { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
}

public class TimelineEventViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TimeLabel { get; set; } = string.Empty;
    public string AccentCssClass { get; set; } = string.Empty;
}
