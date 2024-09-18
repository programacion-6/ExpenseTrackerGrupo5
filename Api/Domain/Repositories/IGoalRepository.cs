using Api.Domain.Entities;

namespace DefaultNamespace;

public interface IGoalRepository<T> : IRepository<T> where T : EntityBase
{
    List<Goal> GetCurrentGoals();
    List<Goal> GetByGoalAmountRange(decimal minGoal, decimal maxGoal);
}