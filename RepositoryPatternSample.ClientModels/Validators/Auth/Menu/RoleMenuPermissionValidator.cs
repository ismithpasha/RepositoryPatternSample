using FluentValidation;
using RepositoryPatternSample.ClientModels.Models.Auth.Role;

namespace RepositoryPatternSample.ClientModels.Validators.Auth.Menu
{
    public class RoleMenuPermissionValidator : AbstractValidator<RoleMenuPermissionVm>
    {
        public RoleMenuPermissionValidator()
        {
            RuleFor(obj => obj.RoleId).NotEmpty().WithMessage("{PropertyName} is required");
            RuleFor(x => x.MenuIds).Must(x => x.Any()).WithMessage("At least one MenuId is required");
            RuleForEach(x => x.MenuIds).GreaterThan(0);
        }
    }
}
