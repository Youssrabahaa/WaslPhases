namespace phase_1.BLL.DTOs
{
    public class CaseDetailsDTO
    {
        public int Id { get; set; }
        public int PatientUserId { get; set; }
        public string PatientName { get; set; } = string.Empty;
        public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; } = string.Empty;
        public int TreatmentCategoryId { get; set; }
        public string TreatmentCategoryName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Urgency { get; set; }
        public int Status { get; set; }
        public string StatusText { get; set; } = string.Empty;
        public decimal? EstimatedPriceMin { get; set; }
        public decimal? EstimatedPriceMax { get; set; }
        public bool NeedsSupervisorApproval { get; set; }
        public string Governorate { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int OffersCount { get; set; }
    }
}