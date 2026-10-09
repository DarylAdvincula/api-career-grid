using CG.DTO.Application;
using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;

        public ApplicationService(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        private async Task<Application> GetByIdOrThrowAsync(int id)
        {
            var application = await _applicationRepository.GetByIdAsync(id);

            if (application is null)
                throw new NotFoundException($"Application with id '{id}' does not exist.");

            return application;
        }

        private static ApplicationDto ToDto(Application application)
        {
            return new ApplicationDto
            {
                Id = application.Id,
                JobPostingId = application.JobPostingId,
                ApplicantProfileId = application.ApplicantProfileId,
                ResumeId = application.ResumeId,
                IsSubmitted = application.IsSubmitted,
                DraftStep = application.DraftStep,
                CoverLetter = application.CoverLetter,
                ApplicationStatus = application.ApplicationStatus,
                SubmittedAt = application.SubmittedAt,
                UpdatedAt = application.UpdatedAt,
            };
        }
        
        public async Task<ApplicationDto> GetByIdAsync(int id)
        {
            var application = await GetByIdOrThrowAsync(id);
            return ToDto(application);
        }

        public async Task<IEnumerable<ApplicationDto>> GetAllByApplicantProfileIdAsync(int applicantProfileId)
        {
            var applications = await _applicationRepository.GetAllByApplicantProfileIdAsync(applicantProfileId);
            return applications.Select(ToDto);
        }

        public async Task<IEnumerable<ApplicationDto>> GetAllAsync()
        {
            var applications = await _applicationRepository.GetAllAsync();
            return applications.Select(ToDto);
        }

        public async Task<ApplicationDto> CreateAsync(ApplicationCreateDto request)
        {
            var newApplication = new Application
            {
                JobPostingId = request.JobPostingId,
                ApplicantProfileId = request.ApplicantProfileId,
                ResumeId = request.ResumeId,
                IsSubmitted = request.IsSubmitted,
                DraftStep = request.DraftStep,
                ApplicationStatus = request.DraftStep ? ApplicationStatus.Draft : ApplicationStatus.Applied,
                CoverLetter = request.CoverLetter
            };

            await _applicationRepository.AddAsync(newApplication);
            await _applicationRepository.SaveChangesAsync();
            return ToDto(newApplication);
        }

        public async Task<ApplicationDto> UpdateAsync(int id, ApplicationUpdateDto request)
        {
            var application = await GetByIdOrThrowAsync(id);
            application.ResumeId = request.ResumeId;
            application.DraftStep = request.DraftStep;
            application.CoverLetter = request.CoverLetter;

            await _applicationRepository.SaveChangesAsync();
            return ToDto(application);
        }

        public async Task DeleteAsync(int id)
        {
            var application = await GetByIdOrThrowAsync(id);
            
            _applicationRepository.Delete(application);
            await _applicationRepository.SaveChangesAsync();
        }
    }
}