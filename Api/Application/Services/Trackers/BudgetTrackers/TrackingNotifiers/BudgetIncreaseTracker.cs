using Api.Domain;

namespace Api.Application;

public class BudgetIncreaseTracker : BaseTrackerChain<Budget>
{
    private const int Percentage = 50;
    private const decimal PercentageAsDecimal = 1.5m;
    private readonly string _userEmail;
    private readonly INotifier<EmailContent> _notifier;

    public BudgetIncreaseTracker(INotifier<EmailContent> notifier, string userEmail)
    {
        _notifier = notifier;
        _userEmail = userEmail;
    }

    public override async Task NotifyTracking(Budget budget)
    {
        if (IsIncreased(budget))
        {
            var email = EmailTemplateGenerator.GetBudgetIncreaseEmail(_userEmail, Percentage);
            await _notifier.Notify(email);
        }
        else
        {
            await base.NotifyTracking(budget);
        }
    }

    private bool IsIncreased(Budget budget)
    {
        var isEmpty = budget.Amount == 0;

        if (isEmpty)
        {
            return false;
        }

        return budget.CurrentAmount >= (budget.Amount * PercentageAsDecimal);
    }
}