using CG.DTO.WorkExperience;

namespace CG.Services.Interfaces
{
    public interface IWorkExperienceService
    {
        Task<WorkExperienceDto> GetByIdAsync(int id);

        Task<IEnumerable<WorkExperienceDto>> GetAllAsync();

        Task<WorkExperienceDto> CreateAsync(WorkExperienceCreateDto request);

        Task<WorkExperienceDto> UpdateAsync(
            int id,
            WorkExperienceUpdateDto request
        );

        Task DeleteAsync(int id);
    }
}