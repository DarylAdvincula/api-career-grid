using CG.DTO.JobPosting;
using CG.Models;

namespace CG.Services.Interfaces
{
    public interface IJobPostingService
    {
        Task<JobPostingDto> GetByIdAsync(int id);

        Task<IEnumerable<JobPostingDto>> GetAllByCompanyIdAsync(int companyId);

        Task<IEnumerable<JobPostingDto>> GetAllAsync(
            string? search = null, 
            ApprovalStatus? approvalStatus = null,
            bool? active = null
        );

        Task<JobPostingDto> CreateAsync(JobPostingCreateDto request);

        Task<JobPostingDto> UpdateAsync(
            int id,
            JobPostingUpdateDto request
        );

        Task<JobPostingDto> MarkForApproval(int id);

        Task<JobPostingDto> ApproveAsync(int id);

        Task<JobPostingDto> RejectAsync(int id);

        Task<JobPostingDto> ArchiveAsync(int id);

        Task<JobPostingDto> RestoreAsync(int id);
    }
}