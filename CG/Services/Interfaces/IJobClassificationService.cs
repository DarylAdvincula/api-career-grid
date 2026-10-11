using CG.DTO.JobClassification;

namespace CG.Services.Interfaces
{
    public interface IJobClassificationService
    {
        Task<JobClassificationDto> GetByIdAsync(int id);

        Task<IEnumerable<JobClassificationDto>> GetAllAsync();

        Task<JobClassificationDto> CreateAsync(JobClassificationCreateDto request);

        Task<JobClassificationDto> UpdateAsync(
            int id,
            JobClassificationUpdateDto request
        );

        Task DeleteAsync(int id);
    }
}