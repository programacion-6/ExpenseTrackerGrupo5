using Api.Domain;

namespace Api.Application;

public class BudgetDecreaseTracker : BaseTrackerChain<Budget>
{
    private const int Percentage = 80;
    private const decimal PercentageAsDecimal = 0.8m;
    private readonly string _userEmail;
    private readonly INotifier<EmailContent> _notifier;

    public BudgetDecreaseTracker(INotifier<EmailContent> notifier, string userEmail)
    {
        _notifier = notifier;
        _userEmail = userEmail;
    }

    public override async Task NotifyTracking(Budget budget)
    {
        if (IsDecresed(budget))
        {
            var email = EmailTemplateGenerator.GetBudgetDecreaseEmail(_userEmail, Percentage);
            await _notifier.Notify(email);
        }
        else
        {
            await base.NotifyTracking(budget);
        }
    }

    public bool IsDecresed(Budget budget)
    {
        var isEmpty = budget.Amount == 0;
        if (isEmpty)
        {
            return false;
        }

        decimal spentAmount = budget.Amount - budget.CurrentAmount;
        return spentAmount >= (budget.Amount * PercentageAsDecimal);
    }

}