using phase_1.DAL.Models;

namespace phase_1.SeedData;

public static class CaseSeed
{
    public static readonly Case[] Data =
    {
        new()
        {
            Id = SeedIds.Cases.ChildSpeechCase,
            PatientUserId = SeedIds.Users.PatientMona,
            ServiceTypeId = SeedIds.ServiceTypes.AtHome,
            TreatmentCategoryId = SeedIds.TreatmentCategories.PediatricSpeech,
            Title = "Follow-up for a child with multiple dental caries",
            Description = "The family is looking for a supervised dental student to help with examination, preventive guidance, and a treatment plan for a 6-year-old child.",
            Urgency = 2,
            Status = 2,
            EstimatedPriceMin = 180.00m,
            EstimatedPriceMax = 250.00m,
            NeedsSupervisorApproval = true,
            CreatedAt = new DateTime(2026, 4, 1, 16, 0, 0, DateTimeKind.Utc),
            Governorate = "Cairo",
            City = "Nasr City",
            Area = "Seventh District"
        },
        new()
        {
            Id = SeedIds.Cases.AdultRehabCase,
            PatientUserId = SeedIds.Users.PatientYoussef,
            ServiceTypeId = SeedIds.ServiceTypes.InClinic,
            TreatmentCategoryId = SeedIds.TreatmentCategories.PostStroke,
            Title = "Restorative dental treatment for damaged molars",
            Description = "The patient needs an in-clinic supervised student for assessment and restoration planning for painful posterior teeth.",
            Urgency = 3,
            Status = 1,
            EstimatedPriceMin = 220.00m,
            EstimatedPriceMax = 320.00m,
            NeedsSupervisorApproval = true,
            CreatedAt = new DateTime(2026, 4, 3, 12, 30, 0, DateTimeKind.Utc),
            Governorate = "Cairo",
            City = "Maadi",
            Area = "Zahraa El Maadi"
        }
    };
}