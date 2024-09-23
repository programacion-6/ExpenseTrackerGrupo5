
namespace DefaultNamespace;

public interface IGoalRepository : IRepository<Goal>
{
    List<Goal> GetActiveUserGoals(Guid userId);
}