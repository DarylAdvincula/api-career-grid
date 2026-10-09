using CG.DTO.JobPosting;

namespace CG.Services.Interfaces
{
    public interface IJobPostingService
    {
        Task<JobPostingDto> GetByIdAsync(int id);

        Task<IEnumerable<JobPostingDto>> GetAllByCompanyIdAsync(int companyId);

        Task<IEnumerable<JobPostingDto>> GetAllAsync(string? search);

        Task<JobPostingDto> CreateAsDraftAsync(JobPostingCreateDto request);

        Task<JobPostingDto> CreateAsPendingAsync(JobPostingCreateDto request);

        Task<JobPostingDto> UpdateAsync(
            int id,
            JobPostingUpdateDto request
        );

        Task<JobPostingDto> ApproveAsync(
            int id,
            int adminAccountUserId
        );

        Task<JobPostingDto> RejectAsync(int id);

        Task DeleteAsync(int id);
    }
}