namespace phase_1.BLL.DTOs
{
    public class CaseListDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ServiceTypeName { get; set; } = string.Empty;
        public string TreatmentCategoryName { get; set; } = string.Empty;
        public int Status { get; set; }
        public string StatusText { get; set; } = string.Empty;
        public int Urgency { get; set; }
        public string Governorate { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int OffersCount { get; set; }
    }
}