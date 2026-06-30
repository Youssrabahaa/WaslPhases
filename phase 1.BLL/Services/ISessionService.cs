using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using phase_1.BLL.DTOs;

namespace phase_1.BLL.Services
{
    public interface ISessionService
    {
        Task<List<SessionDTO>> GetByMatchAsync(int matchId);

        Task<SessionDetailsDTO?> GetDetailsAsync(int id);

        Task<bool> CreateAsync(CreateSessionDTO dto);

        Task<bool> UpdateAsync(UpdateSessionDTO dto);

        Task<bool> StartAsync(int id);

        Task<bool> FinishAsync(int id);

        Task<bool> CancelAsync(int id, int cancelledByUserId, string reason);
    }
}
