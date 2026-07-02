using phase_1.DAL.Models;

namespace phase_1.SeedData;

public static class WorkflowSeed
{
    public static readonly Session[] Sessions =
    {
        new()
        {
            Id = 1,
            MatchId = 1,
            Number = 1,
            StartAt = new DateTime(2026, 7, 3, 12, 0, 0, DateTimeKind.Utc),
            EndAt = new DateTime(2026, 7, 3, 13, 0, 0, DateTimeKind.Utc),
            LocationText = "Wasl demo clinic",
            ClinicRoom = "Room A",
            Status = 1,
            CreatedAt = new DateTime(2026, 7, 1, 12, 0, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2026, 7, 1, 12, 0, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = 2,
            MatchId = 1,
            Number = 2,
            StartAt = new DateTime(2026, 7, 7, 15, 0, 0, DateTimeKind.Utc),
            EndAt = new DateTime(2026, 7, 7, 16, 0, 0, DateTimeKind.Utc),
            LocationText = "Wasl demo clinic",
            ClinicRoom = "Room B",
            Status = 1,
            CreatedAt = new DateTime(2026, 7, 1, 12, 5, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2026, 7, 1, 12, 5, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = 3,
            MatchId = 1,
            Number = 3,
            StartAt = new DateTime(2026, 6, 28, 10, 0, 0, DateTimeKind.Utc),
            EndAt = new DateTime(2026, 6, 28, 11, 0, 0, DateTimeKind.Utc),
            LocationText = "Wasl demo clinic",
            ClinicRoom = "Room C",
            Status = 3,
            CreatedAt = new DateTime(2026, 6, 25, 12, 0, 0, DateTimeKind.Utc),
            UpdatedAt = new DateTime(2026, 6, 28, 11, 5, 0, DateTimeKind.Utc)
        }
    };

    public static readonly Reminder[] Reminders =
    {
        new() { Id = 1, SessionId = 1, Channel = 1, ScheduledAt = new DateTime(2026, 7, 2, 12, 0, 0, DateTimeKind.Utc), Status = 1, CreatedAt = new DateTime(2026, 7, 1, 12, 0, 0, DateTimeKind.Utc) },
        new() { Id = 2, SessionId = 1, Channel = 1, ScheduledAt = new DateTime(2026, 7, 3, 10, 0, 0, DateTimeKind.Utc), Status = 1, CreatedAt = new DateTime(2026, 7, 1, 12, 0, 0, DateTimeKind.Utc) },
        new() { Id = 3, SessionId = 2, Channel = 1, ScheduledAt = new DateTime(2026, 7, 6, 15, 0, 0, DateTimeKind.Utc), Status = 1, CreatedAt = new DateTime(2026, 7, 1, 12, 5, 0, DateTimeKind.Utc) },
        new() { Id = 4, SessionId = 2, Channel = 1, ScheduledAt = new DateTime(2026, 7, 7, 13, 0, 0, DateTimeKind.Utc), Status = 1, CreatedAt = new DateTime(2026, 7, 1, 12, 5, 0, DateTimeKind.Utc) },
        new() { Id = 5, SessionId = 3, Channel = 1, ScheduledAt = new DateTime(2026, 6, 27, 10, 0, 0, DateTimeKind.Utc), SentAt = new DateTime(2026, 6, 27, 10, 0, 0, DateTimeKind.Utc), Status = 2, CreatedAt = new DateTime(2026, 6, 25, 12, 0, 0, DateTimeKind.Utc) },
        new() { Id = 6, SessionId = 3, Channel = 1, ScheduledAt = new DateTime(2026, 6, 28, 8, 0, 0, DateTimeKind.Utc), SentAt = new DateTime(2026, 6, 28, 8, 0, 0, DateTimeKind.Utc), Status = 2, CreatedAt = new DateTime(2026, 6, 25, 12, 0, 0, DateTimeKind.Utc) }
    };

    public static readonly Message[] Messages =
    {
        new() { Id = 1, ConversationId = 1, SenderUserId = SeedIds.Users.PatientMona, ReceiverUserId = SeedIds.Users.StudentAhmed, Content = "Hello Ahmed, I added the first session details.", SentAt = new DateTime(2026, 7, 1, 12, 10, 0, DateTimeKind.Utc), IsRead = true, ReadAt = new DateTime(2026, 7, 1, 12, 12, 0, DateTimeKind.Utc) },
        new() { Id = 2, ConversationId = 1, SenderUserId = SeedIds.Users.StudentAhmed, ReceiverUserId = SeedIds.Users.PatientMona, Content = "Great, I can see it now. I will start it from my sessions page.", SentAt = new DateTime(2026, 7, 1, 12, 14, 0, DateTimeKind.Utc), IsRead = false }
    };

    public static readonly Report[] Reports =
    {
        new() { Id = 1, SessionId = 3, ReporterUserId = SeedIds.Users.StudentAhmed, ReportedUserId = SeedIds.Users.PatientMona, Type = 3, Description = "Completed first assessment and explained the follow-up plan.", Status = 2, CreatedAt = new DateTime(2026, 6, 28, 11, 20, 0, DateTimeKind.Utc), ResolvedAt = new DateTime(2026, 6, 29, 9, 0, 0, DateTimeKind.Utc) }
    };

    public static readonly Review[] Reviews =
    {
        new() { Id = 1, MatchId = 1, ReviewerUserId = SeedIds.Users.PatientMona, ReviewedUserId = SeedIds.Users.StudentAhmed, Rating = 5, Comment = "Professional and clear communication.", CreatedAt = new DateTime(2026, 6, 29, 10, 0, 0, DateTimeKind.Utc) },
        new() { Id = 2, MatchId = 1, ReviewerUserId = SeedIds.Users.StudentAhmed, ReviewedUserId = SeedIds.Users.PatientMona, Rating = 5, Comment = "Patient was cooperative and punctual.", CreatedAt = new DateTime(2026, 6, 29, 10, 15, 0, DateTimeKind.Utc) }
    };

    public static readonly NoShowStrike[] NoShowStrikes =
    {
        new() { Id = 1, SessionId = 2, UserId = SeedIds.Users.PatientMona, Points = 1, Notes = "Demo attendance warning for testing only.", CreatedAt = new DateTime(2026, 7, 1, 13, 0, 0, DateTimeKind.Utc), ExpiresAt = new DateTime(2026, 8, 1, 13, 0, 0, DateTimeKind.Utc) }
    };

    public static readonly Notification[] Notifications =
    {
        new() { Id = 1, UserId = SeedIds.Users.StudentAhmed, Title = "جلسة جديدة", Message = "تمت إضافة جلسة جديدة لمطابقتك مع Mona.", Type = "Session", IsRead = false, CreatedAt = new DateTime(2026, 7, 1, 12, 10, 0, DateTimeKind.Utc) },
        new() { Id = 2, UserId = SeedIds.Users.PatientMona, Title = "تم تأكيد الجلسة", Message = "الجلسة الأولى ظاهرة الآن للطرفين.", Type = "Session", IsRead = false, CreatedAt = new DateTime(2026, 7, 1, 12, 11, 0, DateTimeKind.Utc) },
        new() { Id = 3, UserId = SeedIds.Users.StudentSalma, Title = "حالات جديدة", Message = "توجد حالات مفتوحة يمكنك تقديم عرض عليها.", Type = "Offer", IsRead = false, CreatedAt = new DateTime(2026, 7, 1, 9, 0, 0, DateTimeKind.Utc) }
    };
}
