using CG.DTO.UserAccount;

namespace CG.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserAccountDto> MeAsync();
        
        Task<UserAccountDto> RegisterAsApplicantAsync(UserAccountCreateDto request);

        Task LoginAsync(UserAccountLoginDto request);
        
        Task LogoutAsync();
    }
}