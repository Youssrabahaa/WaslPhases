using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using phase_1.BLL.DTOs;

namespace phase_1.BLL.Services
{
    public interface IMatchService
    {
        Task<bool> AcceptOfferAsync(int offerId);

        Task<MatchDetailsDTO?> GetMatchByIdAsync(int matchId);

        Task<List<MatchDTO>> GetPatientMatchesAsync(int patientId);

        Task<List<MatchDTO>> GetStudentMatchesAsync(int studentId);

        Task<bool> CompleteMatchAsync(int matchId);

        Task CancelExpiredMatchesAsync();

        Task<bool> CancelMatchAsync(int matchId, int userId);
    }
}
