using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface IApplicationStatusLogRepository
    {
        Task<ApplicationStatusLog?> GetByIdAsync(int id);
        Task<IEnumerable<ApplicationStatusLog>> GetAllAsync();
        Task AddAsync(ApplicationStatusLog applicationStatusLog);
        void Update(ApplicationStatusLog applicationStatusLog);
        void Delete(ApplicationStatusLog applicationStatusLog);
        Task SaveChangesAsync();
    }
}