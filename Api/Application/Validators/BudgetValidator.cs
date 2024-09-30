using Api.Domain;

using FluentValidation;

namespace Api.Application;

public class BudgetValidator : AbstractValidator<Budget>
{
    public BudgetValidator()
    {
        RuleFor(b => b.Currency)
            .NotEmpty()
            .WithMessage("Currency is required.")
            .MaximumLength(3);

        RuleFor(b => b.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero.")
            .PrecisionScale(10, 2, false);
            
        RuleFor(b => b.CurrentAmount)
            .PrecisionScale(10, 2, false);
    }
}