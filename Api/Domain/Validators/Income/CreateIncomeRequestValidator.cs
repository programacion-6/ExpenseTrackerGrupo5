namespace Api.Domain.Validators;

using FluentValidation;
using Api.Domain;

public class CreateIncomeRequestValidator : AbstractValidator<CreateIncomeRequest>
{
    public CreateIncomeRequestValidator()
    {
        RuleFor(income => income.Currency)
            .NotEmpty()
            .MaximumLength(3);

        RuleFor(income => income.Amount)
            .GreaterThan(0);

        RuleFor(income => income.Source)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(income => income.Date)
            .NotEmpty();
    }
}