using CG.DAL;
using CG.DTO.ApplicantSkill;
using CG.DTO.Skill;
using CG.Exceptions;
using CG.Helpers.Implementations;
using CG.Models;
using CG.Repositories.Interfaces;
using CG.Services.Interfaces;

namespace CG.Services.Implementations
{
    public class ApplicantSkillService : BaseService, IApplicantSkillService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserAccountRepository _userAccountRepository;
        private readonly IApplicantSkillRepository _applicantSkillRepository;
        private readonly ISkillRepository _skillRepository;

        public ApplicantSkillService(
            AppDbContext context,
            IHttpContextAccessor httpContextAccessor,
            IUserAccountRepository userAccountRepository,
            IApplicantSkillRepository applicantSkillRepository,
            ISkillRepository skillRepository
        ) : base(httpContextAccessor, userAccountRepository)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userAccountRepository = userAccountRepository;
            _applicantSkillRepository = applicantSkillRepository;
            _skillRepository = skillRepository;
        }

        public static ApplicantSkillDto ToDto(ApplicantSkill applicantSkill)
        {
            return new ApplicantSkillDto
            {
                Id = applicantSkill.Id,
                ApplicantProfileId = applicantSkill.ApplicantProfileId,
                SkillId = applicantSkill.SkillId,
                Skill = new SkillDto
                {
                    Id = applicantSkill.Skill.Id,
                    Name = applicantSkill.Skill.Name,
                    JobClassificationId = applicantSkill.Skill.JobClassificationId,
                    JobSubClassificationId = applicantSkill.Skill.JobSubClassificationId,
                    CreatedAt = applicantSkill.Skill.CreatedAt,
                },
                ProficiencyLevel = applicantSkill.ProficiencyLevel,
                YearsOfExperience = applicantSkill.YearsOfExperience,
            };
        }

        private async Task<ApplicantSkill> GetByIdOrThrowAsync(int id)
        {
            var applicantSkill = await _applicantSkillRepository.GetByIdAsync(id);

            if (applicantSkill is null)
                throw new NotFoundException($"Applicant skill with id '{id}' does not exist.");

            return applicantSkill;
        }

        public async Task<ApplicantSkillDto> GetById(int id)
        {
            var applicantSkill = await GetByIdOrThrowAsync(id);
            return ToDto(applicantSkill);
        }

        public async Task<List<ApplicantSkillDto>> GetAllByApplicantProfileIdAsync(int applicantProfileId)
        {
            var applicantSkills = await _applicantSkillRepository.GetAllByApplicantProfileIdAsync(applicantProfileId);
            return applicantSkills.Select(ToDto).ToList();
        }

        public async Task<List<ApplicantSkillDto>> GetAllAsync()
        {
            var applicantSkills = await _applicantSkillRepository.GetAllAsync();
            return applicantSkills.Select(ToDto).ToList();
        }

        public async Task<ApplicantSkillDto> CreateAsync(ApplicantSkillCreateDto request)
        {
            var userAccount = await GetCurrentUser();
            using var transaction = await _context.Database.BeginTransactionAsync();
                
            try
            {
                var newApplicantSkill = new ApplicantSkill();
                
                if (request.SkillId is null)
                {
                    var newSkill = new Skill
                    {
                        JobClassificationId = request?.Skill?.JobClassificationId,
                        JobSubClassificationId = request?.Skill?.JobSubClassificationId,
                        Name = request!.Skill!.Name
                    };

                    await _skillRepository.AddAsync(newSkill);
                    await _skillRepository.SaveChangesAsync();

                    newApplicantSkill.ApplicantProfileId = userAccount.Id;
                    newApplicantSkill.SkillId = newSkill.Id;
                    newApplicantSkill.ProficiencyLevel = EnumHandler.Parse<ProficiencyLevel>(request.ProficiencyLevel);
                    newApplicantSkill.YearsOfExperience = request.YearsOfExperience;
                }
                else
                {
                    var existingSkill = await _skillRepository.GetByIdAsync((int)request.SkillId);
                    
                    if (existingSkill is null)
                        throw new NotFoundException($"Skill with id '{request.SkillId}' does not exist.");
                                    
                    newApplicantSkill.ApplicantProfileId = userAccount.Id;
                    newApplicantSkill.SkillId = (int)request.SkillId;
                    newApplicantSkill.ProficiencyLevel = EnumHandler.Parse<ProficiencyLevel>(request.ProficiencyLevel);
                    newApplicantSkill.YearsOfExperience = request.YearsOfExperience;
                }

                await _applicantSkillRepository.AddAsync(newApplicantSkill);
                await _applicantSkillRepository.SaveChangesAsync();
                await transaction.CommitAsync();
                return ToDto(newApplicantSkill);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ApplicantSkillDto> UpdateAsync(int id, ApplicantSkillUpdateDto request)
        {
            var applicantSkill = await GetByIdOrThrowAsync(id);
            applicantSkill.YearsOfExperience = request.YearsOfExperience;
            applicantSkill.ProficiencyLevel = request.ProficiencyLevel;

            _applicantSkillRepository.Update(applicantSkill);
            await _applicantSkillRepository.SaveChangesAsync();
            return ToDto(applicantSkill);
        }

        public async Task DeleteAsync(int id)
        {
            var applicantSkill = await GetByIdOrThrowAsync(id);
            
            _applicantSkillRepository.Delete(applicantSkill);
            await _applicantSkillRepository.SaveChangesAsync();
        }
    }
}