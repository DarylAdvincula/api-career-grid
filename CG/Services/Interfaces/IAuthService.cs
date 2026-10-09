using CG.DTO.Auth;
using CG.DTO.UserAccount;

namespace CG.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserAccountDto> RegisterAsApplicantAsync(UserAccountCreateDto request);

        Task LoginAsync(LoginDto request);
        
        Task LogoutAsync();
    }
}