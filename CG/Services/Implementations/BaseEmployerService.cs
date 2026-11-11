using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class BaseEmployerService : BaseService, IBaseEmployerService
    {
        protected ICompanyRepository _companyRepository;
        protected IEmployerProfileRepository _employerProfileRepository;
        
        public BaseEmployerService(
            IHttpContextAccessor httpContextAccessor,
            IUserAccountRepository userAccountRepository,
            ICompanyRepository companyRepository,
            IEmployerProfileRepository employerProfileRepository
        ) : base(
            httpContextAccessor,
            userAccountRepository
        )
        {
            _companyRepository = companyRepository;
            _employerProfileRepository = employerProfileRepository;
        }

        public async Task<EmployerProfile> GetCurrentProfileAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            var currentProfile = await _employerProfileRepository.GetByUserAccountIdAsync(currentUser.Id);

            if (currentProfile is null)
                throw new NotFoundException($"Employer profile with user account id '{currentUser.Id}' does not exist.");
            
            return currentProfile;
        }

        public async Task<Company> GetCurrentCompanyAsync()
        {
            var currentProfile = await GetCurrentProfileAsync();
            var currentCompany = await _companyRepository.GetByIdAsync(currentProfile.CompanyId);

            if (currentCompany is null)
                throw new NotFoundException($"Company with id '{currentProfile.CompanyId}' does not exist.");
            
            return currentCompany;
        }

        public async Task BlockPendingOrRejectedStaff()
        {
            var currentProfile = await GetCurrentProfileAsync();

            switch (currentProfile.MembershipStatus)
            {
                case MembershipStatus.Pending:
                    throw new ForbiddenException("Your join request is still under review. Please wait for the owner's approval before performing any action.");

                case MembershipStatus.Rejected:
                    throw new ForbiddenException("Your join request was rejected by the owner. You are not allowed to perform any action for this company.");
            }
        }
    }
}