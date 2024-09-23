namespace Api.Domain;

public interface IGoalRepository : IRepository<Goal>
{
    List<Goal> GetActiveUserGoals(Guid userId);
}