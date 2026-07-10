using System.Collections.Generic;
using System.Threading.Tasks;
using phase_1.DAL.Models;

namespace phase_1.DAL.Repositories
{
    public interface IMatchRepository
    {
        Task<Match?> GetByIdAsync(int matchId);

        Task<List<Match>> GetAllAsync();

        Task<List<Match>> GetByPatientIdAsync(int patientId);

        Task<List<Match>> GetByStudentIdAsync(int studentId);

        Task<Match?> GetByOfferIdAsync(int offerId);

        Task AddAsync(Match match);

        Task UpdateAsync(Match match);

        Task DeleteAsync(Match match);

        Task<List<Match>> GetActiveMatchesAsync();

        Task AddConversationAsync(Conversation conversation);

        Task SaveChangesAsync();
    }
}
