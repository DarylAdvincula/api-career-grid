using CG.DTO.UserAccount;
using FluentValidation;

namespace CG.Validators.UserAccount
{
    public class UserAccountCreateValidator : AbstractValidator<UserAccountCreateDto>
    {
        public UserAccountCreateValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(256)
                .EmailAddress();

            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(100)
                .Matches(@"^(?=[^.]*\.?[^.]*$)[A-Za-z]+(?:[ .-][A-Za-z]+)*$")
                .WithMessage("First name must contain only letters, spaces, hyphens, and at most one dot.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(100)
                .Matches(@"^(?=[^.]*\.?[^.]*$)[A-Za-z]+(?:[ .-][A-Za-z]+)*$")
                .WithMessage("Last name must contain only letters, spaces, hyphens, and at most one dot.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8)
                .MaximumLength(65);

            RuleFor(x => x.RepeatedPassword)
                .NotEmpty()
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match.");
        }
    }
}