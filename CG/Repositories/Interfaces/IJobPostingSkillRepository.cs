using CG.Models;

namespace CG.Repositories.Interfaces
{
    public interface IJobPostingSkillRepository
    {
        Task<JobPostingSkill?> GetByIdAsync(int id);
        Task<IEnumerable<JobPostingSkill>> GetAllAsync();
        Task AddAsync(JobPostingSkill jobPostingSkill);
        void Update(JobPostingSkill jobPostingSkill);
        void Delete(JobPostingSkill jobPostingSkill);
        public Task SaveChangesAsync();
    }
}