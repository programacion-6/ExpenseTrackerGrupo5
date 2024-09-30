namespace Api.Domain.Services;

public class ExpenseService : IExpenseService
{
    private readonly IExpenseRepository _expenseRepository;

    public ExpenseService(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<bool> CreateAsync(Expense expense)
    {
        return await _expenseRepository.Save(expense);
    }

    public async Task<bool> UpdateAsync(Expense expense)
    {
        return await _expenseRepository.Update(expense);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var expense = _expenseRepository.GetById(id);
        return await _expenseRepository.Delete(await expense);
    }

    public async Task<Expense> GetByIdAsync(Guid id)
    {
        return await _expenseRepository.GetById(id);
    }

    public async Task<IEnumerable<Expense>> GetAllByUserAsync(Guid userId)
    {
        return await _expenseRepository.GetAllByUser(userId);
    }

    public async Task<IEnumerable<Expense>> GetUserExpensesByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
    {
        return _expenseRepository.GetUserExpenseByDateRange(userId, startDate, endDate);
    }

    public async Task<IEnumerable<Expense>> GetUserExpensesByCategoryAsync(Guid userId, string category)
    {
        return _expenseRepository.GetUserExpenseByCategory(userId, category);
    }

    public async Task<Guid?> GetUserIdByExpenseId(Guid expenseId)
    {
        return await _expenseRepository.GetUserIdByExpenseId(expenseId);
    }
}