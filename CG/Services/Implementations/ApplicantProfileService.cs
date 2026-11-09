using CG.DAL;
using CG.DTO.ApplicantProfile;
using CG.DTO.ApplicantSkill;
using CG.Exceptions;
using CG.Helpers.Implementations;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class ApplicantProfileService : BaseService, IApplicantProfileService
    {
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly ISkillRepository _skillRepository;
        private readonly IApplicantSkillService _applicantSkillService;
        private readonly IApplicantSkillRepository _applicantSkillRepository;
        private readonly IApplicantProfileRepository _applicantProfileRepository;
        
        public ApplicantProfileService(
            AppDbContext appDbContext,
            IHttpContextAccessor httpContextAccessor,
            IUserAccountRepository userAccountRepository,
            ISkillRepository skillRepository,
            IApplicantSkillService applicantSkillService,
            IApplicantSkillRepository applicantSkillRepository,
            IApplicantProfileRepository applicantProfileRepository
        ) : base(httpContextAccessor, userAccountRepository)
        {
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
            _userAccountRepository = userAccountRepository;
            _skillRepository = skillRepository;
            _applicantSkillService = applicantSkillService;
            _applicantSkillRepository = applicantSkillRepository;
            _applicantProfileRepository = applicantProfileRepository;
        }

        private async Task<ApplicantProfile> GetByIdOrThrowAsync(int id)
        {
            var profile = await _applicantProfileRepository.GetByIdAsync(id);

            if (profile is null)
                throw new NotFoundException($"Applicant profile with id '{id}' does not exist.");
            
            return profile;
        }

        private static ApplicantProfileDto ToDto(
            ApplicantProfile applicantProfile, 
            List<ApplicantSkillDto>? applicantSkillsDto = null
        )
        {
            return new ApplicantProfileDto
            {
                Id = applicantProfile.Id,
                UserAccountId = applicantProfile.UserAccountId,
                Headline = applicantProfile.Headline,
                HomeLocation = applicantProfile.HomeLocation,
                Bio = applicantProfile.Bio,
                CreatedAt = applicantProfile.CreatedAt,
                Skills = applicantSkillsDto ?? []
            };
        }

        public async Task<ApplicantProfileDto> GetByIdAsync(int id)
        {
            var profile = await GetByIdOrThrowAsync(id);
            var skills = await _applicantSkillService.GetAllByApplicantProfileIdAsync(profile.Id);
            return ToDto(profile, skills);
        }

        public async Task<ApplicantProfileDto?> GetByUserAccountIdAsync(int userAccountId)
        {
            var profile = await _applicantProfileRepository.GetByUserAccountIdAsync(userAccountId);

            if (profile is null)
                return null;

            var skills = await _applicantSkillService.GetAllByApplicantProfileIdAsync(profile.Id);
            return ToDto(profile, skills);
        }

        public async Task<ApplicantProfileDto> CreateAsync(ApplicantProfileCreateDto request)
        {
            using var transaction = await _appDbContext.Database.BeginTransactionAsync();
            
            try
            {
                var userAccount = await GetCurrentUser();
                var existingProfile = await _applicantProfileRepository.GetByUserAccountIdAsync(userAccount.Id);

                if (existingProfile is not null)
                    throw new ConflictException("You already have a profile.");
                
                var newProfile = new ApplicantProfile
                {
                    UserAccountId = userAccount.Id,
                    Headline = request.Headline,
                    HomeLocation = request.HomeLocation,
                    Bio = request.Bio
                };

                await _applicantProfileRepository.AddAsync(newProfile);
                await _applicantProfileRepository.SaveChangesAsync();

                foreach (var skill in request.Skills)
                {
                    var newApplicantSkill = new ApplicantSkill();

                    if (skill.SkillId is null)
                    {
                        var jobClassification = new JobClassification();
                        var jobSubClassification = new JobSubClassification();
                        
                        var newSkill = new Skill
                        {
                            JobClassificationId = skill?.Skill?.JobClassificationId,
                            JobSubClassificationId = skill?.Skill?.JobSubClassificationId,
                            Name = skill!.Skill!.Name
                        };

                        await _skillRepository.AddAsync(newSkill);
                        await _skillRepository.SaveChangesAsync();

                        newApplicantSkill.ApplicantProfileId = newProfile.Id;
                        newApplicantSkill.SkillId = newSkill.Id;
                        newApplicantSkill.ProficiencyLevel = EnumHandler.Parse<ProficiencyLevel>(skill.ProficiencyLevel);
                        newApplicantSkill.YearsOfExperience = skill.YearsOfExperience;
                    }
                    else
                    {
                        var existingSkill = await _skillRepository.GetByIdAsync((int)skill.SkillId);
                        
                        if (existingSkill is null)
                            throw new NotFoundException($"Skill with id '{skill.SkillId}' does not exist.");
                                        
                        newApplicantSkill.ApplicantProfileId = userAccount.Id;
                        newApplicantSkill.SkillId = (int)skill.SkillId;
                        newApplicantSkill.ProficiencyLevel = EnumHandler.Parse<ProficiencyLevel>(skill.ProficiencyLevel);
                        newApplicantSkill.YearsOfExperience = skill.YearsOfExperience;
                    }
                    
                    await _applicantSkillRepository.AddAsync(newApplicantSkill);
                    await _applicantSkillRepository.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                var applicantSkills = await _applicantSkillService.GetAllByApplicantProfileIdAsync(newProfile.Id);
                return ToDto(newProfile, applicantSkills);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
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