using CG.DTO.ApplicantSkill;
using CG.Models;

namespace CG.Services.Interfaces
{
    public interface IApplicantSkillService
    {
        Task<ApplicantSkillDto> GetById(int id);
        
        Task<List<ApplicantSkillDto>> GetAllByApplicantProfileIdAsync(int applicantProfileId);
        
        Task<List<ApplicantSkillDto>> GetAllAsync();
        
        Task<ApplicantSkillDto> CreateAsync(ApplicantSkillCreateDto request);
        
        Task<ApplicantSkillDto> UpdateAsync(
            int id,
            ApplicantSkillUpdateDto request
        );
        
        Task DeleteAsync(int id);
    }
}