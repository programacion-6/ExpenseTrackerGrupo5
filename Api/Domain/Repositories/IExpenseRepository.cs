namespace Api.Domain;

public interface IExpenseRepository : IRepository<Expense>
{
    List<Expense> GetUserExpenseByDateRange(Guid userId, DateTime startDate, DateTime endDate);
    List<Expense> GetUserExpenseByCategory(Guid userId, string category);
    Task<List<Expense>> GetAllByUser(Guid userId);
    Task<List<Expense>> GetAllUserExpensesByMonth(Guid userId, DateTime month);
    Task<Guid?> GetUserIdByExpenseId(Guid expenseId);
}