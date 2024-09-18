using Api.Domain.Entities;

namespace DefaultNamespace;

public interface IBudgetRepository<T> : IRepository<T> where T : EntityBase
{
    Budget GetCurrentBudget();
    Budget GetByMonth(DateTime month);
    List<Budget> GetByAmountRange(decimal minAmount, decimal maxAmount);
}