using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using phase_1.DAL.Models;

namespace phase_1.DAL.Repositories.Interfaces
{
    public interface IReminderRepository
    {
        Task<List<Reminder>> GetBySessionIdAsync(int sessionId);

        Task<Reminder?> GetByIdAsync(int id);

        Task AddAsync(Reminder reminder);

        void Update(Reminder reminder);
        void Delete(Reminder reminder);

        Task SaveChangesAsync();
    }
}
