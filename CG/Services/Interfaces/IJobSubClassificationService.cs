using CG.DTO.JobClassification;
using CG.DTO.JobSubClassification;
using CG.Models;

namespace CG.Services.Interfaces
{
    public interface IJobSubClassificationService
    {
        Task<JobSubClassificationDto> GetByIdAsync(int id);

        Task<IEnumerable<JobSubClassificationDto>> GetAllAsync();

        Task<JobSubClassificationDto> CreateAsync(JobSubClassificationCreateDto request);
        
        Task<JobSubClassificationDto> UpdateAsync(
            int id,
            JobSubClassificationUpdateDto request
        );
        
        Task DeleteAsync(int id);
    }
}