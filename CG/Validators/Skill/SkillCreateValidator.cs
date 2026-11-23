using CG.DTO.Skill;
using FluentValidation;

namespace CG.Validators.Skill
{
    public class SkillCreateValidator : AbstractValidator<SkillCreateDto>
    {
        public SkillCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                    .WithMessage("Name is required")
                .MaximumLength(100);
        }
    }
}