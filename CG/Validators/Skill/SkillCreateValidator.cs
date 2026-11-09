using CG.DTO.Skill;
using FluentValidation;

namespace CG.Validators.Skill
{
    public class SkillCreateValidator : AbstractValidator<SkillCreateDto>
    {
        public SkillCreateValidator()
        {
            RuleFor(x => x.JobClassificationId)
                .GreaterThan(0)
                    .WithMessage("Job classification is invalid")
                    .When(x => x.JobClassificationId.HasValue);
            
            RuleFor(x => x.JobSubClassificationId)
                .GreaterThan(0)
                    .WithMessage("Job sub classification is invalid.")
                    .When(x => x.JobSubClassificationId.HasValue);

            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Name is required")
                .MaximumLength(100);
        }
    }
}