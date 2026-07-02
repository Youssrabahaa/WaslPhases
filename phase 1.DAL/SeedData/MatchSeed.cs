using phase_1.DAL.Models;
using Microsoft.EntityFrameworkCore;
 
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
        },
        new()
        {
            Id = 2,
            CaseId = SeedIds.Cases.AdultRehabCase,
            StudentUserId = SeedIds.Users.StudentSalma,
            Message = "يمكنني فحص الحالة في العيادة وتقديم خطة علاج مبدئية.",
            ProposedPrice = 280.00m,
            EstimatedSessionsCount = 2,
            Status = 1,
            CreatedAt = new DateTime(2026, 4, 12, 9, 0, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = 3,
            CaseId = SeedIds.Cases.OrthoCase,
            StudentUserId = SeedIds.Users.StudentOmar,
            Message = "أقترح جلستين متابعة مع مراجعة المشرف بعد الجلسة الأولى.",
            ProposedPrice = 380.00m,
            EstimatedSessionsCount = 2,
            Status = 2,
            CreatedAt = new DateTime(2026, 4, 13, 11, 0, 0, DateTimeKind.Utc),
            DecidedAt = new DateTime(2026, 4, 14, 10, 0, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = 4,
            CaseId = SeedIds.Cases.CleaningCase,
            StudentUserId = SeedIds.Users.StudentLaila,
            Message = "جلسة تنظيف وإرشادات عناية، ثم متابعة قصيرة لو لزم الأمر.",
            ProposedPrice = 190.00m,
            EstimatedSessionsCount = 2,
            Status = 2,
            CreatedAt = new DateTime(2026, 4, 14, 14, 0, 0, DateTimeKind.Utc),
            DecidedAt = new DateTime(2026, 4, 15, 9, 30, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = 5,
            CaseId = SeedIds.Cases.RootCanalCase,
            StudentUserId = SeedIds.Users.StudentAhmed,
            Message = "أقدر أبدأ بفحص أولي وتصوير، ثم أحدد الخطة مع المشرف.",
            ProposedPrice = 520.00m,
            EstimatedSessionsCount = 3,
            Status = 1,
            CreatedAt = new DateTime(2026, 4, 16, 12, 0, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = 6,
            CaseId = SeedIds.Cases.BracesFollowUpCase,
            StudentUserId = SeedIds.Users.StudentSalma,
            Message = "متاح لمتابعة قصيرة وتوجيه المريض قبل زيارة العيادة.",
            ProposedPrice = 150.00m,
            EstimatedSessionsCount = 1,
            Status = 1,
            CreatedAt = new DateTime(2026, 4, 17, 15, 0, 0, DateTimeKind.Utc)
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
        },
        new()
        {
            Id = 2,
            OfferId = 3,
            CaseId = SeedIds.Cases.OrthoCase,
            PatientUserId = SeedIds.Users.PatientNadine,
            StudentUserId = SeedIds.Users.StudentOmar,
            Status = 1,
            CreatedAt = new DateTime(2026, 4, 14, 10, 0, 0, DateTimeKind.Utc),
            AcceptedAt = new DateTime(2026, 4, 14, 10, 0, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = 3,
            OfferId = 4,
            CaseId = SeedIds.Cases.CleaningCase,
            PatientUserId = SeedIds.Users.PatientKarim,
            StudentUserId = SeedIds.Users.StudentLaila,
            Status = 1,
            CreatedAt = new DateTime(2026, 4, 15, 9, 30, 0, DateTimeKind.Utc),
            AcceptedAt = new DateTime(2026, 4, 15, 9, 30, 0, DateTimeKind.Utc)
        }
    };

    public static readonly Conversation[] Conversations =
    {
        new()
        {
            Id        = 1,
            MatchId   = 1,
            CreatedAt = new DateTime(2026, 4, 11, 12, 0, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = 2,
            MatchId = 2,
            CreatedAt = new DateTime(2026, 4, 14, 10, 1, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = 3,
            MatchId = 3,
            CreatedAt = new DateTime(2026, 4, 15, 9, 31, 0, DateTimeKind.Utc)
        }
    };
}
