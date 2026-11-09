using CG.DTO.ApplicantProfile;
using CG.Validators.ApplicantSkill;
using CG.Validators.Skill;
using FluentValidation;

namespace CG.Validators.Applicant
{
    public class ApplicantProfileCreateValidator : AbstractValidator<ApplicantProfileCreateDto>
    {
        public ApplicantProfileCreateValidator()
        {
            RuleFor(x => x.Headline)
                .MaximumLength(150);
            
            RuleFor(x => x.HomeLocation)
                .MaximumLength(150);
            
            RuleFor(x => x.Bio)
                .MaximumLength(1500);

            RuleForEach(x => x.Skills)
                .SetValidator(new ApplicantSkillCreateValidator());
        }
    }
}