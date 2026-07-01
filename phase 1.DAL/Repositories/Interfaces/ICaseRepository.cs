using phase_1.DAL.Models;

namespace phase_1.DAL.Repositories.Interfaces
{
    public interface ICaseRepository
    {
        Task<Case?> GetByIdAsync(int id);
        Task<List<Case>> GetByPatientIdAsync(int patientId);
        Task<List<Case>> GetAllOpenAsync();
        Task AddAsync(Case c);
        Task UpdateAsync(Case c);
        Task SaveChangesAsync();
    }
}
