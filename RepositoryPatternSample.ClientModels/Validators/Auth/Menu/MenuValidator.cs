using FluentValidation;
using RepositoryPatternSample.ClientModels.Models.Auth.Menu;

namespace RepositoryPatternSample.ClientModels.Validators.Auth.Menu
{
    public class MenuCreateValidator : AbstractValidator<MenuCreateVm>
    {
        public MenuCreateValidator()
        {
            RuleFor(obj => obj.Name).NotEmpty()
                .WithMessage("{PropertyName} is required")
                .Length(3, 70);
            RuleFor(obj => obj.Code).NotEmpty()
                .WithMessage("{PropertyName} is required")
                .Length(3, 70);
        }
    }

    public class MenuUpdateValidator : AbstractValidator<MenuVm>
    {
        public MenuUpdateValidator(int id)
        {
            RuleFor(obj => obj.Id).NotEmpty().GreaterThan(0)
              .Equal(id)
                .WithMessage("{PropertyName} must be same as the provided id.");

            RuleFor(obj => obj.Id).NotEmpty().GreaterThan(0);
            RuleFor(obj => obj.Name).NotEmpty()
                .WithMessage("{PropertyName} is required")
                .Length(3, 70);
            RuleFor(obj => obj.Code).NotEmpty()
                .WithMessage("{PropertyName} is required")
                .Length(3, 70);
        }
    }
}
