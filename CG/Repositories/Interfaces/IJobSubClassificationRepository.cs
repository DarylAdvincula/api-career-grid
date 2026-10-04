using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface IJobSubClassificationRepository
    {
        Task<JobSubClassification?> GetByIdAsync(int id);
        Task<IEnumerable<JobSubClassification>> GetAllAsync();
        Task AddAsync(JobSubClassification jobSubClassification);
        void Update(JobSubClassification jobSubClassification);
        void Delete(JobSubClassification jobSubClassification);
        Task SaveChangesAsync();
    }
}