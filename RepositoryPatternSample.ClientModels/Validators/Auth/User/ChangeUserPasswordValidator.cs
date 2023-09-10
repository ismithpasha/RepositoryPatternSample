using FluentValidation;
using RepositoryPatternSample.ClientModels.Models.Admin;

namespace RepositoryPatternSample.ClientModels.Validators.Auth.User
{
    public class ChangeUserPasswordValidator : AbstractValidator<ChangeUserPasswordVm>
    {
        public ChangeUserPasswordValidator()
        {
            RuleFor(obj => obj.ChangeById)
                .NotEmpty().WithMessage("{PropertyName}  is required");

            RuleFor(obj => obj.UserId)
                .NotEmpty().WithMessage("{PropertyName}  is required");


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
