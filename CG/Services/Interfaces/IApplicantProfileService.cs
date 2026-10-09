using CG.DTO.ApplicantProfile;

namespace CG.Services.Interfaces
{
    public interface IApplicantProfileService
    {
        Task<ApplicantProfileDto> GetByIdAsync(int id);

        Task<ApplicantProfileDto> GetByUserAccountIdAsync(int userAccountId);
        
        Task<ApplicantProfileDto> CreateAsync(ApplicantProfileCreateDto request);

        Task<ApplicantProfileDto> UpdateAsync(
            int id, 
            ApplicantProfileUpdateDto request
        );

        Task DeleteAsync(int id);
    }
}