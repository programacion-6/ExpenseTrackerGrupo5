using Api.Domain;

namespace Api.Application;

public class BudgetManagement
{
    private readonly IBudgetService _budgetService;
    private readonly INotifier<EmailContent> _notifier;

    public BudgetManagement(IBudgetService budgetService, INotifier<EmailContent> notifier)
    {
        _budgetService = budgetService;
        _notifier = notifier;
    }

    public async Task ProcessNewIncome(Income income, string userEmail)
    {
        var currentBudget = await _budgetService.GetCurrentUserBudget(income.UserId);
        currentBudget.CurrentAmount += income.Amount;
        await TrackBudget(currentBudget, userEmail);
    }

    private async Task TrackBudget(Budget budget, string userEmail)
    {
        await _budgetService.UpdateUserBudget(budget);
        var budgetTracker = new BudgetTracker(_notifier, userEmail);
        await budgetTracker.Track(budget);
    }

    public async Task ProcessNewExpense(Expense expense, string userEmail)
    {
        var currentBudget = await _budgetService.GetCurrentUserBudget(expense.UserId);
        currentBudget.CurrentAmount -= expense.Amount;
        await TrackBudget(currentBudget, userEmail);
    }
}