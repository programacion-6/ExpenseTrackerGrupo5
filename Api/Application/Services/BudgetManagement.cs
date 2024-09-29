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
        await AlertBudgetTrackingUser(currentBudget, userEmail);
    }

    public async Task ProcessDeleteIncome(Income income)
    {
        var currentBudget = await _budgetService.GetCurrentUserBudget(income.UserId);
        currentBudget.CurrentAmount -= income.Amount;
        await _budgetService.UpdateUserBudget(currentBudget);
    }

    public async Task ProcessUpdatedIncome(Income oldIncome, Income newIncome)
    {
        var currentBudget = await _budgetService.GetCurrentUserBudget(oldIncome.UserId);
        currentBudget.CurrentAmount -= oldIncome.Amount;
        currentBudget.CurrentAmount += newIncome.Amount;
        await _budgetService.UpdateUserBudget(currentBudget);
    }

    public async Task ProcessNewExpense(Expense expense, string userEmail)
    {
        var currentBudget = await _budgetService.GetCurrentUserBudget(expense.UserId);
        currentBudget.CurrentAmount -= expense.Amount;
        await AlertBudgetTrackingUser(currentBudget, userEmail);
    }

    public async Task ProcessDeleteExpense(Expense expense)
    {
        var currentBudget = await _budgetService.GetCurrentUserBudget(expense.UserId);
        currentBudget.CurrentAmount += expense.Amount;
        await _budgetService.UpdateUserBudget(currentBudget);
    }

    public async Task ProcessUpdatedExpense(Expense oldExpense, Expense newExpense)
    {
        var currentBudget = await _budgetService.GetCurrentUserBudget(oldExpense.UserId);
        currentBudget.CurrentAmount += oldExpense.Amount;
        currentBudget.CurrentAmount -= newExpense.Amount;
        await _budgetService.UpdateUserBudget(currentBudget);
    }

    private async Task AlertBudgetTrackingUser(Budget budget, string userEmail)
    {
        await _budgetService.UpdateUserBudget(budget);
        var budgetTracker = new BudgetTracker(_notifier, userEmail);
        await budgetTracker.Track(budget);
    }


}