using CG.DTO.JobPostingSkill;
using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class JobPostingSkillService : IJobPostingSkillService
    {
        private readonly IJobPostingSkillRepository _jobPostingSkillRepository;

        public JobPostingSkillService(IJobPostingSkillRepository jobPostingSkillRepository)
        {
            _jobPostingSkillRepository = jobPostingSkillRepository;
        }

        private async Task<JobPostingSkill> GetByIdOrThrowAsync(int id)
        {
            var jobPostingSkill = await _jobPostingSkillRepository.GetByIdAsync(id);

            if (jobPostingSkill is null)
                throw new NotFoundException($"Job posting skill with id '{id}' does not exist.");
            
            return jobPostingSkill;
        }

        private static JobPostingSkillDto ToDto(JobPostingSkill jobPostingSkill)
        {
            return new JobPostingSkillDto
            {
                Id = jobPostingSkill.Id,
                JobPostingId = jobPostingSkill.JobPostingId,
                SkillId = jobPostingSkill.SkillId,
                IsRequired = jobPostingSkill.IsRequired
            };
        }

        public async Task<JobPostingSkillDto> GetByIdAsync(int id)
        {
            var jobPostingSkill = await GetByIdOrThrowAsync(id);
            return ToDto(jobPostingSkill);
        }

        public async Task<IEnumerable<JobPostingSkillDto>> GetAllByJobPostingIdAsync(int jobPostingId)
        {
            var jobPostingSkills = await _jobPostingSkillRepository.GetAllByJobPostingIdAsync(jobPostingId);
            return jobPostingSkills.Select(ToDto);
        }

        public async Task<IEnumerable<JobPostingSkillDto>> GetAllAsync()
        {
            var jobPostingSkills = await _jobPostingSkillRepository.GetAllAsync();
            return jobPostingSkills.Select(ToDto);
        }

        public async Task<JobPostingSkillDto> CreateAsync(JobPostingSkillCreateDto request)
        {
            var newJobPostingSkill = new JobPostingSkill
            {
                JobPostingId = request.JobPostingId,
                SkillId = request.SkillId,
                IsRequired = request.IsRequired
            };

            await _jobPostingSkillRepository.AddAsync(newJobPostingSkill);
            await _jobPostingSkillRepository.SaveChangesAsync();
            return ToDto(newJobPostingSkill);
        }

        public async Task<JobPostingSkillDto> ToggleRequiredAsync(int id)
        {
            var jobPostingSkill = await GetByIdOrThrowAsync(id);
            jobPostingSkill.IsRequired = !jobPostingSkill.IsRequired;

            _jobPostingSkillRepository.Update(jobPostingSkill);
            await _jobPostingSkillRepository.SaveChangesAsync();
            return ToDto(jobPostingSkill);
        }

        public async Task DeleteAsync(int id)
        {
            var jobPostingSkill = await GetByIdOrThrowAsync(id);

            _jobPostingSkillRepository.Delete(jobPostingSkill);
            await _jobPostingSkillRepository.SaveChangesAsync();
        }
    }
}