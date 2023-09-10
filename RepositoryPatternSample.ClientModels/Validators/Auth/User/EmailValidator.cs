using FluentValidation;

namespace RepositoryPatternSample.ClientModels.Validators.Auth.User
{
    public class EmailValidator : AbstractValidator<string>
    {
        public EmailValidator()
        {

            RuleFor(email => email)
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

        }
    }

}
