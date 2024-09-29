using Api.Domain;

namespace Api.Application;

public class BudgetIncreaseTracker : BaseTrackerChain<Budget>
{
    private const decimal IncresePercentage = 0.50m;
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
            var percent = IncresePercentage * 100;
            var email = EmailTemplateGenerator.GetBudgetIncreaseEmail(_userEmail, percent);
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

        var basePercent = budget.Amount * IncresePercentage;
        return budget.CurrentAmount >= basePercent;
    }
}