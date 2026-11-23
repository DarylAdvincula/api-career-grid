using CG.DTO.ApplicantSkill;
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

            RuleFor(x => x.Skill!)
                .SetValidator(new SkillCreateValidator())
                .When(x => x.Skill is not null);
        }
    }
}