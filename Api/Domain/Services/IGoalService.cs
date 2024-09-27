namespace Api.Domain.Services;

public interface IGoalService
{
    public Task<bool> CreateAsync(Goal goal);
    public Task<bool> UpdateAsync(Goal goal);
    public Task<bool> DeleteAsync(Guid id);
    public Task<Goal> GetByIdAsync(Guid id);
    public Task<IEnumerable<Goal>> GetAllAsync();
    public Task<List<Goal>> GetActiveUserGoals(Guid userId);
}