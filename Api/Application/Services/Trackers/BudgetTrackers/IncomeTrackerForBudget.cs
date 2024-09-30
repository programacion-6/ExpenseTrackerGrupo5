
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
        var budget = await _budgetService.GetUserBudgetByMonthOrCreate(income.UserId, income.Date);
        budget.CurrentAmount += income.Amount;
        await NotifyTrackingToUser(budget, userEmail);
    }

    public async Task TrackUpdatedUserEntry(Income oldIncome, Income newIncome, string userEmail)
    {
        if (DateChecker.AreSameDate(oldIncome.Date, newIncome.Date))
        {
            await TrackUpdatedIncomeWithSameDate(oldIncome, newIncome, userEmail);
        }
        else
        {
            await TrackUpdatedIncomeWithDifferentDate(oldIncome, newIncome, userEmail);
        }
    }

    private async Task TrackUpdatedIncomeWithSameDate(Income oldIncome, Income newIncome, string userEmail)
    {
        var budget = await _budgetService.GetUserBudgetByMonthOrCreate(oldIncome.UserId, oldIncome.Date);
        budget.CurrentAmount -= oldIncome.Amount;
        budget.CurrentAmount += newIncome.Amount;
        await NotifyTrackingToUser(budget, userEmail);
    }

    private async Task TrackUpdatedIncomeWithDifferentDate(Income oldIncome, Income newIncome, string userEmail)
    {
        var oldBudget = await _budgetService.GetUserBudgetByMonthOrCreate(oldIncome.UserId, oldIncome.Date);
        var newBudget = await _budgetService.GetUserBudgetByMonthOrCreate(newIncome.UserId, newIncome.Date);
        oldBudget.CurrentAmount -= oldIncome.Amount;
        newBudget.CurrentAmount += newIncome.Amount;
        await NotifyTrackingToUser(oldBudget, userEmail);
        await NotifyTrackingToUser(newBudget, userEmail);
    }

    public async Task TrackDeletedUserEntry(Income income, string userEmail)
    {
        var budget = await _budgetService.GetUserBudgetByMonthOrCreate(income.UserId, income.Date);
        budget.CurrentAmount -= income.Amount;
        await NotifyTrackingToUser(budget, userEmail);
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