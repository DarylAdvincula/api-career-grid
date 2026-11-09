using CG.DTO.Company;
using CG.DTO.UserAccount;
using CG.Helpers.Implementations;
using CG.Models;
using FluentValidation;

namespace CG.Validators.Company
{
    public class CompanyJoinValidator : AbstractValidator<CompanyJoinDto>
    {
        public CompanyJoinValidator(IValidator<UserAccountCreateDto> userAccountValidator)
        {
            Include(userAccountValidator);

            RuleFor(x => x.CompanyId)
                .NotEmpty()
                    .WithMessage("Company is required.")
                .GreaterThan(0)
                    .WithMessage("Company is invalid.");
            
            RuleFor(x => x.CompanyRole)
                .Must(companyRole => EnumHandler.IsValidMember<CompanyRole>(companyRole))
                    .WithMessage("Company role is invalid.");
        }
    }
}