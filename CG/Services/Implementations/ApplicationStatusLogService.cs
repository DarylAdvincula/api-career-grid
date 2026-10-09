using CG.DTO.ApplicationStatusLog;
using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class ApplicationStatusLogService : IApplicationStatusLogService
    {
        private readonly IApplicationStatusLogRepository _applicationStatusLogRepository;

        public ApplicationStatusLogService(IApplicationStatusLogRepository applicationStatusLogRepository)
        {
            _applicationStatusLogRepository = applicationStatusLogRepository;
        }

        private async Task<ApplicationStatusLog> GetByIdOrThrowAsync(int id)
        {
            var applicationStatusLog = await _applicationStatusLogRepository.GetByIdAsync(id);

            if (applicationStatusLog is null)
                throw new NotFoundException($"Application status log with id '{id}' does not exist.");
            
            return applicationStatusLog;
        }

        private static ApplicationStatusLogDto ToDto(ApplicationStatusLog applicationStatusLog)
        {
            return new ApplicationStatusLogDto
            {
                Id = applicationStatusLog.Id,
                ApplicationId = applicationStatusLog.ApplicationId,
                ChangedByUserId = applicationStatusLog.ChangedByUserId,
                OldApplicationStatus = applicationStatusLog.OldApplicationStatus,
                NewApplicationStatus = applicationStatusLog.NewApplicationStatus,
                Notes = applicationStatusLog.Notes,
                ChangedAt = applicationStatusLog.ChangedAt,
            };
        }

        public async Task<ApplicationStatusLogDto> GetByIdAsync(int id)
        {
            var applicationStatusLog = await GetByIdOrThrowAsync(id);
            return ToDto(applicationStatusLog);
        }

        public async Task<ApplicationStatusLogDto> GetByApplicationIdAsync(int applicationId)
        {
            var applicationStatusLog = await _applicationStatusLogRepository.GetByApplicationIdAsync(applicationId);

            if (applicationStatusLog is null)
                throw new NotFoundException($"Application status log with application id '{applicationId}' does not exist.");

            return ToDto(applicationStatusLog);
        }

        public async Task<IEnumerable<ApplicationStatusLogDto>> GetAllByApplicantProfileIdAsync(int applicantProfileId)
        {
            var applicationStatusLogs = await _applicationStatusLogRepository.GetAllByApplicantProfileIdAsync(applicantProfileId);
            return applicationStatusLogs.Select(ToDto);
        }

        public async Task<IEnumerable<ApplicationStatusLogDto>> GetAllAsync()
        {
            var applicationStatusLogs = await _applicationStatusLogRepository.GetAllAsync();
            return applicationStatusLogs.Select(ToDto);
        }

        public async Task<ApplicationStatusLogDto> CreateAsync(ApplicationStatusLogCreateDto request)
        {
            var existingApplicationStatusLog = await _applicationStatusLogRepository.GetByApplicationIdAsync(request.ApplicationId);

            if (existingApplicationStatusLog is not null)
                throw new ConflictException($"Application status log with application id '{request.ApplicationId}' already exist.");
            
            var newApplicationStatusLog = new ApplicationStatusLog
            {
                ApplicationId = request.ApplicationId,
                ChangedByUserId = request.ChangedByUserId,
                NewApplicationStatus = request.NewApplicationStatus,
                Notes = request.Notes
            };

            await _applicationStatusLogRepository.AddAsync(newApplicationStatusLog);
            await _applicationStatusLogRepository.SaveChangesAsync();
            return ToDto(newApplicationStatusLog);
        }

        public async Task<ApplicationStatusLogDto> UpdateAsync(int id, ApplicationStatusLogUpdateDto request)
        {
            var existingApplicationStatusLog = await GetByIdOrThrowAsync(id);
            existingApplicationStatusLog.NewApplicationStatus = request.NewApplicationStatus;
            existingApplicationStatusLog.Notes = request.Notes;

            await _applicationStatusLogRepository.SaveChangesAsync();
            return ToDto(existingApplicationStatusLog);
        }

        public async Task DeleteAsync(int id)
        {
            var applicationStatusLog = await GetByIdOrThrowAsync(id);
            
            _applicationStatusLogRepository.Delete(applicationStatusLog);
            await _applicationStatusLogRepository.SaveChangesAsync();
        }
    }
}