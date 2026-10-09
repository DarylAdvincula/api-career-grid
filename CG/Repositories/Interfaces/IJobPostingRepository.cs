using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface IJobPostingRepository
    {
        Task<JobPosting?> GetByIdAsync(int id);
        Task<IEnumerable<JobPosting>> GetAllByCompanyIdAsync(int companyId);
        Task<IEnumerable<JobPosting>> GetAllAsync(string? search);
        Task AddAsync(JobPosting jobPosting);
        void Update(JobPosting jobPosting);
        void Delete(JobPosting jobPosting);
        Task SaveChangesAsync();
    }
}