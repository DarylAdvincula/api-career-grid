using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface IJobClassificationRepository
    {
        Task<JobClassification?> GetByIdAsync(int id);
        Task<JobClassification?> GetByNameAsync(string Name);
        Task<IEnumerable<JobClassification>> GetAllAsync();
        Task AddAsync(JobClassification jobClassification);
        void Update(JobClassification jobClassification);
        void Delete(JobClassification jobClassification);
        Task SaveChangesAsync();
    }
}