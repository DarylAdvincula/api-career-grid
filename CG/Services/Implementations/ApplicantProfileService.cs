using CG.DTO.ApplicantProfile;
using CG.Exceptions;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class ApplicantProfileService : IApplicantProfileService
    {
        private readonly IApplicantProfileRepository _applicantProfileRepository;
        
        public ApplicantProfileService(IApplicantProfileRepository applicantProfileRepository)
        {
            _applicantProfileRepository = applicantProfileRepository;
        }

        private async Task<ApplicantProfile> GetByIdOrThrowAsync(int id)
        {
            var profile = await _applicantProfileRepository.GetByIdAsync(id);

            if (profile is null)
                throw new NotFoundException($"Applicant profile with id '{id}' does not exist.");
            
            return profile;
        }

        private static ApplicantProfileDto ToDto(ApplicantProfile applicantProfile)
        {
            return new ApplicantProfileDto
            {
                Id = applicantProfile.Id,
                UserAccountId = applicantProfile.UserAccountId,
                Headline = applicantProfile.Headline,
                HomeLocation = applicantProfile.HomeLocation,
                Bio = applicantProfile.Bio,
                CreatedAt = applicantProfile.CreatedAt,
            };
        }

        public async Task<ApplicantProfileDto> GetByIdAsync(int id)
        {
            var profile = await GetByIdOrThrowAsync(id);
            return ToDto(profile);
        }

        public async Task<ApplicantProfileDto> GetByUserAccountIdAsync(int userAccountId)
        {
            var profile = await _applicantProfileRepository.GetByUserAccountIdAsync(userAccountId);

            if (profile is null)
                throw new NotFoundException($"Applicant profile with user account id '{userAccountId}' does not exists.");

            return ToDto(profile);
        }

        public async Task<ApplicantProfileDto> CreateAsync(ApplicantProfileCreateDto request)
        {
            var existingProfile = await _applicantProfileRepository.GetByUserAccountIdAsync(request.UserAccountId);

            if (existingProfile is not null)
                throw new ConflictException("You already have a profile.");
            
            var newProfile = new ApplicantProfile
            {
                UserAccountId = request.UserAccountId,
                Headline = request.Headline,
                HomeLocation = request.HomeLocation,
                Bio = request.Bio
            };

            await _applicantProfileRepository.AddAsync(newProfile);
            await _applicantProfileRepository.SaveChangesAsync();
            return ToDto(newProfile);
        }

        public async Task<ApplicantProfileDto> UpdateAsync(int id, ApplicantProfileUpdateDto request)
        {
            var existingProfile = await _applicantProfileRepository.GetByIdAsync(id);

            if (existingProfile is null)
                throw new BadRequestException("You do not have a profile yet.");
            
            existingProfile.Headline = request.Headline;
            existingProfile.HomeLocation = request.HomeLocation;
            existingProfile.Bio = request.Bio;

            _applicantProfileRepository.Update(existingProfile);
            await _applicantProfileRepository.SaveChangesAsync();
            return ToDto(existingProfile);
        }

        public async Task DeleteAsync(int id)
        {
            var profile = await GetByIdOrThrowAsync(id);
            
            _applicantProfileRepository.Delete(profile);
            await _applicantProfileRepository.SaveChangesAsync();
        }
    }
}