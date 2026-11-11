using CG.DAL;
using CG.DTO.AccountProfile;
using CG.DTO.Company;
using CG.DTO.EmployerProfile;
using CG.DTO.UserAccount;
using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CG.Services.Implementations
{
    public class StaffService : BaseEmployerService, IStaffService
    {
        private readonly AppDbContext _context;
        
        public StaffService(
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IUserAccountRepository userAccountRepository,
            ICompanyRepository companyRepository,
            IEmployerProfileRepository employerProfileRepository
        ) : base(
            httpContextAccessor,
            userAccountRepository,
            companyRepository,
            employerProfileRepository
        )
        {
            _context = context;
        }

        private async Task<EmployerProfile> GetByUserAccountIdAndCompanyIdOrThrowAsync(int userAccountId, int companyId)
        {
            var profile = await _employerProfileRepository.GetByUserAccountIdAndCompanyIdAsync(userAccountId, companyId);

            if (profile is null)
                throw new NotFoundException($"Applicant profile with user account id '{userAccountId}' and company id '{companyId}' does not exist.");
            
            return profile;
        }

        private static EmployerDto ToDto(EmployerProfile employerProfile, Company company)
        {
            var userAccount = new UserAccountDto
            {
                Id = employerProfile.UserAccount.Id,
                Email = employerProfile.UserAccount.Email,
                FirstName = employerProfile.UserAccount.FirstName,
                LastName = employerProfile.UserAccount.LastName,
                Role = employerProfile.UserAccount.Role,
                IsVerified = employerProfile.UserAccount.IsVerified,
                CreatedAt = employerProfile.UserAccount.CreatedAt,
                UpdatedAt = employerProfile.UserAccount.UpdatedAt
            };

            return new EmployerDto
            {
                Account = userAccount,
                Profile = new EmployerProfileDto
                {
                    Id = employerProfile.Id,
                    UserAccountId = employerProfile.UserAccountId,
                    CompanyId = employerProfile.CompanyId,
                    ApprovedByMemberId = employerProfile.ApprovedByMemberId,
                    CompanyRole = employerProfile.CompanyRole,
                    MembershipStatus = employerProfile.MembershipStatus,
                    JoinedAt = employerProfile.JoinedAt,
                },
                Company = new CompanyDto
                {
                    Id = company.Id,
                    ApprovedByAdminId = company.ApprovedByAdminId,
                    Name = company.Name,
                    Description = company.Description,
                    WebsiteUrl = company.WebsiteUrl,
                    LogoUrl = company.LogoUrl,
                    VerificationStatus = company.VerificationStatus,
                    ApprovedAt = company.ApprovedAt,
                    CreatedAt = company.CreatedAt,
                }
            };
        }

        public async Task<List<EmployerDto>> GetAll(
            string? search = null,
            CompanyRole? role = null
        )
        {
            var currentCompany = await GetCurrentCompanyAsync();
            var staffProfileQuery = _context.EmployerProfiles.AsQueryable()
                .Where(x => x.CompanyId == currentCompany.Id);
            
            if (search is not null)
                staffProfileQuery = staffProfileQuery.Where(x => (
                    x.UserAccount.FirstName.Contains(search) ||
                    x.UserAccount.LastName.Contains(search) || 
                    $"{x.UserAccount.FirstName} {x.UserAccount.LastName}".Contains(search)
                ));
            
            if (search is not null)
                staffProfileQuery = staffProfileQuery.Where(x => x.CompanyRole == role);

            var staffProfiles = await staffProfileQuery
                .Include(x => x.UserAccount)
                .ToListAsync();
            
            return staffProfiles.Select(x => ToDto(x, currentCompany))
                .ToList();
        }
        public async Task<EmployerDto> Approve(int id)
        {
            var currentCompany = await GetCurrentCompanyAsync();
            var targetProfile = await GetByUserAccountIdAndCompanyIdOrThrowAsync(id, currentCompany.Id);
            var currentUser = await GetCurrentUserAsync();

            targetProfile.MembershipStatus = MembershipStatus.Approved;
            targetProfile.ApprovedByMemberId = currentUser.Id;

            _employerProfileRepository.Update(targetProfile);
            await _employerProfileRepository.SaveChangesAsync();
            return ToDto(targetProfile, currentCompany);
        }
        public async Task<EmployerDto> Reject(int id)
        {
            var currentCompany = await GetCurrentCompanyAsync();
            var targetProfile = await GetByUserAccountIdAndCompanyIdOrThrowAsync(id, currentCompany.Id);
            var currentUser = await GetCurrentUserAsync();

            targetProfile.MembershipStatus = MembershipStatus.Rejected;
            targetProfile.ApprovedByMemberId = currentUser.Id;

            _employerProfileRepository.Update(targetProfile);
            await _employerProfileRepository.SaveChangesAsync();
            return ToDto(targetProfile, currentCompany);
        }
    }
}