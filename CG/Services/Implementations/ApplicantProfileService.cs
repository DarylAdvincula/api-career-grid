using CG.DAL;
using CG.DTO.ApplicantProfile;
using CG.DTO.ApplicantSkill;
using CG.DTO.Resume;
using CG.Exceptions;
using CG.Helpers.Implementations;
using CG.Helpers.Interfaces;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class ApplicantProfileService : BaseApplicantService, IApplicantProfileService
    {
        private readonly ISystemConfiguration _systemConfiguration;
        private readonly AppDbContext _appDbContext;
        private readonly ISkillRepository _skillRepository;
        private readonly IApplicantSkillService _applicantSkillService;
        private readonly IApplicantSkillRepository _applicantSkillRepository;
        private readonly IApplicantProfileRepository _applicantProfileRepository;
        private readonly IResumeRepository _resumeRepository;
        private readonly IResumeService _resumeService;
        
        public ApplicantProfileService(
            ISystemConfiguration systemConfiguration,
            AppDbContext appDbContext,
            IHttpContextAccessor httpContextAccessor,
            IUserAccountRepository userAccountRepository,
            ISkillRepository skillRepository,
            IApplicantSkillService applicantSkillService,
            IApplicantSkillRepository applicantSkillRepository,
            IApplicantProfileRepository applicantProfileRepository,
            IResumeRepository resumeRepository,
            IResumeService resumeService
        ) : base(
            httpContextAccessor, 
            userAccountRepository,
            applicantProfileRepository
        )
        {
            _systemConfiguration = systemConfiguration;
            _appDbContext = appDbContext;
            _skillRepository = skillRepository;
            _applicantSkillService = applicantSkillService;
            _applicantSkillRepository = applicantSkillRepository;
            _applicantProfileRepository = applicantProfileRepository;
            _resumeRepository = resumeRepository;
            _resumeService = resumeService;
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
            List<ApplicantSkillDto>? applicantSkillsDto = null,
            List<ResumeDto>? resumesDto = null
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
                Skills = applicantSkillsDto ?? [],
                Resumes = resumesDto ?? []
            };
        }

        public async Task<ApplicantProfileDto> GetByIdAsync(int id)
        {
            var profile = await GetByIdOrThrowAsync(id);
            var skills = await _applicantSkillService.GetAllByApplicantProfileIdAsync(profile.Id);
            var resumes = await _resumeService.GetAllByApplicantProfileIdAsync(profile.Id);
            return ToDto(profile, skills, resumes);
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
                var userAccount = await GetCurrentUserAsync();
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
                }

                await _applicantSkillRepository.SaveChangesAsync();
                
                var resumeCount = request.Resumes.Count;
                var primaryResumeCount = request.Resumes.Count(x => x.IsPrimary);

                if (resumeCount == 0)
                    throw new BadRequestException("Please upload at least one resume.");

                // ensure that the applicant has always a primary resume
                if (primaryResumeCount == 0)
                {
                    if (resumeCount == 1)
                        request.Resumes[0].IsPrimary = true;
                    else
                        throw new BadRequestException("Please set at least one primary resume.");
                }

                foreach (var resume in request.Resumes)
                {
                    var uploadFileValidator = new UploadFileValidator<Resume>(resume.ResumeFile);

                    uploadFileValidator.EnsureValidOrThow();

                    var uploadFileMetadataExtractor = new UploadFileMetadataExtractor(resume.ResumeFile);

                    using var stream = new FileStream(
                        uploadFileMetadataExtractor.GetFilePath(_systemConfiguration.UploadsResumes),
                        FileMode.Create
                    );

                    await resume.ResumeFile.CopyToAsync(stream);

                    var newResume = new Resume
                    {
                        ApplicantProfileId = newProfile.Id,
                        OriginalFileName = uploadFileMetadataExtractor.GetFileName(),
                        StoredFileName = uploadFileMetadataExtractor.GetUniqueFileName(),
                        FilePath = uploadFileMetadataExtractor.GetFilePath(_systemConfiguration.UploadsResumes),
                        FileExtension = uploadFileMetadataExtractor.GetExtension(),
                        ContentType = uploadFileMetadataExtractor.GetContentType(),
                        FileSizeInBytes = uploadFileMetadataExtractor.GetSizeInBytes(),
                        IsPrimary = resume.IsPrimary
                    };

                    await _resumeRepository.AddAsync(newResume);
                }

                await _resumeRepository.SaveChangesAsync();
                await transaction.CommitAsync();

                var applicantSkills = await _applicantSkillService.GetAllByApplicantProfileIdAsync(newProfile.Id);
                var resumes = await  _resumeService.GetAllByApplicantProfileIdAsync(newProfile.Id);
                return ToDto(newProfile, applicantSkills, resumes);
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
                throw new BadRequestException("You haven't created your profile yet. Please create it first.");

            existingProfile.Headline = request.Headline is null
                ? existingProfile.Headline
                : request.Headline;
            existingProfile.HomeLocation = request.HomeLocation is null
                ? existingProfile.HomeLocation
                : request.HomeLocation;
            existingProfile.Bio = request.Bio is null
                ? existingProfile.Bio
                : request.Bio;

            _applicantProfileRepository.Update(existingProfile);
            await _applicantProfileRepository.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            var profile = await GetByIdOrThrowAsync(id);
            
            _applicantProfileRepository.Delete(profile);
            await _applicantProfileRepository.SaveChangesAsync();
        }
    }
}