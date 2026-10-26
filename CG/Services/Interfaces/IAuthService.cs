using CG.DTO.AccountProfile;
using CG.DTO.Company;
using CG.DTO.UserAccount;

namespace CG.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AccountProfileDto> MeAsync();
        
        Task<UserAccountDto> Register(UserAccountCreateDto request);

        Task<UserAccountDto> CreateCompany(CompanyCreateDto request);

        Task<UserAccountDto> JoinCompany(CompanyJoinDto request);

        Task LoginAsync(UserAccountLoginDto request);
        
        Task LogoutAsync();
    }
}