using FluentValidation;
using RepositoryPatternSample.ClientModels.Models.Auth.Role;

namespace RepositoryPatternSample.ClientModels.Validators.Auth.Role
{
    public class RoleValidator : AbstractValidator<RoleCreateVm>
    {
        public RoleValidator()
        {
            RuleFor(obj => obj.Name).NotEmpty().Length(3, 100);
            RuleFor(obj => obj.Description).MaximumLength(500);
        }
    }

    public class RoleUpdateValidator : AbstractValidator<RoleVm>
    {
        public RoleUpdateValidator(int id)
        {
            RuleFor(obj => obj.Id).NotEmpty().GreaterThan(0)
                 .Equal(id)
                 .WithMessage("{PropertyName} must be same as the provided id.");

            RuleFor(obj => obj.Id).NotEmpty().GreaterThan(0);
            RuleFor(obj => obj.Name).NotEmpty().Length(3, 256);
            RuleFor(obj => obj.Description).MaximumLength(500);
        }
    }
}
