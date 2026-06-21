using phase_1.Models;

namespace phase_1.SeedData;

public static class ReferenceSeed
{
    public static readonly University[] Universities =
    {
        new()
        {
            Id = SeedIds.Universities.CairoUniversity,
            Name = "Cairo University",
            Governorate = "Cairo",
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = SeedIds.Universities.AlexandriaUniversity,
            Name = "Alexandria University",
            Governorate = "Alexandria",
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        }
    };

    public static readonly Faculty[] Faculties =
    {
        new()
        {
            Id = SeedIds.Faculties.CairoDentistry,
            Name = "Faculty of Dentistry",
            Location = "Giza",
            UniversityId = SeedIds.Universities.CairoUniversity,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        },
        new()
        {
            Id = SeedIds.Faculties.AlexandriaDentistry,
            Name = "Faculty of Dentistry",
            Location = "El Azareeta",
            UniversityId = SeedIds.Universities.AlexandriaUniversity,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        }
    };

    public static readonly ServiceType[] ServiceTypes =
    {
        new()
        {
            Id = SeedIds.ServiceTypes.AtHome,
            Name = "Home Visit",
            IsActive = true
        },
        new()
        {
            Id = SeedIds.ServiceTypes.InClinic,
            Name = "In-Clinic Session",
            IsActive = true
        },
        new()
        {
            Id = SeedIds.ServiceTypes.OnlineFollowUp,
            Name = "Online Follow-Up",
            IsActive = true
        }
    };

    public static readonly TreatmentCategory[] TreatmentCategories =
    {
        new()
        {
            Id = SeedIds.TreatmentCategories.PediatricSpeech,
            Name = "Pediatric Dentistry",
            Description = "Dental care cases for children, including preventive treatment and early interventions.",
            IsActive = true
        },
        new()
        {
            Id = SeedIds.TreatmentCategories.PostStroke,
            Name = "Restorative Dentistry",
            Description = "Cases involving fillings, simple restorations, and treatment plans to repair tooth structure.",
            IsActive = true
        },
        new()
        {
            Id = SeedIds.TreatmentCategories.Stuttering,
            Name = "Oral Surgery",
            Description = "Cases that may require extractions or minor oral surgical procedures under supervision.",
            IsActive = true
        }
    };
}
