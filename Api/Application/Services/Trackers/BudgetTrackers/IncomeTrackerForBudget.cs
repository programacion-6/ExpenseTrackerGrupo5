
using Api.Domain;

namespace Api.Application;

public class IncomeTrackerForBudget : ITracker<Income, Budget>
{
    private readonly IBudgetService _budgetService;
    private readonly INotifier<EmailContent> _notifier;

    public IncomeTrackerForBudget(IBudgetService budgetService, INotifier<EmailContent> notifier)
    {
        _budgetService = budgetService;
        _notifier = notifier;
    }

    public async Task TrackNewUserEntry(Income income, string userEmail)
    {
        var currentBudget = await _budgetService.GetCurrentUserBudget(income.UserId);
        currentBudget.CurrentAmount += income.Amount;
        await NotifyTrackingToUser(currentBudget, userEmail);
    }

    public async Task TrackUpdatedUserEntry(Income oldIncome, Income newIncome, string userEmail)
    {
        var currentBudget = await _budgetService.GetCurrentUserBudget(oldIncome.UserId);
        currentBudget.CurrentAmount -= oldIncome.Amount;
        currentBudget.CurrentAmount += newIncome.Amount;
        await NotifyTrackingToUser(currentBudget, userEmail);
    }

    public async Task TrackDeletedUserEntry(Income income, string userEmail)
    {
        var currentBudget = await _budgetService.GetCurrentUserBudget(income.UserId);
        currentBudget.CurrentAmount -= income.Amount;
        await NotifyTrackingToUser(currentBudget, userEmail);
    }

    public async Task NotifyTrackingToUser(Budget budget, string userEmail)
    {
        await _budgetService.UpdateUserBudget(budget);
        var budgetTracker = new TrackingNotifierChainClient(_notifier, userEmail);
        budgetTracker.Handle(budget);
    }

}