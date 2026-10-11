using CG.DTO.ApplicantSkill;

namespace CG.Services.Interfaces
{
    public interface IApplicantSkillService
    {
        Task<ApplicantSkillDto> GetById(int id);
        
        Task<IEnumerable<ApplicantSkillDto>> GetAllByApplicantProfileIdAsync(int applicantProfileId);
        
        Task<IEnumerable<ApplicantSkillDto>> GetAllAsync();
        
        Task<ApplicantSkillDto> CreateAsync(ApplicantSkillCreateDto request);
        
        Task<ApplicantSkillDto> UpdateAsync(
            int id,
            ApplicantSkillUpdateDto request
        );
        
        Task DeleteAsync(int id);
    }
}