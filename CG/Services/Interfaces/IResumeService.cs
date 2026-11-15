using CG.DTO.Resume;

namespace CG.Services.Interfaces
{
    public interface IResumeService
    {
        Task<ResumeDto> GetByIdAsync(int id);
        
        Task<List<ResumeDto>> GetAllByApplicantProfileIdAsync(int applicantProfileId);

        Task<List<ResumeDto>> GetAllAsync();

        Task<ResumeDto> CreateAsync(ResumeCreateDto request);

        Task<ResumeDto> UpdateAsync(
            int id,
            ResumeUpdateDto request
        );

        Task DeleteAsync(int id);
    }
}