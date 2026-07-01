using phase_1.BLL.DTOs;

namespace phase_1.BLL.Services
{
    public interface ICaseService
    {
        Task<CaseDetailsDTO?> GetByIdAsync(int id);
        Task<List<CaseListDTO>> GetPatientCasesAsync(int patientId);
        Task<List<CaseListDTO>> GetOpenCasesAsync();
        Task<bool> CreateAsync(CreateCaseDTO dto);
        Task<bool> UpdateAsync(UpdateCaseDTO dto);
        Task<bool> CloseAsync(int caseId, int patientUserId);
    }
}