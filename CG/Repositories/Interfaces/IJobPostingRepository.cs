using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface IJobPostingRepository
    {
        Task<JobPosting?> GetByIdAsync(int id);
        Task<IEnumerable<JobPosting>> GetAllAsync();
        Task AddAsync(JobPosting jobPosting);
        void Update(JobPosting jobPosting);
        void Delete(JobPosting jobPosting);
        Task SaveChangesAsync();
    }
}