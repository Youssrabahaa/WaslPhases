using phase_1.BLL.DTOs;
using phase_1.DAL.Models;
using phase_1.DAL.Repositories.Interfaces;

namespace phase_1.BLL.Services
{
    public class CaseService : ICaseService
    {
        private readonly ICaseRepository _caseRepository;

        public CaseService(ICaseRepository caseRepository)
        {
            _caseRepository = caseRepository;
        }

        public async Task<CaseDetailsDTO?> GetByIdAsync(int id)
        {
            var c = await _caseRepository.GetByIdAsync(id);

            if (c == null)
                return null;

            return new CaseDetailsDTO
            {
                Id = c.Id,
                PatientUserId = c.PatientUserId,
                PatientName = c.PatientUser.FullName,
                ServiceTypeId = c.ServiceTypeId,
                ServiceTypeName = c.ServiceType.Name,
                TreatmentCategoryId = c.TreatmentCategoryId,
                TreatmentCategoryName = c.TreatmentCategory.Name,
                Title = c.Title,
                Description = c.Description,
                Urgency = c.Urgency,
                Status = c.Status,
                StatusText = c.Status == 1 ? "مفتوحة"
                           : c.Status == 2 ? "مطابقة"
                           : "مغلقة",
                EstimatedPriceMin = c.EstimatedPriceMin,
                EstimatedPriceMax = c.EstimatedPriceMax,
                NeedsSupervisorApproval = c.NeedsSupervisorApproval,
                Governorate = c.Governorate,
                City = c.City,
                Area = c.Area,
                CreatedAt = c.CreatedAt,
                OffersCount = c.Offers.Count
            };
        }

        public async Task<List<CaseListDTO>> GetPatientCasesAsync(int patientId)
        {
            var cases = await _caseRepository.GetByPatientIdAsync(patientId);

            return cases.Select(c => new CaseListDTO
            {
                Id = c.Id,
                Title = c.Title,
                ServiceTypeName = c.ServiceType.Name,
                TreatmentCategoryName = c.TreatmentCategory.Name,
                Status = c.Status,
                StatusText = c.Status == 1 ? "مفتوحة"
                           : c.Status == 2 ? "مطابقة"
                           : "مغلقة",
                Urgency = c.Urgency,
                Governorate = c.Governorate,
                City = c.City,
                CreatedAt = c.CreatedAt,
                OffersCount = c.Offers.Count
            }).ToList();
        }

        public async Task<List<CaseListDTO>> GetOpenCasesAsync()
        {
            var cases = await _caseRepository.GetAllOpenAsync();

            return cases.Select(c => new CaseListDTO
            {
                Id = c.Id,
                Title = c.Title,
                ServiceTypeName = c.ServiceType.Name,
                TreatmentCategoryName = c.TreatmentCategory.Name,
                Status = c.Status,
                StatusText = "مفتوحة",
                Urgency = c.Urgency,
                Governorate = c.Governorate,
                City = c.City,
                CreatedAt = c.CreatedAt,
                OffersCount = c.Offers.Count
            }).ToList();
        }

        public async Task<bool> CreateAsync(CreateCaseDTO dto)
        {
            var c = new Case
            {
                PatientUserId = dto.PatientUserId,
                ServiceTypeId = dto.ServiceTypeId,
                TreatmentCategoryId = dto.TreatmentCategoryId,
                Title = dto.Title,
                Description = dto.Description,
                Urgency = dto.Urgency,
                Status = 1,   // Open
                EstimatedPriceMin = dto.EstimatedPriceMin,
                EstimatedPriceMax = dto.EstimatedPriceMax,
                NeedsSupervisorApproval = dto.NeedsSupervisorApproval,
                Governorate = dto.Governorate,
                City = dto.City,
                Area = dto.Area,
                CreatedAt = DateTime.UtcNow
            };

            await _caseRepository.AddAsync(c);
            await _caseRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateAsync(UpdateCaseDTO dto)
        {
            var c = await _caseRepository.GetByIdAsync(dto.Id);

            if (c == null)
                return false;

            // ✅ لا يجوز تعديل الحالة لو مش Open
            if (c.Status != 1)
                return false;

            c.ServiceTypeId = dto.ServiceTypeId;
            c.TreatmentCategoryId = dto.TreatmentCategoryId;
            c.Title = dto.Title;
            c.Description = dto.Description;
            c.Urgency = dto.Urgency;
            c.EstimatedPriceMin = dto.EstimatedPriceMin;
            c.EstimatedPriceMax = dto.EstimatedPriceMax;
            c.NeedsSupervisorApproval = dto.NeedsSupervisorApproval;
            c.Governorate = dto.Governorate;
            c.City = dto.City;
            c.Area = dto.Area;

            await _caseRepository.UpdateAsync(c);
            await _caseRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CloseAsync(int caseId, int patientUserId)
        {
            var c = await _caseRepository.GetByIdAsync(caseId);

            if (c == null)
                return false;

            if (c.PatientUserId != patientUserId)
                return false;

            if (c.Status == 2)
                return false;

            c.Status = 3;
            c.ClosedAt = DateTime.UtcNow;

            await _caseRepository.UpdateAsync(c);
            await _caseRepository.SaveChangesAsync();

            return true;
        }
    }
}