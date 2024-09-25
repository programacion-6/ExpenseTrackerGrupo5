using Api.Domain;

using FluentValidation;

namespace Api.Application;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        ValidateName();
        ValidateEmail();
        ValidatePassword();
    }

    private void ValidateName()
    {
        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MinimumLength(3)
            .WithMessage("The name must be at least 3 characters.")
            .MaximumLength(15)
            .WithMessage("The name should not be longer than 15 characters.")
            .Matches(@"^[a-zA-Z\s]+$")
            .WithMessage("The name can only contain letters and spaces.");
    }

    private void ValidateEmail()
    {
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("The email must be in a valid format.")
            .Must(email => !email.Contains(" "))
            .WithMessage("Email cannot contain spaces.");
    }

    private void ValidatePassword()
    {
        RuleFor(user => user.PasswordHash)
            .NotEmpty()
            .WithMessage("Password is required.")
            .MinimumLength(8)
            .WithMessage("The password must be at least 8 characters.")
            .Matches("[A-Z]")
            .WithMessage("The password must contain at least one uppercase letter.")
            .Matches("[a-z]")
            .WithMessage("The password must contain at least one lowercase letter.")
            .Matches("[0-9]")
            .WithMessage("The password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]")
            .WithMessage("The password must contain at least one special character (e.g., @, #, $, etc.).")
            .Must(password => !password.Contains(" "))
            .WithMessage("Password cannot contain spaces.");
    }
}
