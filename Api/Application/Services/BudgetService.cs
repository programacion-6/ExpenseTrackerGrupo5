
using Api.Domain;

namespace Api.Application;

public class BudgetService : IBudgetService
{
    public Task AddUserBudget(Budget budget)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCurrentUserBudget(Guid userId, Guid budgetId)
    {
        throw new NotImplementedException();
    }

    public Task<Budget> GetCurrentUserBudget(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Budget> GetUserBudgetByMonth(Guid userId, DateTime month)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserBudget(Budget budget)
    {
        throw new NotImplementedException();
    }
}