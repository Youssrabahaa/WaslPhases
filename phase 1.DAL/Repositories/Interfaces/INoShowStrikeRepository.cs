using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using phase_1.DAL.Models;

namespace phase_1.DAL.Repositories.Interfaces
{
    public interface INoShowStrikeRepository
    {
        Task<NoShowStrike?> GetBySessionAsync(int sessionId);

        Task AddAsync(NoShowStrike strike);

        Task<int> GetCountByUserAsync(int userId);

        Task SaveChangesAsync();
    }
}
