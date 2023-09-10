using FluentValidation;
using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;

namespace RepositoryPatternSample.ClientModels.Validators.Auth.User
{
    public class ForgetPasswordValidator : AbstractValidator<ForgetPasswordVm>
    {
        public ForgetPasswordValidator()
        {
            RuleFor(obj => obj.Email)
                  .EmailAddress()
                  .WithMessage("A valid {PropertyName} is required!")
                  .Length(3, 70)
                  .Must(value => !value.Contains("||"))
                  .WithMessage("{PropertyName} cannot contain the characters '||'")
                  .Must(value => !value.Contains("&&"))
                  .WithMessage("{PropertyName} cannot contain the characters '&&'")
                  .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]{3}$")
                  .WithMessage("Email must have a valid domain.")
                  .Must(value => !value.Contains(" "))
                  .WithMessage("{PropertyName} cannot contain spaces.")
                  .Must(value => !value.StartsWith(" ") && !value.EndsWith(" ")).WithMessage("{PropertyName} cannot have leading or trailing spaces.")
                   .Must(value => value == value.ToLower()).WithMessage("{PropertyName} can only contain lowercase letters.");

            RuleFor(obj => obj.NewPassword)
                  .NotEmpty()
                  .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,50}$").WithMessage("Password must include minimum 8 and maximum 50 characters, at least one uppercase letter, one lowercase letter, one number and one special character");

            RuleFor(x => x.ConfirmNewPassword)
                  .NotEmpty().WithMessage("{PropertyName}  is required!")
                  .Must((x, ConfirmNewPassword) => x.NewPassword.Equals(ConfirmNewPassword))
                  .WithMessage("'New Password' and 'Confirm Password' don't match!");
        }
    }
}
