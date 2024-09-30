
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
        var budget = await _budgetService.GetUserBudgetByMonthOrCreate(expense.UserId, expense.Date);
        budget.CurrentAmount += expense.Amount;
        await NotifyTrackingToUser(budget, userEmail);
    }

    public async Task TrackUpdatedUserEntry(Expense oldExpense, Expense newExpense, string userEmail)
    {
        if (DateChecker.AreSameDate(oldExpense.Date, newExpense.Date))
        {
            await TrackUpdatedIncomeWithSameDate(oldExpense, newExpense, userEmail);
        }
        else
        {
            await TrackUpdatedIncomeWithDifferentDate(oldExpense, newExpense, userEmail);
        }
    }

    private async Task TrackUpdatedIncomeWithSameDate(Expense oldExpense, Expense newExpense, string userEmail)
    {
        var budget = await _budgetService.GetUserBudgetByMonthOrCreate(oldExpense.UserId, oldExpense.Date);
        budget.CurrentAmount += oldExpense.Amount;
        budget.CurrentAmount -= newExpense.Amount;
        await NotifyTrackingToUser(budget, userEmail);
    }

    private async Task TrackUpdatedIncomeWithDifferentDate(Expense oldExpense, Expense newExpense, string userEmail)
    {
        var oldBudget = await _budgetService.GetUserBudgetByMonthOrCreate(oldExpense.UserId, oldExpense.Date);
        var newBudget = await _budgetService.GetUserBudgetByMonthOrCreate(newExpense.UserId, newExpense.Date);
        oldBudget.CurrentAmount += oldExpense.Amount;
        newBudget.CurrentAmount -= newExpense.Amount;
        await NotifyTrackingToUser(oldBudget, userEmail);
        await NotifyTrackingToUser(newBudget, userEmail);
    }

    public async Task NotifyTrackingToUser(Budget budget, string userEmail)
    {
        await _budgetService.UpdateUserBudget(budget);

        if (DateChecker.IsCurrent(budget.Month))
        {
            var budgetTracker = new TrackingNotifierChainClient(_notifier, userEmail);
            budgetTracker.Handle(budget);
        }
    }
}