using CG.DTO.EmployerProfile;

namespace CG.Services.Interfaces
{
    public interface IEmployerProfileService
    {
        Task<EmployerProfileDto> GetByIdAsync(int id);
        
        Task<EmployerProfileDto> GetByUserAccountIdAsync(int userAccountId);

        Task<EmployerProfileDto> CreateAsync(EmployerProfileCreateDto request);

        Task<EmployerProfileDto> ChangeRoleAsync(
            int id,
            EmployerProfileChangeRoleDto request
        );

        Task<EmployerProfileDto> ApproveAsync(int id);

        Task<EmployerProfileDto> RejectAsync(int id);

        Task DeleteAsync(int id);
    }
}