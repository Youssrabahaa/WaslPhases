using phase_1.ViewModels.PatientPortal;

namespace phase_1.Services;

public class DemoPatientPortalService : IPatientPortalService
{
    private readonly List<PatientCaseCardViewModel> _cases;
    private readonly List<PatientMatchSummaryViewModel> _matches;
    private readonly List<PatientSessionCardViewModel> _sessions;
    private readonly List<PatientReviewCardViewModel> _receivedReviews;
    private readonly List<PatientReviewCardViewModel> _submittedReviews;

    public DemoPatientPortalService()
    {
        _cases =
        [
            new PatientCaseCardViewModel
            {
                Id = 1,
                Title = "تنظيف أسنان وفحص شامل",
                Description = "أحتاج إلى تنظيف روتيني وفحص شامل للأسنان. لا توجد مشكلات كبيرة، فقط متابعة دورية.",
                Category = "طب الأسنان العام",
                ServiceType = "زيارة منزلية",
                Location = "الرياض",
                StatusLabel = "مفتوحة",
                StatusCssClass = "status-progress",
                UrgencyLabel = "عادية",
                UrgencyCssClass = "status-draft",
                OffersCount = 3,
                CreatedAtLabel = "تم الإنشاء 1447/11/3 هـ",
                HasMatch = false
            },
            new PatientCaseCardViewModel
            {
                Id = 2,
                Title = "حشو أسنان مطلوب",
                Description = "لدي تسوس في الضرس السفلي ويحتاج إلى حشو. أعاني من ألم خفيف منذ أسبوع.",
                Category = "طب الأسنان الترميمي",
                ServiceType = "في العيادة",
                Location = "الرياض",
                StatusLabel = "متطابقة",
                StatusCssClass = "status-open",
                UrgencyLabel = "متوسطة",
                UrgencyCssClass = "status-pending",
                OffersCount = 5,
                CreatedAtLabel = "تم الإنشاء 1447/11/1 هـ",
                HasMatch = true,
                MatchId = 401
            }
        ];

        _matches =
        [
            new PatientMatchSummaryViewModel
            {
                Id = 401,
                CaseId = 2,
                StudentName = "أحمد الراشد",
                StatusLabel = "مطابقة نشطة",
                StatusCssClass = "status-open",
                AgreedPrice = 300,
                SessionsCount = 3,
                OfferMessage = "لدي خبرة في الحشوات المركبة وسأحرص على أن تكون الجلسات مريحة وواضحة لك.",
            }
        ];

        _sessions =
        [
            new PatientSessionCardViewModel
            {
                Id = 601,
                MatchId = 401,
                CaseId = 2,
                Number = 1,
                Title = "الاستشارة الأولية",
                StatusLabel = "scheduled",
                StatusCssClass = "soft-tag-blue",
                DateLabel = "Tuesday, April 28, 2026 at 05:00 PM",
                Location = "العيادة التعليمية",
                Room = "B-12",
                SupervisorName = "د. هالة سمير",
                Notes = "جلسة تقييم أولي ووضع الخطة العلاجية النهائية.",
                CanAddReview = false
            },
            new PatientSessionCardViewModel
            {
                Id = 602,
                MatchId = 401,
                CaseId = 2,
                Number = 2,
                Title = "إجراء الحشو",
                StatusLabel = "scheduled",
                StatusCssClass = "soft-tag-blue",
                DateLabel = "Tuesday, May 5, 2026 at 05:00 PM",
                Location = "العيادة التعليمية",
                Room = "B-12",
                SupervisorName = "د. هالة سمير",
                Notes = "تنفيذ الحشو تحت إشراف مباشر مع مراجعة الإطباق في نهاية الجلسة.",
                CanAddReview = false
            },
            new PatientSessionCardViewModel
            {
                Id = 603,
                MatchId = 401,
                CaseId = 2,
                Number = 0,
                Title = "تنظيف الأسنان",
                StatusLabel = "completed",
                StatusCssClass = "soft-tag-teal",
                DateLabel = "Wednesday, April 15, 2026 at 12:00 PM",
                Location = "العيادة التعليمية",
                Room = "A-04",
                SupervisorName = "د. هالة سمير",
                Notes = "تمت الجلسة بنجاح مع توصيات عناية منزلية ومتابعة بعد أسبوعين.",
                CanAddReview = true
            }
        ];

        _receivedReviews =
        [
            new PatientReviewCardViewModel
            {
                Id = 701,
                MatchId = 401,
                CaseId = 2,
                StudentName = "أحمد الراشد",
                AuthorRoleLabel = "طالب",
                Rating = 5,
                Comment = "مريضة رائعة، متعاونة جدًا والتزمت بكل تعليمات ما بعد العلاج بشكل مثالي.",
                CaseTitle = "حشو أسنان مطلوب",
                CreatedAtLabel = "1447/10/28 هـ"
            }
        ];

        _submittedReviews =
        [
            new PatientReviewCardViewModel
            {
                Id = 702,
                MatchId = 401,
                CaseId = 2,
                StudentName = "أحمد الراشد",
                AuthorRoleLabel = "طالب",
                Rating = 5,
                Comment = "تجربة ممتازة! أحمد كان محترفًا جدًا وجعلني أشعر بالراحة طوال الإجراء.",
                CaseTitle = "حشو أسنان مطلوب",
                CreatedAtLabel = "1447/10/27 هـ"
            },
            new PatientReviewCardViewModel
            {
                Id = 703,
                MatchId = 401,
                CaseId = 1,
                StudentName = "أحمد الراشد",
                AuthorRoleLabel = "طالب",
                Rating = 4,
                Comment = "خدمة جيدة بشكل عام، والمتابعة كانت واضحة ومنظمة.",
                CaseTitle = "تنظيف أسنان وفحص شامل",
                CreatedAtLabel = "1447/10/23 هـ"
            }
        ];
    }

