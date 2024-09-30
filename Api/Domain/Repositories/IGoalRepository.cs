namespace Api.Domain;

public interface IGoalRepository : IRepository<Goal>
{
    Task <List<Goal>> GetActiveUserGoals(Guid userId);
    Task<List<Goal>> GetGoalsByUserId(Guid userId);
}