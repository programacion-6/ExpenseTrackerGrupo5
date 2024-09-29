namespace Api.Domain.Validators.Expens;

using FluentValidation;
using Api.Domain;

public class CreateExpenseRequestValidator : AbstractValidator<CreateExpenseRequest>
{
    public CreateExpenseRequestValidator()
    {

        RuleFor(expense => expense.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero.");

        RuleFor(expense => expense.Description)
            .NotEmpty()
            .MaximumLength(250)
            .WithMessage("Description must not be empty and should be less than 250 characters.");
        
        RuleFor(expense => expense.Date)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Date must not be in the future.");
    }
}