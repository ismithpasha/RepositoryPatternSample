using FluentValidation;
using RepositoryPatternSample.ClientModels.Models.Auth.User;
using System.Text.RegularExpressions;

namespace RepositoryPatternSample.ClientModels.Validators.Auth.User
{
    public class UserValidator : AbstractValidator<UserVm>
    {
        public UserValidator(bool isEntry, bool? hasPhone = false)
        {
            RuleFor(obj => obj.UserName).NotEmpty()
                .WithMessage("{PropertyName} is required")
                .Length(3, 30)
                .Must(value => !Regex.IsMatch(value, @"[^\w\s]")).WithMessage("{PropertyName} cannot contain special characters")
                .Must(value => !value.Contains("||"))
                .WithMessage("{PropertyName} cannot contain the characters '||'")
                .Must(value => !value.Contains("&&"))
                .WithMessage("{PropertyName} cannot contain the characters '&&'")
                .Must(value => !value.Contains(" "))
                .WithMessage("{PropertyName} cannot contain spaces.")
                .Must(value => value == value.ToLower()).WithMessage("{PropertyName} can only contain lowercase letters.");


            RuleFor(obj => obj.Name).NotEmpty()
                .WithMessage("{PropertyName} is required")
                .Length(3, 70)
                .Must(value => !value.Contains("||"))
                .WithMessage("{PropertyName} cannot contain the characters '||'")
                .Must(value => !value.Contains("&&"))
                .WithMessage("{PropertyName} cannot contain the characters '&&'")
                .Must(value => !value.StartsWith(" ") && !value.EndsWith(" ")).WithMessage("{PropertyName} cannot have leading or trailing spaces.");


            if (hasPhone == true)
                RuleFor(obj => obj.PhoneNumber).NotEmpty()
               .WithMessage("{PropertyName} is required")
               .Length(3, 20)
               .Matches(@"^\+?\d{10,12}$").WithMessage("Invalid {PropertyName} format.")
               .Must(value => !value.Contains("||"))
               .WithMessage("{PropertyName} cannot contain the characters '||'")
               .Must(value => !value.Contains("&&"))
               .WithMessage("{PropertyName} cannot contain the characters '&&'")
               .Must(value => !value.StartsWith(" ") && !value.EndsWith(" ")).WithMessage("{PropertyName} cannot have leading or trailing spaces.");

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

            if (isEntry == true)
                RuleFor(obj => obj.Password).NotEmpty().WithMessage("Your password cannot be empty")
                    .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\w\s]).{6,50}$").WithMessage("Password must include minimum 6 and maximum 50 characters, at least one uppercase letter, one lowercase letter, one number and one special character");

        }

    }
}
