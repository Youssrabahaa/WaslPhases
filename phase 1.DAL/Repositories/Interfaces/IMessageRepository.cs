using System.Threading.Tasks;
using phase_1.DAL.Models;

namespace phase_1.DAL.Repositories.Interfaces
{
    public interface IMessageRepository
    {
        Task AddAsync(Message message);
        Task SaveChangesAsync();
    }
}