using Api.Domain;

namespace Api.Application;

public class BudgetService : IBudgetService
{
    private readonly IBudgetRepository _budgetRepository;

    public BudgetService(IBudgetRepository budgetRepository)
    {
        _budgetRepository = budgetRepository;
    }

    public async Task<Budget> AddEmptyMonthlyUserBudget(Guid userId)
    {
        var currentMonth = DateTime.Today;
        var currentUserBudget = await _budgetRepository.GetUserBudgetByMonth(userId, currentMonth);
        if (currentUserBudget is null)
        {
            var emptyUserBudget = new Budget()
            {
                UserId = userId,
                Amount = 0,
                CurrentAmount = 0,
                Currency = "BS",
                Month = DateTime.Today
            };

            var wasSaved = await _budgetRepository.Save(emptyUserBudget);

            return !wasSaved
                ? throw new Exception("An error occurred while saving the budget")
                : emptyUserBudget;
        }

        return currentUserBudget;
    }

    public async Task AddMonthlyUserBudget(Budget budget)
    {
        var currentMonth = DateTime.Today;
        var currentUserBudget = await _budgetRepository.GetUserBudgetByMonth(budget.UserId, currentMonth);

        if (currentUserBudget is not null && currentUserBudget.UserId != budget.UserId)
        {
            throw new UserBudgetException("Permission denied");
        }

        if (currentUserBudget is not null)
        {
            currentUserBudget.Amount = budget.Amount;
            currentUserBudget.CurrentAmount += budget.Amount;
            var wasUpdated = await _budgetRepository.Update(currentUserBudget);
            if (!wasUpdated)
            {
                throw new Exception("An error occurred while updatading the budget");
            }
        } else {
            var wasSaved = await _budgetRepository.Save(budget);
            if (!wasSaved)
            {
                throw new Exception("An error occurred while saving the budget");
            }
        }
    }

    public async Task<Budget> GetCurrentUserBudget(Guid userId)
    {
        var currentMonth = DateTime.Today;
        var currentUserBudget = await _budgetRepository.GetUserBudgetByMonth(userId, currentMonth);
        return currentUserBudget is null
            ? await AddEmptyMonthlyUserBudget(userId)
            : currentUserBudget;
    }


    public async Task<Budget?> GetUserBudgetByMonth(Guid? userId, DateTime month)
    {
        if (userId.HasValue)
        {
            var currentUserBudget = await _budgetRepository.GetUserBudgetByMonth(userId.Value, month);

            return currentUserBudget;
        }

        return default;
    }

    public async Task UpdateUserBudget(Budget budget)
    {
        var wasUpdated = await _budgetRepository.Update(budget);
        if (!wasUpdated)
        {
            throw new Exception("Budget not found");
        }
    }
}