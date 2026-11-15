using CG.DTO.Application;
using CG.Models;

namespace CG.Services.Interfaces
{
    public interface IApplicationService
    {
        public Task<ApplicationDto> GetByIdAsync(int id);

        public Task<List<ApplicationDto>> GetAllForApplicantAsync(ApplicationStatus? applicationStatus = null);

        public Task<List<ApplicationDto>> GetAllByJobPostingIdAsync(int jobPostingId);

        public Task<ApplicationDto> CreateAsync(ApplicationCreateDto request);

        public Task<ApplicationDto> ContinueApplicationAsync(int id);

        public Task<ApplicationDto> UpdateStatusAsync(
            int id, 
            ApplicationStatus? applicationStatus
        );
        
        public Task<ApplicationDto> UpdateAsync(
            int id,
            ApplicationUpdateDto request
        );

        public Task DeleteAsync(int id);
    }
}