    public PatientPortalDashboardViewModel GetDashboard()
    {
        return new PatientPortalDashboardViewModel
        {
            PatientName = "Guest",
            TotalCases = _cases.Count,
            OpenCases = _cases.Count(x => !x.HasMatch),
            ActiveMatches = _matches.Count,
            UpcomingSessions = _sessions.Count(x => x.StatusLabel == "scheduled"),
            RecentCases = _cases.Take(2).ToList(),
            NextSessions = _sessions.Where(x => x.StatusLabel == "scheduled").Take(2).ToList()
        };
    }

    public PatientCasesPageViewModel GetCases()
    {
        return new PatientCasesPageViewModel
        {
            Cases = _cases
        };
    }

    public PatientCaseDetailsViewModel? GetCaseDetails(int id)
    {
        var selectedCase = _cases.FirstOrDefault(x => x.Id == id);
        if (selectedCase is null)
        {
            return null;
        }

        var match = selectedCase.MatchId.HasValue
            ? _matches.FirstOrDefault(x => x.Id == selectedCase.MatchId.Value)
            : null;

        return new PatientCaseDetailsViewModel
        {
            Case = selectedCase,
            Match = match,
            Timeline =
            [
                new TimelineEventViewModel
                {
                    Title = "تم إنشاء الحالة",
                    Description = "تم إرسال الطلب من بوابة المريض وإتاحته للطلاب المناسبين.",
                    TimeLabel = "10:45 ص",
                    AccentCssClass = "timeline-blue"
                },
                new TimelineEventViewModel
                {
                    Title = selectedCase.HasMatch ? "تم قبول أحد العروض" : "استلام عروض جديدة",
                    Description = selectedCase.HasMatch
                        ? "تم ربط الحالة بأحمد الراشد ويمكن الآن متابعة الجلسات."
                        : "الحالة ما زالت مفتوحة ووصلت لها عروض جديدة للمراجعة.",
                    TimeLabel = "11:20 ص",
                    AccentCssClass = "timeline-teal"
                },
                new TimelineEventViewModel
                {
                    Title = selectedCase.HasMatch ? "الجلسات مجدولة" : "بانتظار اختيار العرض المناسب",
                    Description = selectedCase.HasMatch
                        ? "تم تحديد مواعيد الجلسات الأولى ويمكنك إدارتها من صفحة الجلسات."
                        : "يمكنك مراجعة العروض واختيار الطالب الأنسب قبل جدولة أي جلسة.",
                    TimeLabel = "12:00 م",
                    AccentCssClass = "timeline-amber"
                }
            ]
        };
    }

    public PatientMatchDetailsViewModel? GetMatchDetails(int id)
    {
        var match = _matches.FirstOrDefault(x => x.Id == id);
        if (match is null)
        {
            return null;
        }

        var selectedCase = _cases.First(x => x.Id == match.CaseId);
        return new PatientMatchDetailsViewModel
        {
            Match = match,
            Case = selectedCase
        };
    }

    public PatientSessionsPageViewModel GetSessions()
    {
        return new PatientSessionsPageViewModel
        {
            UpcomingCount = _sessions.Count(x => x.StatusLabel == "scheduled"),
            CompletedCount = _sessions.Count(x => x.StatusLabel == "completed"),
            CancelledCount = _sessions.Count(x => x.StatusLabel == "cancelled"),
            Sessions = _sessions
        };
    }

    public PatientSessionDetailsViewModel? GetSessionDetails(int id)
    {
        var session = _sessions.FirstOrDefault(x => x.Id == id);
        if (session is null)
        {
            return null;
        }

        var selectedCase = _cases.First(x => x.Id == session.CaseId);
        var match = _matches.First(x => x.Id == session.MatchId);

        return new PatientSessionDetailsViewModel
        {
            Session = session,
            Case = selectedCase,
            Match = match
        };
    }

    public PatientReviewsPageViewModel GetReviews()
    {
        return new PatientReviewsPageViewModel
        {
            AverageRating = _receivedReviews.Count == 0 ? 0 : _receivedReviews.Average(x => x.Rating),
            ReceivedCount = _receivedReviews.Count,
            SubmittedCount = _submittedReviews.Count,
            ReceivedReviews = _receivedReviews,
            SubmittedReviews = _submittedReviews,
            PendingReview = new PatientReviewPromptViewModel
            {
                MatchId = 401,
                CaseId = 2,
                CaseTitle = "حشو أسنان مطلوب",
                StudentName = "أحمد الراشد"
            }
        };
    }

    public PatientAddReviewViewModel? GetAddReviewModel(int? matchId)
    {
        var selectedMatchId = matchId ?? _matches.First().Id;
        var prompt = GetReviews().PendingReview;
        if (prompt is null || prompt.MatchId != selectedMatchId)
        {
            prompt = new PatientReviewPromptViewModel
            {
                MatchId = selectedMatchId,
                CaseId = 2,
                CaseTitle = "حشو أسنان مطلوب",
                StudentName = "أحمد الراشد"
            };
        }

        return new PatientAddReviewViewModel
        {
            ReviewTarget = prompt
        };
    }
}
