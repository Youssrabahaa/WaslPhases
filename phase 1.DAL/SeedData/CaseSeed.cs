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
            Status = 2,   // ? Matched (???? 1 = Open)
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
            Status = 1,   // Open
            EstimatedPriceMin = 220.00m,
            EstimatedPriceMax = 320.00m,
            NeedsSupervisorApproval = true,
            CreatedAt = new DateTime(2026, 4, 3, 12, 30, 0, DateTimeKind.Utc),
            Governorate = "Cairo",
            City = "Maadi",
            Area = "Zahraa El Maadi"
        },
        new()
        {
            Id = SeedIds.Cases.OrthoCase,
            PatientUserId = SeedIds.Users.PatientNadine,
            ServiceTypeId = SeedIds.ServiceTypes.InClinic,
            TreatmentCategoryId = SeedIds.TreatmentCategories.Stuttering,
            Title = "Orthodontic consultation and follow-up",
            Description = "Patient needs supervised assessment and a simple follow-up plan for braces discomfort.",
            Urgency = 2,
            Status = 2,
            EstimatedPriceMin = 300.00m,
            EstimatedPriceMax = 450.00m,
            NeedsSupervisorApproval = true,
            CreatedAt = new DateTime(2026, 4, 4, 10, 0, 0, DateTimeKind.Utc),
            Governorate = "Giza",
            City = "Dokki",
            Area = "Mossadak"
        },
        new()
        {
            Id = SeedIds.Cases.CleaningCase,
            PatientUserId = SeedIds.Users.PatientKarim,
            ServiceTypeId = SeedIds.ServiceTypes.InClinic,
            TreatmentCategoryId = SeedIds.TreatmentCategories.PediatricSpeech,
            Title = "Scaling and dental cleaning",
            Description = "Needs a student appointment for supervised scaling and oral hygiene instructions.",
            Urgency = 1,
            Status = 2,
            EstimatedPriceMin = 150.00m,
            EstimatedPriceMax = 220.00m,
            NeedsSupervisorApproval = true,
            CreatedAt = new DateTime(2026, 4, 5, 13, 0, 0, DateTimeKind.Utc),
            Governorate = "Alexandria",
            City = "Sidi Gaber",
            Area = "Mostafa Kamel"
        },
        new()
        {
            Id = SeedIds.Cases.RootCanalCase,
            PatientUserId = SeedIds.Users.PatientNadine,
            ServiceTypeId = SeedIds.ServiceTypes.InClinic,
            TreatmentCategoryId = SeedIds.TreatmentCategories.PostStroke,
            Title = "Root canal assessment",
            Description = "Painful molar needs supervised assessment and treatment planning.",
            Urgency = 3,
            Status = 1,
            EstimatedPriceMin = 400.00m,
            EstimatedPriceMax = 650.00m,
            NeedsSupervisorApproval = true,
            CreatedAt = new DateTime(2026, 4, 7, 9, 0, 0, DateTimeKind.Utc),
            Governorate = "Cairo",
            City = "Heliopolis",
            Area = "Korba"
        },
        new()
        {
            Id = SeedIds.Cases.BracesFollowUpCase,
            PatientUserId = SeedIds.Users.PatientKarim,
            ServiceTypeId = SeedIds.ServiceTypes.OnlineFollowUp,
            TreatmentCategoryId = SeedIds.TreatmentCategories.Stuttering,
            Title = "Braces follow-up question",
            Description = "Patient needs a short supervised follow-up and advice before the next clinic visit.",
            Urgency = 1,
            Status = 1,
            EstimatedPriceMin = 120.00m,
            EstimatedPriceMax = 180.00m,
            NeedsSupervisorApproval = false,
            CreatedAt = new DateTime(2026, 4, 8, 15, 30, 0, DateTimeKind.Utc),
            Governorate = "Giza",
            City = "6th of October",
            Area = "First District"
        }
    };
}
