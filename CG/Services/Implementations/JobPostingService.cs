using CG.DTO.JobPosting;
using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class JobPostingService : IJobPostingService
    {
        private readonly IJobPostingRepository _jobPostingRepository;
        
        public JobPostingService(IJobPostingRepository jobPostingRepository)
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

        public async Task<IEnumerable<JobPostingDto>> GetAllAsync(string? search)
        {
            var jobPostings = await _jobPostingRepository.GetAllAsync(search);
            return jobPostings.Select(ToDto);
        }

        public async Task<JobPostingDto> CreateAsDraftAsync(JobPostingCreateDto request)
        {
            var newJobPosting = new JobPosting
            {
                CompanyId = request.CompanyId,
                CreatedByEmployerId = request.CreatedByEmployerId,
                JobClassificationId = request.JobClassificationId,
                JobSubClassificationId = request.JobSubClassificationId,
                Title = request.Title,
                RequirementsMarkdown = request.RequirementsMarkdown,
                JobType = request.JobType,
                WorkEnvironment = request.WorkEnvironment,
                Location = request.Location,
                MinSalary = request.MinSalary,
                MaxSalary = request.MaxSalary,
                ApprovalStatus = ApprovalStatus.Draft
            };

            await _jobPostingRepository.AddAsync(newJobPosting);
            await _jobPostingRepository.SaveChangesAsync();
            return ToDto(newJobPosting);
        }

        public async Task<JobPostingDto> CreateAsPendingAsync(JobPostingCreateDto request)
        {
            var newJobPosting = new JobPosting
            {
                CompanyId = request.CompanyId,
                CreatedByEmployerId = request.CreatedByEmployerId,
                JobClassificationId = request.JobClassificationId,
                JobSubClassificationId = request.JobSubClassificationId,
                Title = request.Title,
                RequirementsMarkdown = request.RequirementsMarkdown,
                JobType = request.JobType,
                WorkEnvironment = request.WorkEnvironment,
                Location = request.Location,
                MinSalary = request.MinSalary,
                MaxSalary = request.MaxSalary,
                ApprovalStatus = ApprovalStatus.Pending
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

        public async Task<JobPostingDto> ApproveAsync(int id, int adminAccountUserId)
        {
            var jobPosting = await GetByIdOrThrowAsync(id);
            jobPosting.ApprovalStatus = ApprovalStatus.Published;
            jobPosting.ApprovedByAdminId = adminAccountUserId;
            jobPosting.ApprovedAt = DateTime.Now;

            _jobPostingRepository.Update(jobPosting);
            await _jobPostingRepository.SaveChangesAsync();
            return ToDto(jobPosting);
        }

        public async Task<JobPostingDto> RejectAsync(int id)
        {
            var jobPosting = await GetByIdOrThrowAsync(id);
            jobPosting.ApprovalStatus = ApprovalStatus.Rejected;

            _jobPostingRepository.Update(jobPosting);
            await _jobPostingRepository.SaveChangesAsync();
            return ToDto(jobPosting);
        }

        public async Task DeleteAsync(int id)
        {
            var jobPosting = await GetByIdOrThrowAsync(id);

            _jobPostingRepository.Delete(jobPosting);
            await _jobPostingRepository.SaveChangesAsync();
        }
    }
}