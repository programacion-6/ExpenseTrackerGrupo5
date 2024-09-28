namespace Api.Domain;

public interface IBudgetService
{
    public Task<Budget> AddEmptyMonthlyUserBudget(Guid userId);
    public Task AddMonthlyUserBudget(Budget budget);
    public Task UpdateUserBudget(Budget budget);
    public Task<Budget> GetCurrentUserBudget(Guid userId);
    public Task<Budget?> GetUserBudgetByMonth(Guid? userId, DateTime month);
}