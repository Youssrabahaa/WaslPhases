using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using phase_1.BLL.DTOs;

namespace phase_1.BLL.Services
{
    public interface INoShowStrikeService
    {
        Task<bool> CreateStrikeAsync(CreateNoShowStrikeDTO dto);

        Task<NoShowStrikeDTO?> GetBySessionAsync(int sessionId);

        Task<int> GetUserStrikeCountAsync(int userId);
    }
}
