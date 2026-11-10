using CG.DTO.JobPosting;
using CG.Helpers.Implementations;
using CG.Models;
using FluentValidation;

namespace CG.Validators.JobPosting
{
    public class JobPostingCreateValidator : AbstractValidator<JobPostingCreateDto>
    {
        public JobPostingCreateValidator()
        {
            RuleFor(x => x.JobClassificationId)
                .GreaterThan(0)
                    .WithMessage("Job classification is invalid.")
                    .When(x => x.JobClassificationId.HasValue);
            
            RuleFor(x => x.JobSubClassificationId)
                .GreaterThan(0)
                    .WithMessage("Job sub classification is invalid.")
                    .When(x => x.JobSubClassificationId.HasValue);
            
            RuleFor(x => x.Title)
                .NotEmpty()
                    .WithMessage("Job title is required.")
                .MaximumLength(150);
            
            RuleFor(x => x.RequirementsMarkdown)
                .NotEmpty()
                    .WithMessage("Job requirements are required.")
                .MaximumLength(5000);
            
            RuleFor(x => x.JobType)
                .Must(jobType => EnumHandler.IsValidMember<JobType>(jobType))
                    .WithMessage("Job type is invalid.")
                    .When(x => x.JobType is not null);
            
            RuleFor(x => x.WorkEnvironment)
                .Must(workEnvironment => EnumHandler.IsValidMember<WorkEnvironment>(workEnvironment))
                    .WithMessage("Work environment is invalid.")
                    .When(x => x.WorkEnvironment is not null);

            RuleFor(x => x.Location)
                .MaximumLength(150);

            RuleFor(x => x.MinSalary)
                .GreaterThan(0)
                    .When(x => x.MinSalary.HasValue)
                .LessThanOrEqualTo(x => x.MaxSalary)
                    .When(x => x.MinSalary.HasValue && x.MaxSalary.HasValue);
            
            RuleFor(x => x.MaxSalary)
                .GreaterThan(0)
                    .When(x => x.MaxSalary.HasValue)
                .GreaterThanOrEqualTo(x => x.MinSalary)
                    .When(x => x.MaxSalary.HasValue && x.MinSalary.HasValue);
        }
    }
}