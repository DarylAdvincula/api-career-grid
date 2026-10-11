using CG.DTO.JobPostingSkill;

namespace CG.Services.Interfaces
{
    public interface IJobPostingSkillService
    {
        Task<JobPostingSkillDto> GetByIdAsync(int id);

        Task<IEnumerable<JobPostingSkillDto>> GetAllByJobPostingIdAsync(int jobPostingId);

        Task<IEnumerable<JobPostingSkillDto>> GetAllAsync();

        Task<JobPostingSkillDto> CreateAsync(JobPostingSkillCreateDto request);

        Task<JobPostingSkillDto> ToggleRequiredAsync(int id);

        Task DeleteAsync(int id);
    }
}