using CG.DTO.EmployerProfile;
using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class EmployerProfileService : IEmployerProfileService
    {
        private readonly IEmployerProfileRepository _employerProfileRepository;

        public EmployerProfileService(IEmployerProfileRepository employerProfileRepository)
        {
            _employerProfileRepository = employerProfileRepository;
        }

        private async Task<EmployerProfile> GetByIdOrThrowAsync(int id)
        {
            var profile = await _employerProfileRepository.GetByIdAsync(id);

            if (profile is null)
                throw new NotFoundException($"Employer profile with id '{id}' does not exist.");
            
            return profile;
        }

        private static EmployerProfileDto ToDto(EmployerProfile employerProfile)
        {
            return new EmployerProfileDto
            {
                Id = employerProfile.Id,
                UserAccountId = employerProfile.UserAccountId,
                CompanyId = employerProfile.CompanyId,
                ApprovedByMemberId = employerProfile.ApprovedByMemberId,
                CompanyRole = employerProfile.CompanyRole,
                MembershipStatus = employerProfile.MembershipStatus,
                JoinedAt = employerProfile.JoinedAt,
            };
        }

        public async Task<EmployerProfileDto> GetByIdAsync(int id)
        {
            var profile = await GetByIdOrThrowAsync(id);
            return ToDto(profile);
        }

        public async Task<EmployerProfileDto> GetByUserAccountIdAsync(int userAccountId)
        {
            var profile = await _employerProfileRepository.GetByUserAccountIdAsync(userAccountId);

            if (profile is null)
                throw new NotFoundException($"Employer profile with user account id '{userAccountId}' does not exist.");
            
            return ToDto(profile);
        }

        public async Task<EmployerProfileDto> CreateAsync(EmployerProfileCreateDto request)
        {
            var existingProfile = await _employerProfileRepository.GetByUserAccountIdAsync(request.UserAccountId);

            if (existingProfile is not null)
                throw new ConflictException("You already have a profile.");
            
            var newProfile = new EmployerProfile
            {
                UserAccountId = request.UserAccountId,
                CompanyId = request.CompanyId,
                CompanyRole = request.CompanyRole,
                MembershipStatus = MembershipStatus.Pending,
            };

            await _employerProfileRepository.AddAsync(newProfile);
            await _employerProfileRepository.SaveChangesAsync();
            return ToDto(newProfile);
        }

        public async Task<EmployerProfileDto> ChangeRoleAsync(
            int id,
            EmployerProfileChangeRoleDto request
        )
        {
            var profile = await GetByIdOrThrowAsync(id);
            profile.CompanyRole = request.CompanyRole;

            _employerProfileRepository.Update(profile);
            await _employerProfileRepository.SaveChangesAsync();
            return ToDto(profile);
        }

        public async Task<EmployerProfileDto> ApproveAsync(int id)
        {
            var profile = await GetByIdOrThrowAsync(id);
            
            if (profile.MembershipStatus == MembershipStatus.Approved)
                throw new ConflictException($"Employer profile with id '{id}' is already approved.");
            
            profile.MembershipStatus = MembershipStatus.Approved;

            _employerProfileRepository.Update(profile);
            await _employerProfileRepository.SaveChangesAsync();
            return ToDto(profile);
        }

        public async Task<EmployerProfileDto> RejectAsync(int id)
        {
            var profile = await GetByIdOrThrowAsync(id);

            if (profile.MembershipStatus == MembershipStatus.Rejected)
                throw new ConflictException($"Employer profile with id '{id}' is already rejected.");
            
            profile.MembershipStatus = MembershipStatus.Rejected;

            _employerProfileRepository.Update(profile);
            await _employerProfileRepository.SaveChangesAsync();
            return ToDto(profile);
        }

        public async Task DeleteAsync(int id)
        {
            var profile = await GetByIdOrThrowAsync(id);

            _employerProfileRepository.Delete(profile);
            await _employerProfileRepository.SaveChangesAsync();
        }
    }
}