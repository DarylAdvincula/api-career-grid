using CG.DTO.JobPosting;
using CG.Exceptions;
using CG.Helpers.Implementations;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class JobPostingService : BaseEmployerService, IJobPostingService
    {
        private readonly IJobPostingRepository _jobPostingRepository;
        
        public JobPostingService(
            IHttpContextAccessor httpContextAccessor,
            IUserAccountRepository userAccountRepository,
            ICompanyRepository companyRepository,
            IJobPostingRepository jobPostingRepository,
            IEmployerProfileRepository employerProfileRepository
        ) : base(
            httpContextAccessor,
            userAccountRepository,
            companyRepository,
            employerProfileRepository
        )
        {
            _jobPostingRepository = jobPostingRepository;
        }
        
        private async Task<JobPosting> GetByIdOrThrowAsync(int id)
        {
            var jobPosting = await _jobPostingRepository.GetByIdAsync(id);

            if (jobPosting is null)
                throw new NotFoundException($"Job posting with id '{id}' does not exist.");
            
            return jobPosting;
        }

        private static JobPostingDto ToDto(JobPosting jobPosting)
        {
            return new JobPostingDto
            {
                Id = jobPosting.Id,
                CompanyId = jobPosting.CompanyId,
                CreatedByEmployerId = jobPosting.CreatedByEmployerId,
                JobClassificationId = jobPosting.JobClassificationId,
                JobSubClassificationId = jobPosting.JobSubClassificationId,
                ApprovedByAdminId = jobPosting.ApprovedByAdminId,
                Title = jobPosting.Title,
                RequirementsMarkdown = jobPosting.RequirementsMarkdown,
                JobType = jobPosting.JobType,
                WorkEnvironment = jobPosting.WorkEnvironment,
                Location = jobPosting.Location,
                MinSalary = jobPosting.MinSalary,
                MaxSalary = jobPosting.MaxSalary,
                ApprovalStatus = jobPosting.ApprovalStatus,
                ApprovedAt = jobPosting.ApprovedAt,
                IsActive = jobPosting.IsActive,
                PostedAt = jobPosting.PostedAt,
                UpdatedAt = jobPosting.UpdatedAt,
            };
        }
        
        public async Task<JobPostingDto> GetByIdAsync(int id)
        {
            var jobPosting = await GetByIdOrThrowAsync(id);
            return ToDto(jobPosting);
        }

        public async Task<IEnumerable<JobPostingDto>> GetAllByCompanyIdAsync(int companyId)
        {
            var jobPostings = await _jobPostingRepository.GetAllByCompanyIdAsync(companyId);
            return jobPostings.Select(ToDto);
        }

        public async Task<IEnumerable<JobPostingDto>> GetAllAsync(
            string? search = null, 
            ApprovalStatus? approvalStatus = null,
            bool? active = null
        )
        {           
            var currentUser = await GetCurrentUserAsync();
            var forbiddenException = new ForbiddenException($"You are not allowed to access this resource.");

            // applicant, owner, and hiring manager
            var hasNonPublishedOrInactiveFilter = (
                (
                    approvalStatus is not null && 
                    approvalStatus != ApprovalStatus.Published
                ) ||
                active == false
            );

            // handle restrictions for each role
            if (currentUser.Role == Role.Applicant)
            {
                if (hasNonPublishedOrInactiveFilter)
                    throw forbiddenException;

                active = true;
                approvalStatus = ApprovalStatus.Published;
            }
            else
            {
                var currentProfile = await GetCurrentProfileAsync();

                if (
                    currentProfile.CompanyRole == CompanyRole.Owner || 
                    currentProfile.CompanyRole == CompanyRole.HiringManager
                )
                {
                    if (hasNonPublishedOrInactiveFilter)
                        throw forbiddenException;

                    active = true;
                    approvalStatus = ApprovalStatus.Published;
                }
                else if (
                    currentProfile.CompanyRole == CompanyRole.Recruiter && 
                    active == false // no access to archived posts (admin only)
                )
                    throw forbiddenException;
            }

            var jobPostings = await _jobPostingRepository.GetAllAsync(search, approvalStatus, active);
            return jobPostings.Select(ToDto);
        }

        public async Task<JobPostingDto> CreateAsync(JobPostingCreateDto request)
        {
            var currentUser = await GetCurrentUserAsync();
            var currentCompany = await GetCurrentCompanyAsync();
            var newJobPosting = new JobPosting
            {
                CompanyId = currentCompany.Id,
                CreatedByEmployerId = currentUser.Id,
                JobClassificationId = request.JobClassificationId,
                JobSubClassificationId = request.JobSubClassificationId,
                Title = request.Title,
                RequirementsMarkdown = request.RequirementsMarkdown,
                JobType = string.IsNullOrEmpty(request.JobType)
                    ? null
                    : EnumHandler.Parse<JobType>(request.JobType),
                WorkEnvironment = string.IsNullOrEmpty(request.WorkEnvironment)
                    ? null
                    : EnumHandler.Parse<WorkEnvironment>(request.WorkEnvironment),
                Location = request.Location,
                MinSalary = request.MinSalary,
                MaxSalary = request.MaxSalary,
                ApprovalStatus = request.IsDraft
                    ? ApprovalStatus.Draft
                    : ApprovalStatus.Pending
            };

            await _jobPostingRepository.AddAsync(newJobPosting);
            await _jobPostingRepository.SaveChangesAsync();
            return ToDto(newJobPosting);
        }

        public async Task<JobPostingDto> UpdateAsync(int id, JobPostingUpdateDto request)
        {
            var jobPosting = await GetByIdOrThrowAsync(id);
            jobPosting.JobClassificationId = request.JobClassificationId;
            jobPosting.JobSubClassificationId = request.JobSubClassificationId;
            jobPosting.Title = request.Title;
            jobPosting.RequirementsMarkdown = request.RequirementsMarkdown;
            jobPosting.JobType = request.JobType;
            jobPosting.WorkEnvironment = request.WorkEnvironment;
            jobPosting.Location = request.Location;
            jobPosting.MinSalary = request.MinSalary;
            jobPosting.MaxSalary = request.MaxSalary;

            _jobPostingRepository.Update(jobPosting);
            await _jobPostingRepository.SaveChangesAsync();
            return ToDto(jobPosting);
        }

        public async Task<JobPostingDto> MarkForApproval(int id)
        {
            var jobPosting = await GetByIdOrThrowAsync(id);
            var currentProfile = await GetCurrentProfileAsync();

            // ensure that the admin can only approve their company's job posting
            if (jobPosting.CompanyId != currentProfile.CompanyId)
                throw new ForbiddenException("You are not allowed to perform this operation.");
            
            if (jobPosting.ApprovalStatus != ApprovalStatus.Draft)
                throw new BadRequestException("You can only mark drafted job postings for approval.");
            
            if (!jobPosting.IsActive)
                throw new BadRequestException("You can only approve active job postings.");
            
            _jobPostingRepository.Update(jobPosting);
            await _jobPostingRepository.SaveChangesAsync();
            return ToDto(jobPosting);
        }

        public async Task<JobPostingDto> ApproveAsync(int id)
        {
            var jobPosting = await GetByIdOrThrowAsync(id);
            var currentProfile = await GetCurrentProfileAsync();

            // ensure that the admin can only approve their company's job posting
            if (jobPosting.CompanyId != currentProfile.CompanyId)
                throw new ForbiddenException("You are not allowed to perform this operation.");
            
            if (jobPosting.ApprovalStatus != ApprovalStatus.Pending)
                throw new BadRequestException("You can only approve pending job postings.");

            if (!jobPosting.IsActive)
                throw new BadRequestException("You can only approve active job postings.");
            
            jobPosting.ApprovalStatus = ApprovalStatus.Published;
            jobPosting.ApprovedByAdminId = currentProfile.UserAccountId;
            jobPosting.ApprovedAt = DateTime.Now;

            _jobPostingRepository.Update(jobPosting);
            await _jobPostingRepository.SaveChangesAsync();
            return ToDto(jobPosting);
        }

        public async Task<JobPostingDto> RejectAsync(int id)
        {
            var jobPosting = await GetByIdOrThrowAsync(id);
            var currentProfile = await GetCurrentProfileAsync();

            // ensure that the admin can only reject their company's job posting
            if (jobPosting.CompanyId != currentProfile.CompanyId)
                throw new ForbiddenException("You are not allowed to perform this operation.");;
            
            if (jobPosting.ApprovalStatus != ApprovalStatus.Pending)
                throw new BadRequestException("You can only reject pending job postings.");

            if (!jobPosting.IsActive)
                throw new BadRequestException("You can only reject active job postings.");

            jobPosting.ApprovalStatus = ApprovalStatus.Draft;

            _jobPostingRepository.Update(jobPosting);
            await _jobPostingRepository.SaveChangesAsync();
            return ToDto(jobPosting);
        }

        public async Task<JobPostingDto> ArchiveAsync(int id)
        {
            var jobPosting = await GetByIdOrThrowAsync(id);
            var currentProfile = await GetCurrentProfileAsync();

            // ensure that the admin can only delete their company's job posting
            if (jobPosting.CompanyId != currentProfile.CompanyId)
                throw new ForbiddenException("You are not allowed to perform this operation.");

            if (jobPosting.ApprovalStatus != ApprovalStatus.Published)
                throw new BadRequestException("You can only archive published job postings.");

            if (!jobPosting.IsActive)
                throw new BadRequestException("You can only archive active job postings.");
            
            jobPosting.IsActive = false;
            jobPosting.ApprovalStatus = ApprovalStatus.Draft;

            _jobPostingRepository.Update(jobPosting);
            await _jobPostingRepository.SaveChangesAsync();
            return ToDto(jobPosting);
        }

        public async Task<JobPostingDto> RestoreAsync(int id)
        {
            var jobPosting = await GetByIdOrThrowAsync(id);
            var currentProfile = await GetCurrentProfileAsync();

            // ensure that the admin can only delete their company's job posting
            if (jobPosting.CompanyId != currentProfile.CompanyId)
                throw new ForbiddenException("You are not allowed to perform this operation.");

            if (jobPosting.IsActive)
                throw new BadRequestException("You can only restore archived job postings.");

            jobPosting.IsActive = true;
            jobPosting.ApprovalStatus = ApprovalStatus.Draft;

            _jobPostingRepository.Update(jobPosting);
            await _jobPostingRepository.SaveChangesAsync();
            return ToDto(jobPosting);
        }
    }
}