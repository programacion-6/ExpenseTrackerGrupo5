namespace Api.Domain.Services;

public interface IExpenseService
{
    Task<bool> CreateAsync(Expense expense);
    Task<bool> UpdateAsync(Expense expense);
    Task<bool> DeleteAsync(Guid id);
    Task<Expense> GetByIdAsync(Guid id);
    Task<IEnumerable<Expense>> GetAllByUserAsync(Guid userId);
    Task<IEnumerable<Expense>> GetUserExpensesByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<Expense>> GetUserExpensesByCategoryAsync(Guid userId, string category);
}