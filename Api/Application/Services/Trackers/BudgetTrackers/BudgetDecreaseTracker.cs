using Api.Domain;

namespace Api.Application;

public class BudgetDecreaseTracker : BaseTrackerChain<Budget>
{
    private const decimal IncresePercentage = 0.80m;
    private readonly string _userEmail;
    private readonly INotifier<EmailContent> _notifier;

    public BudgetDecreaseTracker(INotifier<EmailContent> notifier, string userEmail)
    {
        _notifier = notifier;
        _userEmail = userEmail;
    }

    public override async Task Handle(Budget budget)
    {
        if (IsDecresed(budget))
        {
            var percent = IncresePercentage * 100;
            var email = EmailTemplateGenerator.GetBudgetDecreaseEmail(_userEmail, percent);
            await _notifier.Notify(email);
        }
        else
        {
            await base.Handle(budget);
        }
    }

    public bool IsDecresed(Budget budget)
    {
        var isEmpty = budget.Amount == 0;
        if (isEmpty)
        {
            return false;
        }


        var basePercent = budget.Amount * IncresePercentage;

        return budget.CurrentAmount > basePercent;
    }

}