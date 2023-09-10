using FluentValidation;
using RepositoryPatternSample.ClientModels.Models.Auth.User;
using System.Text.RegularExpressions;

namespace RepositoryPatternSample.ClientModels.Validators.Auth.User
{
    public class UserTypeValidator : AbstractValidator<UserTypeVm>
    {
        public UserTypeValidator()
        {
            RuleFor(obj => obj.Name).NotEmpty()
                 .WithMessage("{PropertyName} is required")
                 .Length(3, 100);
            RuleFor(obj => obj.Description).MaximumLength(999);
        }
        public UserTypeValidator(int id)
        {
            RuleFor(obj => obj.Id).NotEmpty().GreaterThan(0)
           .Equal(id)
           .WithMessage("{PropertyName} must be same as the provided id.");

            RuleFor(obj => obj.Name).NotEmpty()
                 .WithMessage("{PropertyName} is required")
                 .Length(3, 100);
            RuleFor(obj => obj.Description).MaximumLength(999);
        }
    }
}
