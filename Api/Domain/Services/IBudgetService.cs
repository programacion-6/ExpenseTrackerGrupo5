namespace Api.Domain;

public interface IBudgetService
{
    public Task<Budget> AddEmptyMonthlyUserBudget(Guid userId);
    public Task AddUserBudget(Budget budget);
    public Task UpdateUserBudget(Budget budget);
    public Task DeleteCurrentUserBudget(Guid? userId);
    public Task<Budget> GetCurrentUserBudget(Guid userId);
    public Task<Budget?> GetUserBudgetByMonth(Guid? userId, DateTime month);
}