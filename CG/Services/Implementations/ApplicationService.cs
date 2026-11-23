using CG.DTO.ApplicantProfile;
using CG.DTO.ApplicantSkill;
using CG.DTO.Application;
using CG.DTO.Resume;
using CG.DTO.Skill;
using CG.DTO.UserAccount;
using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CG.Services.Implementations
{
    public class ApplicationService : BaseService, IApplicationService
    {
        private readonly IBaseApplicantService _baseApplicantService;
        private readonly IBaseEmployerService _baseEmployerService;
        private readonly IApplicationRepository _applicationRepository;
        private readonly IJobPostingRepository _jobPostingRepository;
        private readonly IResumeRepository _resumeRepository;

        public ApplicationService(
            IHttpContextAccessor httpContextAccessor,
            IUserAccountRepository userAccountRepository,
            IBaseApplicantService baseApplicantService,
            IBaseEmployerService baseEmployerService,
            IApplicationRepository applicationRepository,
            IJobPostingRepository jobPostingRepository,
            IResumeRepository resumeRepository
        ) : base(
            httpContextAccessor,
            userAccountRepository
        )
        {
            _baseApplicantService = baseApplicantService;
            _baseEmployerService = baseEmployerService;
            _applicationRepository = applicationRepository;
            _jobPostingRepository = jobPostingRepository;
            _resumeRepository = resumeRepository;
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
                CoverLetter = application.CoverLetter,
                ApplicationStatus = application.ApplicationStatus,
                SubmittedAt = application.SubmittedAt,
                UpdatedAt = application.UpdatedAt,
                Resume = new ResumeDto
                {
                    Id = application.Resume.Id,
                    ApplicantProfileId = application.Resume.ApplicantProfileId,
                    OriginalFileName = application.Resume.OriginalFileName,
                    StoredFileName = application.Resume.StoredFileName,
                    FilePath = application.Resume.FilePath,
                    FileExtension = application.Resume.FileExtension,
                    ContentType = application.Resume.ContentType,
                    FileSizeInBytes = application.Resume.FileSizeInBytes,
                    IsPrimary = application.Resume.IsPrimary,
                    UploadedAt = application.Resume.UploadedAt
                },
                ApplicantProfile = new ApplicantProfileDto
                {
                    Id = application.ApplicantProfile.Id,
                    UserAccountId = application.ApplicantProfile.UserAccountId,
                    Headline = application.ApplicantProfile.Headline,
                    HomeLocation = application.ApplicantProfile.HomeLocation,
                    Bio = application.ApplicantProfile.Bio,
                    CreatedAt = application.ApplicantProfile.CreatedAt,
                    Skills = application.ApplicantProfile.ApplicantSkills.Select(x => new ApplicantSkillDto
                    {
                        Id = x.Id,
                        ApplicantProfileId = x.ApplicantProfileId,
                        SkillId = x.SkillId,
                        Skill = new SkillDto
                        {
                            Id = x.Skill.Id,
                            Name = x.Skill.Name,
                            CreatedAt = x.Skill.CreatedAt
                        }
                    }).ToList(),
                    UserAccount = new UserAccountDto
                    {
                        Id = application.ApplicantProfile.UserAccount.Id,
                        Email = application.ApplicantProfile.UserAccount.Email,
                        FirstName = application.ApplicantProfile.UserAccount.FirstName,
                        LastName = application.ApplicantProfile.UserAccount.LastName,
                        Role = application.ApplicantProfile.UserAccount.Role,
                        IsVerified = application.ApplicantProfile.UserAccount.IsVerified,
                        CreatedAt = application.ApplicantProfile.UserAccount.CreatedAt,
                        UpdatedAt = application.ApplicantProfile.UserAccount.UpdatedAt,
                    }
                }
            };
        }
        
        public async Task<ApplicationDto> GetByIdAsync(int id)
        {
            var application = await GetByIdOrThrowAsync(id);
            return ToDto(application);
        }

        public async Task<List<ApplicationDto>> GetAllForApplicantAsync(ApplicationStatus? applicationStatus = null)
        {   
            var currentProfile = await _baseApplicantService.GetRequiredCurrentProfileAsync();
            var applications = await _applicationRepository.GetAllByApplicantProfileIdAsync(currentProfile.Id, applicationStatus);
            return applications.Select(ToDto)
                .ToList();
        }

        public async Task<List<ApplicationDto>> GetAllByJobPostingIdAsync(int jobPostingId)
        {
            var currentCompany = await _baseEmployerService.GetCurrentCompanyAsync();
            var jobPosting = await _jobPostingRepository.GetByIdAsync(jobPostingId);

            if (jobPosting is null)
                throw new NotFoundException($"Job posting with id '{jobPostingId}' does not exist.");
            
            if (jobPosting.CompanyId != currentCompany.Id)
                throw new ForbiddenException("You are not allowed to access this resource.");
            
            var applications = await _applicationRepository.GetAllByCompanyAndJobPostingIdAsync(currentCompany.Id, jobPostingId);
            return applications.Select(ToDto)
                .ToList();
        }

        public async Task<ApplicationDto> CreateAsync(ApplicationCreateDto request)
        {
            try
            {
                var currentProfile = await _baseApplicantService.GetRequiredCurrentProfileAsync();
                var resume = await _resumeRepository.GetByIdAsync(request.ResumeId);

                if (resume is null)
                    throw new NotFoundException($"Resume with id '{request.ResumeId}' does not exist.");

                if (resume.ApplicantProfileId != currentProfile.Id)
                    throw new ForbiddenException("You are not allowed to access this resource.");
                
                var newApplication = new Application
                {
                    JobPostingId = request.JobPostingId,
                    ApplicantProfileId = currentProfile.Id,
                    ResumeId = request.ResumeId,
                    ApplicationStatus = request.IsDraft
                        ? ApplicationStatus.Draft
                        : ApplicationStatus.Applied,
                    CoverLetter = request.CoverLetter
                };

                await _applicationRepository.AddAsync(newApplication);
                await _applicationRepository.SaveChangesAsync();
                return ToDto(newApplication);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 } sql)
            {
                throw new ConflictException("You've already applied for this job.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ApplicationDto> ContinueApplicationAsync(int id)
        {
            var currentProfile = await _baseApplicantService.GetRequiredCurrentProfileAsync();
            var application = await GetByIdOrThrowAsync(id);
            
            if (application.ApplicantProfileId != currentProfile.Id)
                throw new ForbiddenException("You are not allowed to perform this action.");
                
            if (application.ApplicationStatus != ApplicationStatus.Draft)
                throw new ForbiddenException("You can only continue to apply drafted applications.");

            application.ApplicationStatus = ApplicationStatus.Applied;

            _applicationRepository.Update(application);
            await _applicationRepository.SaveChangesAsync();
            return ToDto(application);
        }

        public async Task<ApplicationDto> UpdateStatusAsync(int id, ApplicationStatus? applicationStatus)
        {
            var currentCompany = await _baseEmployerService.GetCurrentCompanyAsync();
            var application = await GetByIdOrThrowAsync(id);
            var jobPosting = await _jobPostingRepository.GetByIdAsync(application.JobPostingId);

            if (applicationStatus == ApplicationStatus.Draft)
                throw new ForbiddenException("You are not allowed to perform this action.");
            
            if (applicationStatus is null)
                throw new BadRequestException("Application status is invalid.");

            if (jobPosting is null)
                throw new NotFoundException($"Job posting with id '{application.JobPostingId}' does not exist.");
            
            if (jobPosting.CompanyId != currentCompany.Id)
                throw new ForbiddenException("You are not allowed to perform this action.");
            
            application.ApplicationStatus = (ApplicationStatus)applicationStatus;

            _applicationRepository.Update(application);
            await _applicationRepository.SaveChangesAsync();
            return ToDto(application);
        }

        public async Task<ApplicationDto> UpdateAsync(int id, ApplicationUpdateDto request)
        {
            var currentProfile = await _baseApplicantService.GetRequiredCurrentProfileAsync();
            var resume = await _resumeRepository.GetByIdAsync(request.ResumeId);
            
            if (resume is null)
                throw new NotFoundException($"Resume with id '{request.ResumeId}' does not exist.");

            if (resume.ApplicantProfileId != currentProfile.Id)
                throw new ForbiddenException("You are not allowed to access this resource.");
            
            var application = await GetByIdOrThrowAsync(id);

            if (application.ApplicationStatus != ApplicationStatus.Draft)
                throw new ForbiddenException("You can only edit draft applications.");
            
            application.ResumeId = request.ResumeId;
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