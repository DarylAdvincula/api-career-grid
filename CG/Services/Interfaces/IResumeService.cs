using CG.DTO.Resume;

namespace CG.Services.Interfaces
{
    public interface IResumeService
    {
        Task<ResumeDto> GetByIdAsync(int id);
        
        Task<List<ResumeDto>> GetAllByApplicantProfileIdAsync(int id);
        
        Task<List<ResumeDto>> GetAllFromCurrentUserAsync();

        Task<List<ResumeDto>> GetAllAsync();

        Task<ResumeDto> CreateAsync(ResumeCreateDto request);

        Task DeleteAsync(int id);
    }
}