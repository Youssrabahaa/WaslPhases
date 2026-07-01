using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using phase_1.DAL.Models;

namespace phase_1.DAL.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<Report?> GetByIdAsync(int id);
        Task<Report?> GetByIdWithDetailsAsync(int id);
        Task<List<Report>> GetAllAsync();
        Task<List<Report>> GetByStatusAsync(int status);
        Task<List<Report>> GetBySessionIdAsync(int sessionId);
        Task<List<Report>> GetByReporterIdAsync(int reporterId);
        Task AddAsync(Report report);
        Task UpdateAsync(Report report);
        Task<int> GetCountByStatusAsync(int status);
        Task SaveChangesAsync();
    }
}
