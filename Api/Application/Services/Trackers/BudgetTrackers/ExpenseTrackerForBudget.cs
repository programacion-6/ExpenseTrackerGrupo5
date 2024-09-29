
using Api.Domain;

namespace Api.Application;

public class ExpenseTrackerForBudget : ITracker<Expense, Budget>
{
    private readonly IBudgetService _budgetService;
    private readonly INotifier<EmailContent> _notifier;

    public ExpenseTrackerForBudget(IBudgetService budgetService, INotifier<EmailContent> notifier)
    {
        _budgetService = budgetService;
        _notifier = notifier;
    }

    public async Task TrackNewUserEntry(Expense expense, string userEmail)
    {
        var currentBudget = await _budgetService.GetCurrentUserBudget(expense.UserId);
        currentBudget.CurrentAmount -= expense.Amount;
        await NotifyTrackingToUser(currentBudget, userEmail);
    }

    public async Task TrackDeletedUserEntry(Expense expense, string userEmail)
    {
        var currentBudget = await _budgetService.GetCurrentUserBudget(expense.UserId);
        currentBudget.CurrentAmount += expense.Amount;
        await NotifyTrackingToUser(currentBudget, userEmail);
    }

    public async Task TrackUpdatedUserEntry(Expense oldExpense, Expense newExpense, string userEmail)
    {
        var currentBudget = await _budgetService.GetCurrentUserBudget(oldExpense.UserId);
        currentBudget.CurrentAmount += oldExpense.Amount;
        currentBudget.CurrentAmount -= newExpense.Amount;
        await NotifyTrackingToUser(currentBudget, userEmail);
    }

    public async Task NotifyTrackingToUser(Budget budget, string userEmail)
    {
        await _budgetService.UpdateUserBudget(budget);
        var budgetTracker = new TrackingNotifierChainClient(_notifier, userEmail);
        await budgetTracker.Handle(budget);
    }
}