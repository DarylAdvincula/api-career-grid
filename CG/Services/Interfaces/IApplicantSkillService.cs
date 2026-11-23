using CG.DTO.ApplicantSkill;

namespace CG.Services.Interfaces
{
    public interface IApplicantSkillService
    {
        Task<ApplicantSkillDto> GetById(int id);
        
        Task<List<ApplicantSkillDto>> GetAllByApplicantProfileIdAsync(int applicantProfileId);
        
        Task<List<ApplicantSkillDto>> GetAllAsync();
        
        Task<ApplicantSkillDto> CreateAsync(ApplicantSkillCreateDto request);
        
        Task DeleteAsync(int id);
    }
}