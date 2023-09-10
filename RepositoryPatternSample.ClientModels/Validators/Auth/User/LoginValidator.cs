using FluentValidation;
using RepositoryPatternSample.ClientModels.Models.Auth.User;

namespace RepositoryPatternSample.ClientModels.Validators.Auth.User
{
    public class LoginValidator : AbstractValidator<LoginVm>
    {
        public LoginValidator()
        {
            RuleFor(obj => obj.UserName)
                .NotEmpty().WithMessage("{PropertyName} is required")
                  .Must(value => value == value.ToLower()).WithMessage("{PropertyName} can only contain lowercase letters.")
                  .MaximumLength(100)
                  .Must(value => !value.Contains(" "))
                  .WithMessage("{PropertyName} cannot contain spaces.");

            RuleFor(obj => obj.Password)
                  .NotEmpty().WithMessage("{PropertyName} is required")
                  .MaximumLength(70);
        }
    }
}
