using CG.DTO.Application;

namespace CG.Services.Interfaces
{
    public interface IApplicationService
    {
        public Task<ApplicationDto> GetByIdAsync(int id);

        public Task<IEnumerable<ApplicationDto>> GetAllByApplicantProfileIdAsync(int applicantProfileId);

        public Task<IEnumerable<ApplicationDto>> GetAllAsync();

        public Task<ApplicationDto> CreateAsync(ApplicationCreateDto request);

        public Task<ApplicationDto> UpdateAsync(
            int id,
            ApplicationUpdateDto request
        );

        public Task DeleteAsync(int id);
    }
}