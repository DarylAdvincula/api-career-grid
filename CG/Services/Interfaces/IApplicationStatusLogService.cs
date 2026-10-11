using CG.DTO.ApplicationStatusLog;

namespace CG.Services.Interfaces
{
    public interface IApplicationStatusLogService
    {
        Task<ApplicationStatusLogDto> GetByIdAsync(int id);

        Task<ApplicationStatusLogDto> GetByApplicationIdAsync(int applicationId);

        Task<IEnumerable<ApplicationStatusLogDto>> GetAllByApplicantProfileIdAsync(int applicantProfileId);

        Task<IEnumerable<ApplicationStatusLogDto>> GetAllAsync();

        Task<ApplicationStatusLogDto> CreateAsync(ApplicationStatusLogCreateDto request);

        Task<ApplicationStatusLogDto> UpdateAsync(int id, ApplicationStatusLogUpdateDto request);

        Task DeleteAsync(int id);
    }
}