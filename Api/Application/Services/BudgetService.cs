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
        var currentUserBudget = await GetCurrentUserBudget(userId);
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

            await AddUserBudget(emptyUserBudget);
            return emptyUserBudget;
        }

        return currentUserBudget;
    }


    public async Task AddUserBudget(Budget budget)
    {
        var wasSaved = await _budgetRepository.Save(budget);
        if (!wasSaved)
        {
            throw new Exception("An error occurred while saving the budget");
        }
    }

    public async Task DeleteCurrentUserBudget(Guid? userId)
    {
        if (!userId.HasValue)
        {
            throw new BudgetException("Budget not found");
        }

        var budgetFound = await GetCurrentUserBudget(userId.Value);
        if (budgetFound is null)
        {
            throw new BudgetException("Budget not found");
        }

        if (budgetFound.UserId != userId)
        {
            throw new UserBudgetException("Permission denied");
        }

        var wasDeleted = await _budgetRepository.Delete(budgetFound);
        if (!wasDeleted)
        {
            throw new Exception("An error occurred while deleting the budget");
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
        var budgetFound = await _budgetRepository.GetById(budget.Id);
        if (budgetFound is null)
        {
            throw new BudgetException("Budget not found");
        }

        if (budgetFound.UserId != budget.UserId)
        {
            throw new UserBudgetException("Permission denied");
        }

        var wasDeleted = await _budgetRepository.Update(budgetFound);
        if (!wasDeleted)
        {
            throw new Exception("An error occurred while deleting the budget");
        }
    }
}