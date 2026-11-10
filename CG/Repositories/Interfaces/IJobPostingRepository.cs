using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface IJobPostingRepository
    {
        Task<JobPosting?> GetByIdAsync(int id);
        Task<IEnumerable<JobPosting>> GetAllByCompanyIdAsync(int companyId);
        Task<IEnumerable<JobPosting>> GetAllAsync(
            string? search = null,
            ApprovalStatus? approvalStatus = null,
            bool? active = null
        );
        Task AddAsync(JobPosting jobPosting);
        void Update(JobPosting jobPosting);
        void Delete(JobPosting jobPosting);
        Task SaveChangesAsync();
    }
}