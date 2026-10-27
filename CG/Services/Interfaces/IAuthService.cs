using CG.DTO.AccountProfile;
using CG.DTO.Company;
using CG.DTO.UserAccount;

namespace CG.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AccountProfileDto> MeAsync();
        
        Task<UserAccountDto> RegisterAsApplicant(UserAccountCreateDto request);

        Task<UserAccountDto> RegisterAsOwner(CompanyCreateDto request);

        Task<UserAccountDto> RegisterAsStaff(CompanyJoinDto request);

        Task LoginAsync(UserAccountLoginDto request);
        
        Task LogoutAsync();

        Task VerifyEmailAsync(string verificationCode);
    }
}