using FluentValidation;
using RepositoryPatternSample.ClientModels.Models.Auth.Role;

namespace RepositoryPatternSample.ClientModels.Validators.Auth.Role
{
    public class UserRolePermissionValidator : AbstractValidator<UserRoleVm>
    {
        public UserRolePermissionValidator()
        {
            RuleFor(obj => obj.UserId).NotEmpty()
                .WithMessage("{PropertyName} is required");

            RuleFor(obj => obj.RoleId).NotEmpty()
                .WithMessage("{PropertyName} is required");

        }
    }
}
