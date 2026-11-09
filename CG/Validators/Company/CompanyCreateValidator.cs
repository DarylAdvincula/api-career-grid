using CG.DTO.Company;
using CG.DTO.UserAccount;
using FluentValidation;

namespace CG.Validators.Company
{
    public class CompanyCreateValidator : AbstractValidator<CompanyCreateDto>
    {
        public CompanyCreateValidator(IValidator<UserAccountCreateDto> userAccountValidator)
        {
            Include(userAccountValidator);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Description)
                .NotEmpty()
                .MaximumLength(1500);

            RuleFor(x => x.WebsiteUrl)
                .NotEmpty()
                .MaximumLength(2083)
                .Must(BeAValidUrl)
                .WithMessage("Please provide a valid website URL.");

            RuleFor(x => x.LogoUrl)
                .NotEmpty()
                .MaximumLength(2083)
                .Must(BeAValidUrl)
                .WithMessage("Please provide a valid logo URL.");
        }

        private static bool BeAValidUrl(string value)
        {
            return Uri.TryCreate(value, UriKind.Absolute, out var uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
        }
    }
}