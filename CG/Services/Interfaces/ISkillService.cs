using CG.DTO.Skill;

namespace CG.Services.Interfaces
{
    public interface ISkillService
    {
        Task<SkillDto> GetByIdAsync(int id);

        Task<IEnumerable<SkillDto>> GetAllAsync(string? search = null);

        Task<SkillDto> CreateAsync(SkillCreateDto request);

        Task<SkillDto> UpdateAsync(
            int id,
            SkillUpdateDto request
        );

        Task DeleteAsync(int id);
    }
}