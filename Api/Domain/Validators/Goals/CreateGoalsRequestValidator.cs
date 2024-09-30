namespace Api.Domain.Validators;

using FluentValidation;
using Api.Domain;

public class CreateGoalsRequestValidator : AbstractValidator<CreateGoalRequest>
{
    public CreateGoalsRequestValidator()
    {
        RuleFor(goals => goals.Currency)
            .NotEmpty()
            .MaximumLength(3);

        RuleFor(goals => goals.goal_amount)
            .GreaterThan(0)
            .NotEmpty();

        RuleFor(goals => goals.Deadline)
            .NotEmpty();
    }
}