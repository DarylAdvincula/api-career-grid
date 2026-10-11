using CG.DTO.Skill;
using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class SkillService : ISkillService
    {
        private readonly IJobClassificationRepository _jobClassificationRepository;
        private readonly IJobSubClassificationRepository _jobSubClassificationRepository;
        private readonly ISkillRepository _skillRepository;

        public SkillService(
            IJobClassificationRepository jobClassificationRepository,
            IJobSubClassificationRepository jobSubClassificationRepository,
            ISkillRepository skillRepository
        )
        {
            _jobClassificationRepository = jobClassificationRepository;
            _jobSubClassificationRepository = jobSubClassificationRepository;
            _skillRepository = skillRepository;
        }

        private async Task<Skill> GetByIdOrThrowAsync(int id)
        {
            var skill = await _skillRepository.GetByIdAsync(id);

            if (skill is null)
                throw new NotFoundException($"Skill with id '{id}' does not exist.");
            
            return skill;
        }

        private static SkillDto ToDto(Skill skill)
        {
            return new SkillDto
            {
                Id = skill.Id,
                JobClassificationId = skill.JobClassificationId,
                JobSubClassificationId = skill.JobSubClassificationId,
                Name = skill.Name,
                CreatedAt = skill.CreatedAt
            };
        }

        public async Task<SkillDto> GetByIdAsync(int id)
        {
            var skill = await GetByIdOrThrowAsync(id);
            return ToDto(skill);
        }

        public async Task<IEnumerable<SkillDto>> GetAllAsync()
        {
            var skills = await _skillRepository.GetAllAsync();
            return skills.Select(ToDto);
        }

        public async Task<SkillDto> CreateAsync(SkillCreateDto request)
        {
            if (request.JobClassificationId is not null)
            {
                var jobClassification = await _jobClassificationRepository.GetByIdAsync((int)request.JobClassificationId);
                
                if (jobClassification is null)
                    throw new NotFoundException($"Job classification with id '{request.JobClassificationId}' does not exist.");
            }

            if (request.JobSubClassificationId is not null)
            {
                var jobSubClassification = await _jobSubClassificationRepository.GetByIdAsync((int)request.JobSubClassificationId);

                if (jobSubClassification is null)
                    throw new NotFoundException($"Job sub classification with id '{request.JobSubClassificationId}' does not exist.");
            }

            var newSkill = new Skill
            {
                JobClassificationId = request.JobClassificationId,
                JobSubClassificationId = request.JobSubClassificationId,
                Name = request.Name
            };

            await _skillRepository.AddAsync(newSkill);
            await _skillRepository.SaveChangesAsync();
            return ToDto(newSkill);
        }

        public async Task<SkillDto> UpdateAsync(int id, SkillUpdateDto request)
        {
            var skill = await GetByIdOrThrowAsync(id);
            
            if (request.JobClassificationId is not null)
            {
                var jobClassification = await _jobClassificationRepository.GetByIdAsync((int)request.JobClassificationId);
                
                if (jobClassification is null)
                    throw new NotFoundException($"Job classification with id '{request.JobClassificationId}' does not exist.");
            }

            if (request.JobSubClassificationId is not null)
            {
                var jobSubClassification = await _jobSubClassificationRepository.GetByIdAsync((int)request.JobSubClassificationId);

                if (jobSubClassification is null)
                    throw new NotFoundException($"Job sub classification with id '{request.JobSubClassificationId}' does not exist.");
            }

            skill.JobClassificationId = request.JobClassificationId;
            skill.JobSubClassificationId = request.JobSubClassificationId;
            skill.Name = request.Name;
;
            await _skillRepository.SaveChangesAsync();
            return ToDto(skill);
        }

        public async Task DeleteAsync(int id)
        {
            var skill = await GetByIdOrThrowAsync(id);
            
            _skillRepository.Delete(skill);
            await _skillRepository.SaveChangesAsync();
        }
    }
}