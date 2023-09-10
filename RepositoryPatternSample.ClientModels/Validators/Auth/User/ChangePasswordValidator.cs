using FluentValidation;
using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;

namespace RepositoryPatternSample.ClientModels.Validators.Auth.User
{
    public class ChangePasswordValidator : AbstractValidator<ChangePasswordVm>
    {
        public ChangePasswordValidator()
        {
            RuleFor(obj => obj.OldPassword)
                .NotEmpty().WithMessage("{PropertyName}  is required")
                .NotEqual(x => x.NewPassword).WithMessage("New password must not be the same as old password!");

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
