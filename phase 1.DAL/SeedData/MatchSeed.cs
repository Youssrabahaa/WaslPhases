using phase_1.DAL.Models;
using Microsoft.EntityFrameworkCore;
using phase_1.DAL.Models;
 
namespace phase_1.SeedData;

public static class MatchSeed
{
    public static readonly Offer[] Offers =
    {
        new()
        {
            Id              = 1,
            CaseId          = SeedIds.Cases.ChildSpeechCase,
            StudentUserId   = SeedIds.Users.StudentAhmed,
            Message         = "أقترح البدء بتقييم داخل العيادة ثم جلستين علاجيتين حسب نتيجة الفحص.",
            ProposedPrice   = 220.00m,
            EstimatedSessionsCount = 3,
            Status          = 2,   // Accepted
            CreatedAt       = new DateTime(2026, 4, 10, 10, 0, 0, DateTimeKind.Utc),
            DecidedAt       = new DateTime(2026, 4, 11, 12, 0, 0, DateTimeKind.Utc)
        }
    };

   
    public static readonly Match[] Matches =
    {
        new()
        {
            Id             = 1,
            OfferId        = 1,
            CaseId         = SeedIds.Cases.ChildSpeechCase,
            PatientUserId  = SeedIds.Users.PatientMona,
            StudentUserId  = SeedIds.Users.StudentAhmed,
            Status         = 1,   // Active
            CreatedAt      = new DateTime(2026, 4, 11, 12, 0, 0, DateTimeKind.Utc),
            AcceptedAt     = new DateTime(2026, 4, 11, 12, 0, 0, DateTimeKind.Utc)
        }
    };

    public static readonly Conversation[] Conversations =
    {
        new()
        {
            Id        = 1,
            MatchId   = 1,
            CreatedAt = new DateTime(2026, 4, 11, 12, 0, 0, DateTimeKind.Utc)
        }
    };
}