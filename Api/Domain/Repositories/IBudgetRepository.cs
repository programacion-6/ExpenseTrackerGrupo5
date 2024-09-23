
namespace DefaultNamespace;

public interface IBudgetRepository : IRepository<Budget>
{
    Budget GetUserBudgetByMonth(Guid userId, DateTime month);
}