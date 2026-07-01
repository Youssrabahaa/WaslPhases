using System.ComponentModel.DataAnnotations;

namespace phase_1.BLL.DTOs
{
    public class CreateCaseDTO
    {
        [Required]
        public int PatientUserId { get; set; }

        [Required]
        public int ServiceTypeId { get; set; }

        [Required]
        public int TreatmentCategoryId { get; set; }

        [Required, MaxLength(120)]
        public string Title { get; set; } = string.Empty;

        [Required, MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        public int Urgency { get; set; } = 2;

        public decimal? EstimatedPriceMin { get; set; }
        public decimal? EstimatedPriceMax { get; set; }

        public bool NeedsSupervisorApproval { get; set; }

        [Required, MaxLength(100)]
        public string Governorate { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string City { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Area { get; set; } = string.Empty;
    }
}