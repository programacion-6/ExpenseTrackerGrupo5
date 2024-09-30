namespace Api.Domain.Services;

public class GoalService : IGoalService
{
    private readonly IGoalRepository _goalRepository;

    public GoalService(IGoalRepository goalRepository)
    {
        _goalRepository = goalRepository;
    }

    public async Task<bool> CreateAsync(Goal goal)
    {
        return await _goalRepository.Save(goal);
    }

    public async Task<bool> UpdateAsync(Goal goal)
    {
        return await _goalRepository.Update(goal);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var goal = await _goalRepository.GetById(id);
        if (goal != null)
        {
            return await _goalRepository.Delete(goal);
        }
        return false;
    }

    public async Task<Goal> GetByIdAsync(Guid id)
    {
        return await _goalRepository.GetById(id);
    }

    public async Task<IEnumerable<Goal>> GetAllAsync()
    {
        return await _goalRepository.GetAll();
    }

    public async Task<List<Goal>> GetActiveUserGoals(Guid userId)
    {
        return _goalRepository.GetActiveUserGoals(userId);
    }
    public async Task<List<Goal>> GetGoalsByUserId(Guid userId)
    {
        return await _goalRepository.GetGoalsByUserId(userId);
    }
}