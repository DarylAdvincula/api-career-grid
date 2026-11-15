using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class BaseApplicantService : BaseService, IBaseApplicantService
    {
        private readonly IApplicantProfileRepository _applicantProfileRepository;

        public BaseApplicantService(
            IHttpContextAccessor httpContextAccessor,
            IUserAccountRepository userAccountRepository,
            IApplicantProfileRepository applicantProfileRepository
        ) : base(
            httpContextAccessor,
            userAccountRepository
        )
        {
            _applicantProfileRepository = applicantProfileRepository;
        }

        public async Task<ApplicantProfile?> GetOptionalCurrentProfileAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            var currentProfile = await _applicantProfileRepository.GetByUserAccountIdAsync(currentUser.Id);
            return currentProfile;
        }

        public async Task<ApplicantProfile> GetRequiredCurrentProfileAsync()
        {
            var currentUser = await GetCurrentUserAsync();
            var currentProfile = await _applicantProfileRepository.GetByUserAccountIdAsync(currentUser.Id);

            if (currentProfile is null)
                throw new NotFoundException("You haven't created your profile yet. Please create it first.");
            
            return currentProfile;
        }
    }
}