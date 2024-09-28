namespace Api.Domain;

public interface IBudgetRepository : IRepository<Budget>
{
    public Task<Budget?> GetUserBudgetByMonth(Guid userId, DateTime month);
}