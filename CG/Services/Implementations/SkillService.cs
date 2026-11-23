using CG.DTO.Skill;
using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;

        public SkillService(ISkillRepository skillRepository)
        {
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
                Name = skill.Name,
                CreatedAt = skill.CreatedAt
            };
        }

        public async Task<SkillDto> GetByIdAsync(int id)
        {
            var skill = await GetByIdOrThrowAsync(id);
            return ToDto(skill);
        }

        public async Task<IEnumerable<SkillDto>> GetAllAsync(string? search = null)
        {
            var skills = await _skillRepository.GetAllAsync(search);
            return skills.Select(ToDto);
        }

        public async Task<SkillDto> CreateAsync(SkillCreateDto request)
        {
            var newSkill = new Skill
            {
                Name = request.Name
            };

            await _skillRepository.AddAsync(newSkill);
            await _skillRepository.SaveChangesAsync();
            return ToDto(newSkill);
        }

        public async Task<SkillDto> UpdateAsync(int id, SkillUpdateDto request)
        {
            var skill = await GetByIdOrThrowAsync(id);
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