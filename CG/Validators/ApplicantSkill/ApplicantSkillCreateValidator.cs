using CG.DTO.ApplicantSkill;
using CG.Helpers.Implementations;
using CG.Models;
using CG.Validators.Skill;
using FluentValidation;

namespace CG.Validators.ApplicantSkill
{
    public class ApplicantSkillCreateValidator : AbstractValidator<ApplicantSkillCreateDto>
    {
        public ApplicantSkillCreateValidator()
        {
            RuleFor(x => x.SkillId)
                .GreaterThan(0)
                    .WithMessage("Skill is invalid.")
                    .When(x => x.SkillId.HasValue);
            
            RuleFor(x => x.YearsOfExperience)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Years of experience must not be less than zero.")
                    .When(x => x.YearsOfExperience.HasValue);
            
            RuleFor(x => x.ProficiencyLevel)
                .Must(proficiencyLevel => EnumHandler.IsValidMember<ProficiencyLevel>(proficiencyLevel))
                    .WithMessage("Proficiency level is invalid");

            RuleFor(x => x.Skill!)
                .SetValidator(new SkillCreateValidator())
                .When(x => x.Skill is not null);
        }
    }
}