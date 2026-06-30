using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using phase_1.DAL.Models;

namespace phase_1.DAL.Repositories.Interfaces
{
    public interface ISessionRepository
    {
        Task<List<Session>> GetByMatchAsync(int matchId);

        Task<Session?> GetByIdAsync(int id);

        Task AddAsync(Session session);

        void Update(Session session);

        Task SaveChangesAsync();
    }
}
